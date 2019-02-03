using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    public float density = 0.0f;
    public float source = 0.0f;
    float prevDensity = 0.0f;
    public float verticalVelocity = -5.0f;
    public float previousVerticalVelocity;
    public float previousHorizontalVelocity = 0.0f;
    public float horizontalVelocity = 2.0f;
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
        SetHorizontalVelocity(horizontalVelocity);
        SetVerticalVelocity(verticalVelocity);
    }

    private void OnTriggerStay(Collider other)
    {
      
        Rigidbody box = other.attachedRigidbody;

        //  box.AddForceAtPosition()
        box.AddForce(new Vector3(horizontalVelocity * density/10, 0, verticalVelocity * density /100), ForceMode.Impulse);

    }

    public void diffuse(float a, float c)
    {

        float surroundingDensity = 0;
        for (int i = 0; i < surroundingPoints.Count; i++)
        {
            surroundingPoints[i].SetPreviousDensity(surroundingPoints[i].density);
            surroundingDensity += surroundingPoints[i].density;
            if (surroundingPoints[i].density > 300.0f)
            {
                surroundingPoints[i].SetDensity(300.0f);
            }
        }
        //SetPreviousDensity( density);
        SetDensity(GetPreviousDensity() + a * (surroundingDensity) / c);

        //diff density, differet colours


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
        SetVelocityTextureDir();
    }

    public void SetHorizontalVelocity( float hVel)
    {
     //   Debug.Log(hVel + "hvel set");
        horizontalVelocity = hVel;
        SetVelocityTextureDir();


    }

   void SetVelocityTextureDir()
    {
        Transform child = gameObject.transform.GetChild(0);

        float dir = 0;
    
        if(verticalVelocity <0)
        {
           
            if(horizontalVelocity ==0)
            {
                dir = 0;
            }
            else
            {
                float percent = (verticalVelocity / horizontalVelocity );
                float additionalDir = 45 * percent;
                if (horizontalVelocity < 0)
                {
                    dir = -90;
                    Debug.Log(dir + "hoz < 0");
                }
                else
                {
                    dir = 90;
                }
                dir += additionalDir;
                Debug.Log("final dir" + dir);
            }
        }
        else if(horizontalVelocity < 0)
        {
            if (verticalVelocity == 0)
            {
                dir = -90;
            }
            else
            {
                float percent = ( verticalVelocity/ horizontalVelocity);
                float additionalDir = 45 * percent;
                if (verticalVelocity < 0)
                {
                    dir = 90;
                    Debug.Log(dir + "hoz < 0");
                }
                else
                {
                    dir = -90;
                }
                dir += additionalDir;
                Debug.Log("final dir" + dir);
            }
        }
        else
        {
            dir = 90;
            float percent = (verticalVelocity / horizontalVelocity);
            float additionalDir = 45 * percent;
            dir += additionalDir;
        }
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, dir);
        child.transform.localRotation = rotation;
    }

    public float GetDensitySource()
    {
        return density;
    }

    public void SetDensitySource(float source)
    {
        //take the 0. to get the touch on the square
        density = source;
   
        Color colour = new Color(1.0f, 0.0f, 0.0f, (source / 50.0f));
       // Debug.Log((source /100.0f) + "source / 100");

        material.color = colour;

       // prevDensity = density;
        // Update is called once per frame
    }
    void Update () {
        

    }
}
