using UnityEngine;
using System;

[Serializable]
public struct CameraBoundary
{
    public Vector2 size;
    public Vector2 offSet;

    /// <summary>
    /// The value of the right bound X
    /// (including the offset)
    /// </summary>
    public float right
    {
        get
        {
            return offSet.x + (size.x * 0.5f);
        }
    }
    /// <summary>
    /// The value of the left bound X
    /// (including the offset)
    /// </summary>
    public float left
    {
        get
        {
            return offSet.x - (size.x * 0.5f);
        }
    }
    /// <summary>
    /// Return the value of the top bound y
    /// (including the offset)
    /// </summary>
    public float up
    {
        get
        {
            return offSet.y + (size.y * 0.5f);
        }
    }
    /// <summary>
    /// Return the value of the bottom bound y
    /// (including the offset)
    /// </summary>
    public float down
    {
        get
        {
            return offSet.y - (size.y * 0.5f);
        }
    }

    public CameraBoundary(Vector2 size)
    {
        this.size = new Vector2(size.x, size.y);
        this.offSet = Vector2.zero;
    }
    public CameraBoundary(float sizeX, float sizeY)
    {
        this.size = new Vector2(sizeX, sizeY);
        this.offSet = Vector2.zero;
    }
    public CameraBoundary(Vector2 size, Vector2 offSet)
    {
        this.size = new Vector2(size.x, size.y);
        this.offSet = new Vector2(offSet.x, offSet.y);
    }
    public CameraBoundary(Vector2 size, float offSetX, float offSetY)
    {
        this.size = new Vector2(size.x, size.y);
        this.offSet = new Vector2(offSetX, offSetY);
    }
}
