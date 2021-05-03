#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Karprod;

public class WaterPlaneGeneration : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private MeshFilter filter;

    [Header("Square")]
    [SerializeField] private float size = 16;
    [SerializeField, Min(1)] private int gridSubdivNbr = 32;

    [Header("Rect")]
    [SerializeField] private Vector2 rectSize = new Vector2(16, 16);
    [SerializeField, Min(1)] private Vector2Int rectSubdivNbr = new Vector2Int(32, 32);

    private void Start()
    {
        //GenerateSquareMesh();
    }

    [ContextMenu("MakeSquareMesh")]
    public void GenerateSquareMesh()
    {
        PlaneMeshGenerator.Create("myMesh", true, size);
        filter.mesh = GenerateSquareMesh(size, gridSubdivNbr);
    }

    [ContextMenu("MakeRectMesh")]
    public void GenerateRectangleMesh()
    {
        filter.mesh = GenerateRectangleMesh(rectSize, rectSubdivNbr);
    }

    private Mesh GenerateSquareMesh(float meshSize, int meshSubdivNbr)
    {
        //Empty Mesh
        Mesh m = new Mesh();
        //Mesh future data
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normmals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        //Calcul Data
        float halfSize = meshSize * 0.5f;
        float subdivSize = 1 / (float)meshSubdivNbr;
        int vertLine = meshSubdivNbr + 1;

        for (int x = 0; x <= meshSubdivNbr; x++)
        {
            for (int y = 0; y <= meshSubdivNbr; y++)
            {
                vertices.Add(new Vector3(-halfSize + (meshSize * (subdivSize * x)), 0, -halfSize + (meshSize * (subdivSize * y))));
                normmals.Add(Vector3.up);
                uvs.Add(new Vector2(subdivSize * x, subdivSize * y));
            }
        }

        //Skip la dernière ligne
        for (int i = 0; i < vertLine * vertLine - vertLine; i++)
        {
            //Skip le dernier point d'une ligne
            if ((i + 1) % vertLine == 0)
            {
                continue;
            }

            //create Quad
            triangles.AddRange(MakeSquare(i, vertLine));
        }

        //Applying Mesh Data
        m.SetVertices(vertices);
        m.SetNormals(normmals);
        m.SetUVs(0, uvs);
        m.SetTriangles(triangles, 0);

        return m;
    }
    private Mesh GenerateRectangleMesh(Vector2 meshSize, Vector2Int meshSubdivNbr)
    {
        //Empty Mesh
        Mesh m = new Mesh();
        //Mesh future data
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normmals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        //Calcul Data
        Vector2 halfSize = meshSize * 0.5f;
        Vector2 subdivSize = new Vector2(1 / (float)meshSubdivNbr.x, 1 / (float)meshSubdivNbr.y);
        Vector2Int vertLine = new Vector2Int(meshSubdivNbr.x + 1, meshSubdivNbr.y + 1);

        for (int x = 0; x <= meshSubdivNbr.x; x++)
        {
            for (int y = 0; y <= meshSubdivNbr.y; y++)
            {
                vertices.Add(new Vector3(-halfSize.x + (meshSize.x * (subdivSize.x * x)), 0, -halfSize.y + (meshSize.y * (subdivSize.y * y))));
                normmals.Add(Vector3.up);
                uvs.Add(new Vector2(subdivSize.x * x, subdivSize.y * y));
            }
        }

        //Skip la dernière ligne
        for (int i = 0; i < (vertLine.x * vertLine.y) - vertLine.y; i++)
        {
            //Skip le dernier point d'une ligne
            if ((i + 1) % vertLine.y == 0)
            {
                continue;
            }

            //create Quad
            triangles.AddRange(MakeSquare(i, vertLine.y));
        }

        //Applying Mesh Data
        m.SetVertices(vertices);
        m.SetNormals(normmals);
        m.SetUVs(0, uvs);
        m.SetTriangles(triangles, 0);

        return m;
    }

    #region Méthode for create square
    //Variable Tampon
    private int[] _square = new int[6] { 0, 0, 0, 0, 0, 0 };

    /// <summary> Créer deux triangles qui forme un carré dans le sens horaire </summary>
    /// <param name="point"> Le point du coin supérieur gauche du quad </param>
    /// <param name="vertPerLine"> Le nombre de vertices par ligne </param>
    /// <returns></returns>
    private int[] MakeSquare(int point, int vertPerLine)
    {
        //Reset previous square calculation
        _square = new int[6] { 0, 0, 0, 0, 0, 0 };

        //triangle 1
        #region SchemaTriangle 1
        /* sens horaire
         * 2 
         * |\ 
         * | \ 
         * |  \
         * 1---0 
         */
        #endregion
        _square[0] = point + 1 + vertPerLine;   //basDroite
        _square[1] = point + vertPerLine;       //basGauche
        _square[2] = point;                      //hautGauche

        //triangle 2 
        #region SchemaTriangle 2
        /* sens horaire
         * 1___2
         *  \  |
         *   \ |
         *    \|
         *     3
         */
        #endregion
        _square[3] = point;                      //hautGauche
        _square[4] = point + 1;                  //hautDroite
        _square[5] = point + 1 + vertPerLine;   //basDroite

        return _square;
    }
    #endregion
}
#endif