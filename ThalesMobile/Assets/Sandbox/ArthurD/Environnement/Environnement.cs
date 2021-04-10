using System.Collections.Generic;
using UnityEngine;

public enum Relief { Coast, Flat, Hilly, Land };
public enum Weather { ClearSky, Wind };

public class Environnement : MonoBehaviour
{
    public Boundary limit = new Boundary(new Vector2(20,40));
    public List<Zone> zones;

    private void OnValidate()
    {
        
    }

    private void OnDrawGizmos()
    {

    }
}
