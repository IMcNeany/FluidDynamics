using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    public float density = 0.0f;
    public float source = 0.0f;
    float prevDensity = 0.0f;
    public float verticalVelocity = 0.0f;
    public float previousVerticalVelocity;
    public float previousHorizontalVelocity = 0.0f;
    public float horizontalVelocity = 0.0f;
    public float densitySource = 0.0f;
    public int gridX;
    public int gridY;
    float diffusion;
    Material material;
    public List<PointData> surroundingPoints = new List<PointData>();
    public List<PointData> pointsToDiffuse = new List<PointData>();
    // Use this for initialization
    void Start() {
        //need to create and assign a render texture..
        material = gameObject.GetComponent<SpriteRenderer>().material;
        SetHorizontalVelocity(3.0f);
        SetVerticalVelocity(0.1f);
    }



    public void diffuse(float a, float c)
    {

        float surroundingDensity = 0;
        for (int i = 0; i < surroundingPoints.Count; i++)
        {
            surroundingPoints[i].SetPreviousDensity(surroundingPoints[i].density);
            surroundingDensity += surroundingPoints[i].density;
            if (surroundingPoints[i].density > 100.0f)
            {
                surroundingPoints[i].SetDensity(100.0f);
            }
        }
        SetDensity(GetPreviousDensity() + a * (surroundingDensity) / c);


    }
  
    public void SetMat(Material mat)
    {
        material = mat;
        material.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
      
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
        density = dens;
        SetDensitySource(density);
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

    public float GetDensitySource()
    {
        return density;
    }

    public void SetDensitySource(float source)
    {
        //take the 0. to get the touch on the square
        density = source;
   
        Color colour = new Color(1.0f, 0.0f, 0.0f, (source / 100.0f));
       // Debug.Log((source /100.0f) + "source / 100");

        material.color = colour;

        prevDensity = density;
        // Update is called once per frame
    }
    void Update () {
        

    }
}
