using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Relief { Coast, Flat, Hilly, Land };
public enum Weather { ClearSky, Wind };

public class Environnement : MonoBehaviour
{
    public Boundary limit = new Boundary(new Vector2(20,40));
    public List<Zone> zones;

/*#if UNITY_EDITOR*/
    
    private void OnValidate()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < zones.Count; i++)
	    {

		}
    }

/*#endif*/
}
