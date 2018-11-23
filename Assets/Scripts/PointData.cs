using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    public float density;
    public float source;
    float prevDensity;
    public float verticalVelocity;
    public float previousVerticalVelocity;
    public float previousHorizontalVelocity;
    public float horizontalVelocity;
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
         //   density[i] += Time.deltaTime * densitySources[i];
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
                   // density[(i + (n + 2) * j)] = prevDensity[((i) + (n + 2) * j)] + a * (density[((i - 1) + (n + 2) * j)] + density[((i+1)+ (n+2) * j)] + density[(i) + (n+2) * (j-1) ]+ density[(i) + (n+2) *(j+1)])/(1+4 *a);
                }
            }
          //  setBoundaries(n, b, x);
        }
    }
    
    public float GetDensity()
    {
        return density;
    }

    public float GetPreviousDensity()
    {
        return prevDensity;
    }

    public void SetPreviousDensity( float prevD)
    {
        prevDensity = prevD;
    }

    public void SetDensity( float dens)
    {
        prevDensity = dens;
    }

    public void SetSource(float s)
    {
        source = s;
    }

    public float GetSource()
    {
        return source;
    }

    public float GetHorizontalVelocity()
    {
        return horizontalVelocity;
    }

    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }

    public void SetVerticalVelocity( float vVel)
    {
        verticalVelocity = vVel;
    }

    public void SetHorizontalVelocity( float hVel)
    {
        horizontalVelocity = hVel;
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
