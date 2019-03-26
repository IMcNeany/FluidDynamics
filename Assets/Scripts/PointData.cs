using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData : MonoBehaviour {

    public float density = 1.0f;
    public float source = 0.0f;
    float prevDensity = 0.0f;
    public float verticalVelocity = 1.0f;
    public float previousVerticalVelocity = 0.0f;
    public float previousHorizontalVelocity = 0.0f;
    public float horizontalVelocity = 1.0f;
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
        SetHorizontalVelocity(1);
        SetVerticalVelocity(1);
    }

    private void OnTriggerStay(Collider other)
    {
      
        Rigidbody box = other.attachedRigidbody;

        //  box.AddForceAtPosition()
        box.AddForce(new Vector3(horizontalVelocity, 0, verticalVelocity), ForceMode.VelocityChange);
      // box.AddRelativeForce(new Vector3())

    }

    public void diffuse(float a, float c)
    {

         /* float surroundingDensity = 0;
          for (int i = 0; i < surroundingPoints.Count; i++)
          {
              //surroundingPoints[i].SetPreviousDensity(surroundingPoints[i].density);
              surroundingDensity += surroundingPoints[i].density;
              if (surroundingPoints[i].density > 300.0f)
              {
                  surroundingPoints[i].SetDensity(300.0f);
                Debug.Log("max density set for point" + i);
              }
          }
    
          SetDensity(GetPreviousDensity() + a * (surroundingDensity) / c);*/

          //diff density, differet colours*/

     /*   if (surroundingPoints.Count == 2)
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
        }*/

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
        SetDensitySource(dens);
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
       // previousVerticalVelocity = verticalVelocity;
     
        verticalVelocity = vVel;

      
        SetVelocityTextureDir();
    }

    public void SetHorizontalVelocity( float hVel)
    {
        //   Debug.Log(hVel + "hvel set");
        //previousHorizontalVelocity = horizontalVelocity;
    
        horizontalVelocity = hVel;
      
        SetVelocityTextureDir();

    }

   void SetVelocityTextureDir()
    {
       Transform child = gameObject.transform.GetChild(0);

        float dir = 0;
        bool setRotation = true;
        if(verticalVelocity == 0 && horizontalVelocity == 0)
        {
            child.gameObject.SetActive(false);
            setRotation = false;
        }
        else if(verticalVelocity <0)
        {
             setRotation = true;
            child.gameObject.SetActive(true);
            if (horizontalVelocity ==0)
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
            setRotation = true;
            child.gameObject.SetActive(true);
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
            setRotation = true;
            child.gameObject.SetActive(true);
            dir = 90;
            float percent = (verticalVelocity / horizontalVelocity);
            float additionalDir = 45 * percent;
            dir += additionalDir;
        }
       
        if(setRotation)
        {
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, dir);
            child.transform.localRotation = rotation;
        }
    }

    public float GetDensitySource()
    {
        return density;
    }

    public void SetDensitySource(float source)
    {
        //take the 0. to get the touch on the square
        density = source;

        if (source < 0)
        {
            density = 0;
        }

        Color colour = new Color(1.0f, 0.0f, 0.0f, (source/50));
       // Debug.Log((source /100.0f) + "source / 100");

        material.color = colour;

       // prevDensity = density;
        // Update is called once per frame
    }
    void Update () {
        

    }
}
