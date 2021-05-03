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

        public ZoneState state;
        [Range(0f, 2f)] public float windDir;
        public bool drawWind;

        [Space(10)]
        public Color color;
        [ReorderableList]
        public List<Transform> points;

        public ZoneEditable(string name)
        {
            this.name = name;
            color = Color.yellow;

            points = new List<Transform>();

            state = ZoneState.SeaCalm;
            windDir = 1f;
            drawWind = false;
        }
        public ZoneEditable(string name, Color color)
        {
            this.name = name;
            this.color = color;

            points = new List<Transform>();

            state = ZoneState.SeaCalm;
            windDir = 1f;
            drawWind = false;
        }
    }
}

