#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Karprod
{
    public class PlaneMeshGenerator
    {

        #region AssetCreation
        public static void Create(string name, bool optimising, float size)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }

            //Creating The Mesh
            Mesh mesh;
            mesh = PlaneMeshGenerator.GenerateMesh(size);
            mesh = Object.Instantiate(mesh) as Mesh;

            if (optimising)
            {
                MeshUtility.Optimize(mesh);
            }

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, mesh);

            //Debug.log the test       
            TestResult(path);
        }
        public static void Create(string name, bool optimising, float sizeX, float sizeY)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }

            //Creating The Mesh
            Mesh mesh;
            mesh = PlaneMeshGenerator.GenerateMesh(new Vector2(sizeX, sizeY));
            mesh = Object.Instantiate(mesh) as Mesh;

            if (optimising)
            {
                MeshUtility.Optimize(mesh);
            }

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, mesh);

            //Debug.log the test       
            TestResult(path);
        }
        public static void Create(string name, bool optimising, float size, int subdivisions)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }

            //Creating The Mesh
            Mesh mesh;
            mesh = PlaneMeshGenerator.GenerateMesh(size, subdivisions);
            mesh = Object.Instantiate(mesh) as Mesh;

            if (optimising)
            {
                MeshUtility.Optimize(mesh);
            }

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, mesh);

            //Debug.log the test       
            TestResult(path);
        }
        public static void Create(string name, bool optimising, float sizeX, float sizeY, int subdivX, int subdivY)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }

            //Creating The Mesh
            Mesh mesh;
            mesh = PlaneMeshGenerator.GenerateMesh(new Vector2(sizeX, sizeY), new Vector2Int(subdivX, subdivY));
            mesh = Object.Instantiate(mesh) as Mesh;

            if (optimising)
            {
                MeshUtility.Optimize(mesh);
            }

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, mesh);

            //Debug.log the test       
            TestResult(path);
        }

        // Exemple OBJ creation
        //https://gist.github.com/MattRix/0522c27ee44c0fbbdf76d65de123eeff

        private static string PathAsking(string name)
        {
            string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assets/", name, "png");
            path = FileUtil.GetProjectRelativePath(path);
            return path;
        }
        private static void DeleteCopy(string filePath)
        {
            if ((Mesh)AssetDatabase.LoadAssetAtPath(filePath, typeof(Mesh)) != null)
            {
                Debug.Log("A file have already this name, this file was replace");

                FileUtil.DeleteFileOrDirectory(filePath);

                //Update Folder
                AssetDatabase.Refresh();
            }

        }
        private static void AssetCreation(string filePath, Mesh mesh)
        {
            AssetDatabase.CreateAsset(mesh, filePath);
            //Update Folder
            AssetDatabase.Refresh();
        }
        private static bool TestResult(string filePath)
        {
            if ((Mesh)AssetDatabase.LoadAssetAtPath(filePath, typeof(Mesh)) == null)
            {
                Debug.Log("Couldn't Import");
                return false;
            }
            else
            {
                Debug.Log("Import Successful");
                return true;
            }
        }
        #endregion

        #region Mesh Generation
        //Square
        private static Mesh GenerateMesh(float meshSize)
        {
            //EdgeCase control
            meshSize = meshSize > 0 ? meshSize : 1;

            //Empty Mesh
            Mesh m = new Mesh();
            //Mesh future data
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normmals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            //Calcul Data
            int meshSubdivNbr = Mathf.CeilToInt(meshSize);
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
        private static Mesh GenerateMesh(float meshSize, int meshSubdivNbr)
        {
            //EdgeCase control
            meshSize = meshSize > 0 ? meshSize : 1;
            meshSubdivNbr = meshSubdivNbr >= 1 ? meshSubdivNbr : 1;

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

        //Rectangle
        private static Mesh GenerateMesh(Vector2 meshSize)
        {
            //EdgeCase control
            meshSize.x = meshSize.x > 0 ? meshSize.x : 1;
            meshSize.y = meshSize.y > 0 ? meshSize.y : 1;

            //Empty Mesh
            Mesh m = new Mesh();
            //Mesh future data
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normmals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            //Calcul Data
            Vector2Int meshSubdivNbr = new Vector2Int( Mathf.CeilToInt(meshSize.x), Mathf.CeilToInt(meshSize.y)); 
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
        private static Mesh GenerateMesh(Vector2 meshSize, Vector2Int meshSubdivNbr)
        {
            //EdgeCase control
            meshSize.x = meshSize.x > 0 ? meshSize.x : 1;
            meshSize.y = meshSize.y > 0 ? meshSize.y : 1;
            meshSubdivNbr.x = meshSubdivNbr.x >= 1 ? meshSubdivNbr.x : 1;
            meshSubdivNbr.y = meshSubdivNbr.y >= 1 ? meshSubdivNbr.y : 1;

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

        //Variable Tampon & Methode Square
        private static int[] _square = new int[6] { 0, 0, 0, 0, 0, 0 };
        /// <summary> Créer deux triangles qui forme un carré dans le sens horaire </summary>
        /// <param name="point"> Le point du coin supérieur gauche du quad </param>
        /// <param name="vertPerLine"> Le nombre de vertices par ligne </param>
        /// <returns></returns>
        private static int[] MakeSquare(int point, int vertPerLine)
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
}
#endif
