using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshTool : MonoBehaviour
{

    [Header("Input")]
    public Transform[] edgeTransforms;
    private Vector3[] edges;
    private Vector3[] edgesUp;
    private Vector3[] chamferEdges;
    public float meshHeight = 1f;
    public float chamferHeight = 0.1f;
    public float chamferOffset = 0.1f;
    public GameObject targetGameObjectReference;
    private GameObject target;
    private int verticeIndex = 0;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private Mesh mesh;

    [Header("Output")]
    public string meshOutputName;
    public string meshSavingPath;
    

    [ContextMenu("Process Mesh")]
    private void Initialize()
    {
        vertices.Clear();
        triangles.Clear();

        edges = new Vector3[edgeTransforms.Length];
        edgesUp = new Vector3[edgeTransforms.Length];
        chamferEdges = new Vector3[edgeTransforms.Length];

        float meanX = 0;
        float meanY = 0;
        float meanZ = 0;
        for (int i = 0; i < edgeTransforms.Length; i++)
        {
            meanX += edgeTransforms[i].position.x;
            meanY += edgeTransforms[i].position.y;
            meanZ += edgeTransforms[i].position.z;
        }
        meanX = meanX / edgeTransforms.Length;
        meanY = meanY / edgeTransforms.Length;
        meanZ = meanZ / edgeTransforms.Length;

        Vector3 chamferCenter = new Vector3(meanX, meanY, meanZ) + Vector3.up * meshHeight + Vector3.up * chamferHeight;

        //Set all edges
        for (int i = 0; i < edgeTransforms.Length; i++)
        {
            edges[i] = edgeTransforms[i].position;
            edgesUp[i] = edgeTransforms[i].position + Vector3.up * meshHeight;
            chamferEdges[i] = edgeTransforms[i].position + Vector3.up * meshHeight + Vector3.up * chamferHeight;
        }

        //Offset champfer edges
        for (int i = 0; i < edgeTransforms.Length; i++)
        {
            Vector3 dir = chamferCenter - chamferEdges[i];
            chamferEdges[i] += dir.normalized * chamferOffset;
        }

        mesh = new Mesh();

        target = Instantiate(targetGameObjectReference);
        target.name = meshOutputName;

        target.GetComponent<MeshFilter>().mesh = mesh;

        ProcessData();
    }
    void ProcessData()
    {
        int trianglesCount = edges.Length - 2;
        verticeIndex = 0;
        //Create base face.
        for (int i = 1; i < edges.Length - 1; i++)
        {
            vertices.Add(edges[0]);
            vertices.Add(edges[i]);
            vertices.Add(edges[i + 1]);

            CreateTriangles(1);
            verticeIndex += 3;
        }

        //Create upper champfer face.
        for (int i = 1; i < edges.Length - 1; i++)
        {
            vertices.Add(chamferEdges[0]);
            vertices.Add(chamferEdges[i]);
            vertices.Add(chamferEdges[i+1]);

            CreateTriangles(-1);
            verticeIndex += 3;
        }

        //Create Side faces
        for (int i = 0; i < edges.Length; i++)
        {
            if(i != edges.Length -1)
            {
                vertices.Add(edges[i]);
                vertices.Add(edgesUp[i]);
                vertices.Add(edges[i + 1]);
                vertices.Add(edgesUp[i + 1]);
            }
            else 
            {
                vertices.Add(edges[i]);
                vertices.Add(edgesUp[i]);
                vertices.Add(edges[0]);
                vertices.Add(edgesUp[0]);
            }
                
            CreateTriangleQuad(1);
            verticeIndex += 4;
        }

        //Create Champfer Side faces
        for (int i = 0; i < edges.Length; i++)
        {
            if (i != edges.Length - 1)
            {
                vertices.Add(edgesUp[i]);
                vertices.Add(chamferEdges[i]);
                vertices.Add(edgesUp[i + 1]);
                vertices.Add(chamferEdges[i + 1]);
            }
            else
            {
                vertices.Add(edgesUp[i]);
                vertices.Add(chamferEdges[i]);
                vertices.Add(edgesUp[0]);
                vertices.Add(chamferEdges[0]);
            }

            CreateTriangleQuad(1);
            verticeIndex += 4;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        var collider = target.GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;

        target.isStatic = true;

        SaveMesh(meshSavingPath, meshOutputName);
    }
    private void CreateTriangles(int normal)
    {
        if (normal == 1)
        {
            triangles.Add(0 + verticeIndex);
            triangles.Add(2 + verticeIndex);
            triangles.Add(1 + verticeIndex);
        }
        else if (normal == -1)
        {
            triangles.Add(1 + verticeIndex);
            triangles.Add(2 + verticeIndex);
            triangles.Add(0 + verticeIndex);
        }
    }
    private void CreateTriangleQuad(int normal)
    {
        if (normal == 1)
        {
            triangles.Add(0 + verticeIndex);
            triangles.Add(2 + verticeIndex);
            triangles.Add(1 + verticeIndex);

            triangles.Add(2 + verticeIndex);
            triangles.Add(3 + verticeIndex);
            triangles.Add(1 + verticeIndex);
        }                             
        else if (normal == -1)        
        {                             
            triangles.Add(1 + verticeIndex);
            triangles.Add(2 + verticeIndex);
            triangles.Add(0 + verticeIndex);
                              
            triangles.Add(1 + verticeIndex);
            triangles.Add(3 + verticeIndex);
            triangles.Add(2 + verticeIndex);
        }
    }
    private void SaveMesh(string path, string name)
    {
        string newPath = path;
        newPath += name + ".asset";

        string prefabPath = path;
        prefabPath += name + ".prefab";

        AssetDatabase.CreateAsset(mesh, newPath);
        AssetDatabase.SaveAssets();
        PrefabUtility.SaveAsPrefabAssetAndConnect(target, prefabPath, InteractionMode.AutomatedAction);
    }

}
