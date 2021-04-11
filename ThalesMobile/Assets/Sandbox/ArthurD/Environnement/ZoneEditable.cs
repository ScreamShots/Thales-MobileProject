using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Thales.Tool.LevelDesign
{
    [System.Serializable]
    public struct ZoneEditable
    {
        [HideInInspector]
        public string name;
        public Relief relief;
        public Weather currentWeather;
        [Space(10)]
        public Color color;
        [ReorderableList]
        public List<Transform> points;

        public ZoneEditable(string name)
        {
            this.name = name;
            color = Color.yellow;

            points = new List<Transform>();

            relief = Relief.Flat;
            currentWeather = Weather.ClearSky;
        }
        public ZoneEditable(string name, Color color)
        {
            this.name = name;
            this.color = color;

            points = new List<Transform>();

            relief = Relief.Flat;
            currentWeather = Weather.ClearSky;
        }
    }
}

