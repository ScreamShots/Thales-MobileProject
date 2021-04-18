#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class TextureGenerator
{
    public static Texture2D CreateTexture(int size)
    {
        //Création de la Texture
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);

        //Set la texture full noir
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                texture.SetPixel(0, 0, Color.white);
            }
        }
        //Safe la modification
        texture.Apply();

        return texture;
    }
    public static Texture2D CreateTexture(int sizeX, int sizeY)
    {
        //Création de la Texture
        Texture2D texture = new Texture2D(sizeX, sizeY, TextureFormat.ARGB32, false);

        //Set la texture full noir
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                texture.SetPixel(0, 0, Color.white);
            }
        }
        //Safe la modification
        texture.Apply();

        return texture;
    }
    public static Texture2D CreateTexture(Vector2Int size)
    {
        //Création de la Texture
        Texture2D texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        //Set la texture full noir
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                texture.SetPixel(0, 0, Color.white);
            }
        }
        //Safe la modification
        texture.Apply();

        return texture;
    }
    
    public static Texture2D Blur(Texture2D image, int blurSize)
    {
        Texture2D blurred = new Texture2D(image.width, image.height);

        // look at every pixel in the blur rectangle
        for (int xx = 0; xx < image.width; xx++)
        {
            for (int yy = 0; yy < image.height; yy++)
            {
                float avgR = 0, avgG = 0, avgB = 0, avgA = 0;
                int blurPixelCount = 0;

                // average the color of the red, green and blue for each pixel in the
                // blur size while making sure you don't go outside the image bounds
                for (int x = xx; (x < xx + blurSize && x < image.width); x++)
                {
                    for (int y = yy; (y < yy + blurSize && y < image.height); y++)
                    {
                        Color pixel = image.GetPixel(x, y);

                        avgR += pixel.r;
                        avgG += pixel.g;
                        avgB += pixel.b;
                        avgA += pixel.a;

                        blurPixelCount++;
                    }
                }

                avgR = avgR / blurPixelCount;
                avgG = avgG / blurPixelCount;
                avgB = avgB / blurPixelCount;
                avgA = avgA / blurPixelCount;

                // now that we know the average for the blur size, set each pixel to that color
                for (int x = xx; x < xx + blurSize && x < image.width; x++)
                {
                    for (int y = yy; y < yy + blurSize && y < image.height; y++)
                    {
                        blurred.SetPixel(x, y, new Color(avgR, avgG, avgB, avgA));

                    }
                }
            }
        }
        blurred.Apply();
        return blurred;
    }

    public static string PathAsking(string name)
    {
        string path = EditorUtility.SaveFilePanel("Save Texture Asset", "Assets/", name, "png");
        path = FileUtil.GetProjectRelativePath(path);
        return path;
    }
    public static void TextureToAssetPNG(string filePath, Texture2D texture)
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
    public static void DeleteCopy(string filePath)
    {
        if ((Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D)) != null)
        {
            Debug.Log("A file have already this name, this file was replace");

            FileUtil.DeleteFileOrDirectory(filePath);

            //Update Folder
            AssetDatabase.Refresh();
        }

    }
    public static bool TestTextureAtPath(string filePath)
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
#endif
