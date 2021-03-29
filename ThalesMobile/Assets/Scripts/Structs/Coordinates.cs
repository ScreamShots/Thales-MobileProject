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
    /// <param name="_position">OceanEntity position</param>
    /// <param name="_direction">OceanEntity direction</param>
    /// <param name="rotation">OceanEntity rotation</param>
    public Coordinates(Vector3 _position, Vector2 _direction, float rotation)
    {
        position = ConvertWorldToVector2(_position);
        direction =_direction;
        this.rotation = rotation;
    }

    public static Vector2 ConvertWorldToVector2(Vector3 vector)
    {
        return new Vector2(vector.x,vector.z);
    }

    public static Vector3 ConvertVector2ToWorld(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

}
