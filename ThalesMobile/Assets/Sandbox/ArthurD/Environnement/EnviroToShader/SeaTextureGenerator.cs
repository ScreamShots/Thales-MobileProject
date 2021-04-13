using UnityEngine;
using UnityEditor;

public static class SeaTextureGenerator
{
    static Color[] zoneColor = new Color[9]
    {
        Color.black,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        Color.grey,
        Color.white
    };

    public static void GenerateSeaTexture(Environnement map, string name)
    {
        //generate the path
        string path = TextureGenerator.PathAsking(name);
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
        float inverseDetail = (float)1f / detailLevel;

        //Determine quel coté est le plus long
        bool maxIsX = map.limit.size.x > map.limit.size.y;
        float mapMaxLenght = maxIsX ? map.limit.size.x : map.limit.size.y;

        int textureSize = Mathf.CeilToInt(mapMaxLenght * detailLevel);
        float textNormalizer = (float)1f / textureSize;
        float borderEmptySpace = (maxIsX ? map.limit.size.x - map.limit.size.y : map.limit.size.y - map.limit.size.x) * 0.5f;
        #endregion
        //Creating The texture
        Texture2D texture = TextureGenerator.CreateTexture(textureSize);

        float textStartPosX = map.limit.offSet.x + map.limit.leftBorder;
        float textStartPosY = map.limit.offSet.y + map.limit.downBorder;
        if (maxIsX)
        {
            textStartPosY -= borderEmptySpace;
        }
        else
        {
            textStartPosX -= borderEmptySpace;
        }

        Color tempColor = Color.grey;
        //Send Data in
        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                tempColor = zoneColor[map.ZoneIn(new Vector2(textStartPosX + (x * inverseDetail), textStartPosY + (y * inverseDetail)))];
                texture.SetPixel(x, y, tempColor);
            }
        }

        texture.Apply();

        //Check for copy
        TextureGenerator.DeleteCopy(path);

        //create the asset
        TextureGenerator.TextureToAssetPNG(path, texture);

        //Debug.log the test       
        TextureGenerator.TestTextureAtPath(path);
    }
}
