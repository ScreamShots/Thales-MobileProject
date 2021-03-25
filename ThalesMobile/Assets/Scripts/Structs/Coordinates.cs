using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Thomas Depraz - 25/03/2021 - Struct used to store different informations about OceanEntitys positions.
/// </summary>
public struct Coordinates
{
    public Vector2 position { get; set; }
    public Vector2 direction { get; set; }
    public float rotation { get; set; }

    /// <summary>
    /// Default constructor for the Coordinates struct. 
    /// </summary>
    /// <param name="position">OceanEntity position</param>
    /// <param name="direction">OceanEntity direction</param>
    /// <param name="rotation">OceanEntity rotation</param>
    public Coordinates(Vector2 position, Vector2 direction, float rotation)
    {
        this.position = position;
        this.direction = direction;
        this.rotation = rotation;
    }
}
