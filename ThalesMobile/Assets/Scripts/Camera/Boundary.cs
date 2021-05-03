using UnityEngine;
using System;

#pragma warning disable 0661
#pragma warning disable 0660

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

    //Operator
    public static Boundary operator +(Boundary a, Boundary b) 
    {
        Boundary result = new Boundary(Vector2.zero, Vector2.zero);
        result.size = new Vector2(a.size.x + b.size.x, a.size.y + b.size.x);
        result.offSet = new Vector2(a.offSet.x + b.size.x, a.offSet.y + b.size.x);
        return result;
    }
    public static Boundary operator -(Boundary a, Boundary b)
    {
        Boundary result = new Boundary(Vector2.zero, Vector2.zero);
        result.size = new Vector2(a.size.x - b.size.x, a.size.y - b.size.x);
        result.offSet = new Vector2(a.offSet.x - b.size.x, a.offSet.y - b.size.x);
        return result;
    }
    public static bool operator ==(Boundary lhs, Boundary rhs)
    {
        if (lhs.size.x == rhs.size.x && lhs.size.y == rhs.size.y 
            && lhs.offSet.x == rhs.offSet.x && lhs.offSet.y == rhs.offSet.y)
        {
            return true;
        }

        return false;
    }
    public static bool operator !=(Boundary lhs, Boundary rhs)
    {
        if (lhs.size.x != rhs.size.x || lhs.size.y != rhs.size.y
            || lhs.offSet.x != rhs.offSet.x || lhs.offSet.y != rhs.offSet.y)
        {
            return true;
        }

        return false;
    }
}

#pragma warning restore 0660
#pragma warning restore 0661
