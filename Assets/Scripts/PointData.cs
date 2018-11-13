using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    float[] density;
    float[] prevDensity;
    Vector2[] velocity;
    Vector2[] prevVelocity;
    public List<Vector2> densitySources = null;


    // Use this for initialization
    void Start () {
        

	}
    
    public float GetDensity(int i)
    {
        return density[i];
    }

    public float GetPreviousDensity(int i)
    {
        return prevDensity[i];
    }

    public void SetPreviousDensity(int i, float prevD)
    {
        prevDensity[i] = prevD;
    }

    public void SetDensity(int i, float dens)
    {
        prevDensity[i] = dens;
    }

    public Vector2 GetVelocity(int i)
    {
        return velocity[i];
    }

    public Vector2 GetPreviousVelocity(int i)
    {
        return prevVelocity[i];
    }

    public void SetPreviousVelocity(int i, Vector2 prevV)
    {
        prevVelocity[i] = prevV;
    }

    public void SetVelocity(int i, Vector2 vel)
    {
        prevVelocity[i] = vel;
    }

    public Vector2 GetDensitySource(int i)
    {
        return densitySources[i];
    }

    public void SetDensitySource(Vector2 source)
    {
        //take the 0. to get the touch on the square
        densitySources.Add(source);
        //densitySources.Add(source);
        Debug.Log("source" + source);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
