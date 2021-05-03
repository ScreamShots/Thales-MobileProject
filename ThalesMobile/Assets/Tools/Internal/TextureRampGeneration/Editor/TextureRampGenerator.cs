using System.IO;
using UnityEngine;
using UnityEditor;

namespace Karprod
{
    //Default Size is 256*256px
    public static class TextureRamp
    {
        public static Texture2D Generate(Gradient grd)
        {
            //Var pour répartir le gradient sur la texture
            float hToGrd = (float)1f / 256;

            //Création de la Texture
            Texture2D textureRamp = new Texture2D(256, 256, TextureFormat.ARGB32, false);

            //Pour chaque ligne horizontal
            for (int x = 0; x < 256; x++)
            {
                //Pour chaque pixel vertical
                for (int y = 0; y < 256; y++)
                {
                    textureRamp.SetPixel(x, y, grd.Evaluate(x * hToGrd));
                }
            }

            textureRamp.Apply();

            return textureRamp;
        }
        public static Texture2D Generate(Gradient grd, int width, int height)
        {
            //Var pour répartir le gradient sur la texture
            float hToGrd = (float)1f / width;

            //Création de la Texture
            Texture2D textureRamp = new Texture2D(width, height, TextureFormat.ARGB32, false);

            //Pour chaque ligne horizontal
            for (int x = 0; x < width; x++)
            {
                //Pour chaque pixel vertical
                for (int y = 0; y < height; y++)
                {
                    textureRamp.SetPixel(x, y, grd.Evaluate(x * hToGrd));
                }
            }

            textureRamp.Apply();

            return textureRamp;
        }

        public static void Create(Gradient grd, string name)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }

            //Creating The texture
            Texture2D texture = TextureRamp.Generate(grd, 256, 256);

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, texture);

            //Debug.log the test       
            TestResult(path);
        }
        public static void Create(Gradient grd, string name, int size)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }

            //Creating The texture
            Texture2D texture = TextureRamp.Generate(grd, size, size);

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, texture);

            //Debug.log the test       
            TestResult(path);
        }
        public static void Create(Gradient grd, string name, int width, int height)
        {
            //generate the path
            string path = PathAsking(name);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }
            //Creating The texture
            Texture2D texture = TextureRamp.Generate(grd, width, height);

            //Check for copy
            DeleteCopy(path);

            //create the asset
            AssetCreation(path, texture);

            //Debug.log the test       
            TestResult(path);
        }

        //Create Texture Methodes
        private static string PathAsking(string name)
        {
            string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assets/", name, "png");
            path = FileUtil.GetProjectRelativePath(path);
            return path ;
        }
        private static void DeleteCopy(string filePath)
        {
            if ((Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D)) != null)
            {
                Debug.Log("A file have already this name, this file was replace");

                FileUtil.DeleteFileOrDirectory(filePath);

                //Update Folder
                AssetDatabase.Refresh();
            }

        }
        private static void AssetCreation(string filePath, Texture2D texture)
        {
            FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);

            byte[] bytes = texture.EncodeToPNG();
            for (int i = 0; i < bytes.Length; i++)
            {
                writer.Write(bytes[i]);
            }

            writer.Close();
            stream.Close();

            AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);

            //Update Folder
            AssetDatabase.Refresh();
        }
        private static bool TestResult(string filePath)
        {
            if ((Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D)) == null)
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
    }
}