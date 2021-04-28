#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class SeaTextureGenerator
{
    public static void GenerateZoneDataTexture(Environnement map, string name)
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


        //Set les couleurs de chaques zone
        List<Color> zoneColor = new List<Color>() { Color.black, Color.red, Color.green, Color.blue };
        if (map.zones.Length != 0)
        {
            zoneColor.RemoveRange(1, 3);
            for (int i = 0; i < map.zones.Length; i++)
            {
                float waveStrenght = 0;
                float colorIntensity = 0;
                switch (map.zones[i].state)
                {
                    case ZoneState.SeaCalm:
                        waveStrenght = 0.3f;
                        colorIntensity = 0.5f;
                        break;
                    case ZoneState.SeaWay:
                        waveStrenght = 0.5f;
                        colorIntensity = 0.3f;
                        break;
                    case ZoneState.SeaTurbulent:
                        waveStrenght = 1f;
                        colorIntensity = 1f;
                        break;
                    case ZoneState.WindyZone:
                        waveStrenght = 0.7f;
                        colorIntensity = 0.7f;
                        break;
                    default:
                        break;
                }

                zoneColor.Add(
                    new Color(
                        map.zones[i].windDir * 0.5f, //R = direction du vent
                        waveStrenght, //G = Wave Height
                        colorIntensity, //B = Color Intensity
                        1));
            }
        }

        Color tempColor;
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

        //texture = TextureGenerator.Blur(texture, 1);

        //Check for copy
        TextureGenerator.DeleteCopy(path);

        //create the asset
        TextureGenerator.TextureToAssetPNG(path, texture);

        //Debug.log the test       
        TextureGenerator.TestTextureAtPath(path);
    }

    public static void GenerateSeaTexture(Environnement map, string name)
    {
        //generate the path
        string path = TextureGenerator.PathAsking(name);
        if (path == null || path == string.Empty)
        {
            Debug.Log("SeaTexture creation have been cancel");
            return;
        }

        //Set les couleurs de chaques zone
        List<Color> zoneColor = new List<Color>() { Color.red, Color.green, Color.blue };
        if(map.zones.Length != 0)
        {
            zoneColor.RemoveRange(0,2);
            for (int i = 0; i < map.zones.Length; i++)
            {
                zoneColor.Add(map.zones[i].debugColor);
            }
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
#endif
