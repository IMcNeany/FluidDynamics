using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    public float density = 1.0f;
    public float source = 0.0f;
    float prevDensity = 0.0f;
    public float verticalVelocity = -5.0f;
    public float previousVerticalVelocity;
    public float previousHorizontalVelocity = 0.0f;
    public float horizontalVelocity = 2.0f;
    public float densitySource = 0.0f;
    public int gridX;
    public int gridY;
    public float diffusion;
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

        /*  float surroundingDensity = 0;
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

          //diff density, differet colours*/

        if (surroundingPoints.Count == 2)
        {
            SetDensity((GetPreviousDensity() + a * surroundingPoints[0].density + surroundingPoints[1].density) / c);
        }

        if (surroundingPoints.Count == 3)
        {
            SetDensity((GetPreviousDensity() + a * surroundingPoints[0].density + surroundingPoints[1].density + surroundingPoints[2].density) / c);
        }

        if (surroundingPoints.Count == 4)
        {
            SetDensity((GetPreviousDensity() + a * surroundingPoints[0].density + surroundingPoints[1].density + surroundingPoints[2].density + surroundingPoints[3].density) / c);
        }

    }

   public void DiffuseHorizontalVelocity(float a, float c)
    {

        if (surroundingPoints.Count == 2)
        {
            SetHorizontalVelocity((previousHorizontalVelocity + a * surroundingPoints[0].GetHorizontalVelocity() + surroundingPoints[1].GetHorizontalVelocity()) / c);
        }

        if (surroundingPoints.Count == 3)
        {
            SetHorizontalVelocity((previousHorizontalVelocity + a * surroundingPoints[0].GetHorizontalVelocity() + surroundingPoints[1].GetHorizontalVelocity() + surroundingPoints[2].GetHorizontalVelocity()) / c);
        }

        if (surroundingPoints.Count == 4)
        {
            SetHorizontalVelocity((previousHorizontalVelocity + a * surroundingPoints[0].GetHorizontalVelocity() + surroundingPoints[1].GetHorizontalVelocity() + surroundingPoints[2].GetHorizontalVelocity() + surroundingPoints[3].GetHorizontalVelocity()) / c);
        }
    }

    public void DiffuseVerticalVelocity(float a, float c)
    {
        if (surroundingPoints.Count == 2)
        {
            SetVerticalVelocity((previousVerticalVelocity + a * surroundingPoints[0].GetVerticalVelocity() + surroundingPoints[1].GetVerticalVelocity()) / c);
        }

        if (surroundingPoints.Count == 3)
        {
            SetVerticalVelocity((previousVerticalVelocity + a * surroundingPoints[0].GetVerticalVelocity() + surroundingPoints[1].GetVerticalVelocity() + surroundingPoints[2].GetVerticalVelocity()) / c);
        }

        if (surroundingPoints.Count == 4)
        {
            SetVerticalVelocity((previousVerticalVelocity + a * surroundingPoints[0].GetVerticalVelocity() + surroundingPoints[1].GetVerticalVelocity() + surroundingPoints[2].GetVerticalVelocity() + surroundingPoints[3].GetVerticalVelocity()) / c);
        }
    }


    public void Advect(int i, float size)
    {
        float dt0 = Time.deltaTime * size;
        for (int j =0; j < size; j++)
        {
            float x = i - dt0 * GetHorizontalVelocity();
            float y = j - dt0 * GetVerticalVelocity();


            if (x < 0.5f)
            {
                x = 0.5f;
            }
            if (x > size + 0.5f)
            {
                x = size + 0.5f;
            }
            int i0 = (int)x;
            int i1 = i0 + 1;
            if (y < 0.5f)
            {
                y = 0.5f;
            }
            if (y > size + 0.5f)
            {
                y = size + 0.5f;
            }
            int j0 = (int)y;
            int j1 = j0 + 1;
            float s1 = x - i0;
            float s0 = 1 - s1;
            float t1 = y - j0;
            float t0 = 1 - t1;

          
               //    SetDensity(s0 * (t0 * GetPreviousDensity() + t1 * surroundingPoints[0].GetPreviousDensity()) 
                 //      + s1 * (t0 * surroundingPoints[1].GetPreviousDensity() + t1 * surroundingPoints[2].GetPreviousDensity()));
          
        }


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
        previousVerticalVelocity = verticalVelocity;
        verticalVelocity = vVel;
        SetVelocityTextureDir();
    }

    public void SetHorizontalVelocity( float hVel)
    {
        //   Debug.Log(hVel + "hvel set");
        previousHorizontalVelocity = horizontalVelocity;
        horizontalVelocity = hVel;
        SetVelocityTextureDir();

    }

   void SetVelocityTextureDir()
    {
    /*    Transform child = gameObject.transform.GetChild(0);

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
        child.transform.localRotation = rotation;*/
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
