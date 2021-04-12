using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class SeaTextureGenerator
{
    public static void GenerateSeaTexture(Boundary map, string name)
    {
        //generate the path
        string path = PathAsking(name);
        if (path == null || path == string.Empty)
        {
            Debug.Log("SeaTexture creation have been cancel");
            return;
        }

        #region Set the Texture size
        //detail level must be a multiple of 4 
        int detailLevel = 12;
        if (detailLevel % 4 != 0)
        {
            Debug.LogError("DetailLevel must be a multiple of 4");
            return;
        }

        //Determine quel coté est le plus long
        bool maxIsX = map.size.x > map.size.y;
        float mapMaxLenght = maxIsX ? map.size.x : map.size.y;

        int textureSize = Mathf.CeilToInt(mapMaxLenght * detailLevel);
        int borderEmptySpace = Mathf.CeilToInt((maxIsX ? map.size.y : map.size.x) * detailLevel * 0.5f);

        #endregion
        //Creating The texture
        Texture2D texture = CreateTexture(textureSize);

        //Send Data in
        if (maxIsX)
        {
            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    if (x < borderEmptySpace)
                    {
                        texture.SetPixel(x, y, Color.red);
                    }
                    else if( x > textureSize - borderEmptySpace)
                    {
                        texture.SetPixel(x, y, Color.red);
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.green);
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    if (y < borderEmptySpace)
                    {
                        texture.SetPixel(x, y, Color.red);
                    }
                    else if (y > textureSize - borderEmptySpace)
                    {
                        texture.SetPixel(x, y, Color.red);
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.green);
                    }
                }
            }
        }

        texture.Apply();

        //Check for copy
        DeleteCopy(path);

        //create the asset
        AssetCreation(path, texture);

        //Debug.log the test       
        TestResult(path);
    }

    #region Texture Methode
    //Create Texture Methodes
    public static Texture2D CreateTexture(int size)
    {
        //Création de la Texture
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);

        //Set la texture full noir
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                texture.SetPixel(0, 0, Color.red);
            }
        }
        //Safe la modification
        texture.Apply();

        return texture;
    }
    private static string PathAsking(string name)
    {
        string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assets/", name, "png");
        path = FileUtil.GetProjectRelativePath(path);
        return path;
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
    #endregion
}
