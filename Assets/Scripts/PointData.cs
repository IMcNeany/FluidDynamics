using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    float[] density;
    float[] prevDensity;
    Vector2[] velocity;
    Vector2[] prevVelocity;
    public List<float> densitySources = null;
    float diffusion;



    // Use this for initialization
    void Start () {
        

	}

    void AddSource(int n)
    {
        int size = (n+2) * (n+2);

        for(int i = 0; i < size; i++)
        {
            density[i] += Time.deltaTime * densitySources[i];
        }

    }

    void diffuse(int n, int b)
    {
        float a = Time.deltaTime * diffusion *  n * n;
        for (int k = 0; k <20; k++)
        {
            for(int i = 0; i <= n; i++)
            {
                for(int j = 1; j<=n; j++)
                {
                    density[(i + (n + 2) * j)] = prevDensity[((i) + (n + 2) * j)] + a * (density[((i - 1) + (n + 2) * j)] + density[((i+1)+ (n+2) * j)] + density[(i) + (n+2) * (j-1) ]+ density[(i) + (n+2) *(j+1)])/(1+4 *a);
                }
            }
            setBoundaries(n, b, x);
        }
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

    public float GetDensitySource(int i)
    {
        return densitySources[i];
    }

    public void SetDensitySource(float source)
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
