using UnityEngine;
using System;

[Serializable]
public struct Boundary
{
    //Variable
    public Vector2 size;
    public Vector2 offSet;

    //Property
    public float rightBorder
    {
        get
        {
            return offSet.x + (size.x * 0.5f);
        }
    }
    public float leftBorder
    {
        get
        {
            return offSet.x - (size.x * 0.5f);
        }
    }
    public float upBorder
    {
        get
        {
            return offSet.y + (size.y * 0.5f);
        }
    }
    public float downBorder
    {
        get
        {
            return offSet.y - (size.y * 0.5f);
        }
    }

    //Constructor
    public Boundary(Vector2 size)
    {
        this.size = new Vector2(size.x, size.y);
        this.offSet = Vector2.zero;
    }
    public Boundary(float sizeX, float sizeY)
    {
        this.size = new Vector2(sizeX, sizeY);
        this.offSet = Vector2.zero;
    }
    public Boundary(Vector2 size, Vector2 offSet)
    {
        this.size = new Vector2(size.x, size.y);
        this.offSet = new Vector2(offSet.x, offSet.y);
    }
    public Boundary(Vector2 size, float offSetX, float offSetY)
    {
        this.size = new Vector2(size.x, size.y);
        this.offSet = new Vector2(offSetX, offSetY);
    }

    //Methode
    public bool InBoundary(Vector2 pos)
    {
        //Est ce que je dépasse en x ?
        if (pos.x < leftBorder || rightBorder < pos.x)
        {
            return false;
        }
        else
        //Est ce que je dépasse en y ?
        if (pos.y < downBorder || upBorder < pos.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool InBoundary(Vector3 pos)
    {
        //Est ce que je dépasse en x ?
        if (pos.x < leftBorder || rightBorder < pos.x)
        {
            return false;
        }
        else
        //Est ce que je dépasse en y ?
        if (pos.z < downBorder || upBorder < pos.z)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
