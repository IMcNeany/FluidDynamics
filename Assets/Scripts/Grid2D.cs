using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid2D : MonoBehaviour {
    public int size = 18;
    public GameObject pointData;
    public List<PointData> points = new List<PointData>();
    public Slider slider;
    public InputField vertVel;
    public InputField horVel;
    public GameObject cube;
    float verticalVelocity = 0.0f;
    float horizontalVelocity = 0.0f;
    float diffusionRate = 0.2f;
    public List<PointData> DiffuseNodes = new List<PointData>();
    public Material mat;
    int nodeCount = 0;
    bool hitOne = true;
    int newsize;
    float newVertVel;
    float newHorzVel;
    // Use this for initialization
    void Start () {
        DrawGrid();
        SetNeighbours();
        slider.value = size;
      
       
    }

    void DensityStep()
    {
        AddDiffusionSource();
        SwapDensity();
        DiffuseDensity();
        SwapDensity();
        AdvectDensity();
    }

   /* void dens_step(int N, float[] x, float[] x0, float[] u, float[] v, float diff, float dt)
    {
        add_source(N, x, x0, dt);
        SWAP(x0, x);
        diffuse(N, 0, x, x0, diff, dt);
        SWAP(x0, x);
        advect(N, 0, x, x0, u, v, dt);
    }

  */

    void AddDiffusionSource()
    {
       // int densSize = (size) + (size);
        //poss denssize but idkk
        for(int i = 0; i < points.Count; i++)
        {
            points[i].SetDensity(points[i].density += Time.deltaTime * points[i].GetPreviousDensity());
        }
    }

    void SwapDensity()
    {
        for (int i = 0; i < points.Count; i++)
        {
            float tmp = points[i].GetPreviousDensity();
            points[i].SetPreviousDensity(points[i].density);
            points[i].SetDensity(tmp);
        }
    }

   void DiffuseDensity()
    {
        float a = Time.deltaTime * diffusionRate * size * size;
        LinearSolver( a, 1 + 4 * a);
    }

    void LinearSolver(float a, float c)
    {
        for (int k = 0; k < 20; k++)
        {
            //for size of points linear solve
            for(int i = 0; i < size; i++)
            {
                points[i].diffuse(a, c);
            }
         /*   for (int i = 1; i <= size; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    points[(i) + (size + 2) * (j)].SetDensity((points[(i) + (size + 2) * (j)].GetPreviousDensity() + a * (points[(i - 1) + (size + 2) * (j)].density
                        + points[(i + 1) + (size + 2) * (j)].density + points[(i) + (size + 2) * (j - 1)].density + points[(i) + (size + 2) * (j + 1)].density) / c));
                    Debug.Log(i +j *((i + 1) + (size + 2) * (j + 1)) + " point ");
                }
                
            }*/
        }
    }

    void AdvectDensity()
    {
        float dt0 = Time.deltaTime * points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            for (int k = 0; k <= points.Count; k++)
            {


                float x = i - dt0 * points[i].GetHorizontalVelocity();
                float y = k - dt0 * points[i].GetVerticalVelocity();

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

                /*    if (j0 == size-1)
                    {
                        j0 = 0;
                        j1 = 1;
                    }
                    if (i0 == size -1)
                    {
                        i0 = 0;
                        i1 = 1;
                    }
                    if (i1 == size-1)
                    {
                        i1 = 0;
                    }
                    if (i1 == size-1)
                    {
                        j1 = 0;
                    }*/

                points[i].SetDensity(s0 * (t0 * points[(i0) + (size + 2) * (j0)].GetPreviousDensity() + t1 * points[(i0) + (size + 2) * (j1)].GetPreviousDensity())
                          + s1 * (t0 * points[(i1) + (size + 2) * (j0)].GetPreviousDensity() + t1 * points[(i1) + (size + 2) * (j1)].GetPreviousDensity()));
            }
        }
    }
    /*void vel_step(int N, float[] u, float[] v, float[] u0, float[] v0, float visc, float dt)
    {
        add_source(N, u, u0, dt);
        add_source(N, v, v0, dt);
        SWAP(u0, u);
        diffuse(N, 1, u, u0, visc, dt);
        SWAP(v0, v);
        diffuse(N, 2, v, v0, visc, dt);
        project(N, u, v, u0, v0);
        SWAP(u0, u);
        SWAP(v0, v);
        advect(N, 1, u, u0, u0, v0, dt);
        advect(N, 2, v, v0, u0, v0, dt);
        project(N, u, v, u0, v0);
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
           
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                //agent.destination = hit.point;
              
                    if (hit.collider.gameObject.GetComponent<PointData>())
                    {
                        PointData point = hit.collider.gameObject.GetComponent<PointData>();
                        point.SetDensitySource(300.0f);

                        //  Debug.Log("mouse pos" + hit.point.x + hit.point.z);
                       
                    }
               


              //  Diffuse( b, diffusionRate);
            }
        }

        DensityStep();
    }

    void DrawGrid()
    {
        float spacing = 12.0f / size;
        Debug.Log(spacing + "spacing " + size + "size" 
            + 12/18);
        float xValue = 2.0f;
        float yValue = 0.5f;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject newPoint = Instantiate(pointData,new Vector3(xValue, 0,yValue), new Quaternion(1.0f,0,0,1));
                newPoint.transform.localScale = new Vector3(spacing, spacing, spacing);
                points.Add(newPoint.GetComponent<PointData>());
                newPoint.GetComponent<PointData>().gridX = i;
                newPoint.GetComponent<PointData>().gridY = j;
                newPoint.GetComponent<PointData>().SetMat(mat);
             //   Debug.Log(horizontalVelocity + "h");
                newPoint.GetComponent<PointData>().SetHorizontalVelocity(horizontalVelocity);
                newPoint.GetComponent<PointData>().SetVerticalVelocity(verticalVelocity);
                newPoint.transform.SetParent(gameObject.transform);

                xValue += spacing;
            }
            xValue = 2.0f;
            yValue += spacing;
        }
    }

    void SetNeighbours()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                PointData point = points[i + j * size];
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if ((Mathf.Abs(x) == Mathf.Abs(y)))
                        {
                            continue;
                        }
                        int checkX = point.gridX + x;
                        int checkY = point.gridY + y;

                        if (checkY >= 0 && checkY < size && checkX >= 0 && checkX < size)
                        {
                            point.surroundingPoints.Add(points[checkX*size + checkY]);
                        }
                    }
                }
            }
        }
    }

   /* void Density(int b, float diffusionRate)
    {
       // AddSource(source);
        //Swap()
        Diffuse(b, diffusionRate);
        //swap
        //Advect(b);
    }

    void AddSource(float source)
    {
        int totalSize = size * size;
        for (int i = 0; i < totalSize; i++)
        {
            points[i].density += Time.deltaTime * points[i].GetSource();
        }
    }

    void Diffuse( int b, float diffusion)
    {
        float a = Time.deltaTime * diffusion * size;
      //  Debug.Log(a + "a");
       // LinearSolver( b, a, 1 + 4 * a);
    }

  /*  void LinearSolver(int b, float a, float c)
    {

        for (int i = 0; i < size-1; i++)
        {
            for (int j = 0; j < size; j++)
            {
                DiffuseDensity(points[i * size + j], a, c);

            }
        }

        //SetBoundaryDensity(b); //x vertical vel
        // SetBoundaryVerticalVelocity(b);
        Advect(b);
    }

     void DiffuseDensity(PointData point, float a , float c)
    {
        point.diffuse(a ,c);
    
    }


    void Advect( int b)
    {

        float dt = Time.deltaTime * size;
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                //   Debug.Log(i + " i1 " + j + " j1");
                //Debug.Log(i * size)
                //float x = i - dt * points[(i * size + j)].GetVerticalVelocity();

                float x = i - points[i * size + j].GetVerticalVelocity() * dt;

                // float y = j - dt * points[(i * size + j)].GetHorizontalVelocity();

                float y = j - points[i * size + j].GetHorizontalVelocity() * dt;

                if (x < 0.5f)
                {
                    x = 0.5f;

                }

                if (x > size + 0.5f)
                {
                    x = size;

                }

                int i0 = (int)x;
                int i1 = i0 + 1;

                if (y < 0.5f)
                {
                    y = 0.5f;

                }
                if (y > size + 0.5f)
                {
                    y = size;

                }

                int j0 = (int)y;
                int j1 = j0 + 1;
                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;
     

              
                if(j1 > size)
                {
                    j1 = 0;
                }
                if(i1 > size)
                {
                    i1 = 0;
                }
                   points[(i * size + (j))].SetDensity( s0 * ((t0 * points[(i0 * size + (j0))].GetPreviousDensity()) + 
                       (t1 * points[(i0 * size + (j1))].GetPreviousDensity()) + s1 * ((t0 * points[(i1 * size + (j0))].GetPreviousDensity()) + (t1 * points[(i1 * size + (j1))].GetPreviousDensity()))));
            

                    
                // t0 i0 j1 top left
                //t0 i1 j1 bottom left
                //t0 i1 j1 bottom left
                //t1 i0j0 disappears
                //t1 i1 j0 top left
                //t1 i1 j1 dissapeaers
                //t1 i0 j1 disapppeears

           
            }
           
        }
       
   
    }


    void Swap(float i, float j)
    {
        float tmp = i;
        i = j;
        j = tmp;
    }

    void project()
    {

    for (int i = 1; i <= size; i++)
    {
        for (int j = 1; j <= size; j++)
        {
            points[(i + (size + 2) * (j))].previousVerticalVelocity = -0.5f * (points[((i+1) + (size + 2) * (j))].horizontalVelocity
                - points[((i-1) + (size + 2) * (j))].horizontalVelocity + points[(i + (size + 2) * (j+1))].verticalVelocity - points[(i + (size + 2) * (j-1))].verticalVelocity) / size;
            points[(i + (size + 2) * (j))].previousHorizontalVelocity = 0;
                Debug.Log(points[(i + (size + 2) * (j))].previousVerticalVelocity + "prev velocity" + points[(i + (size + 2) * (j))].previousHorizontalVelocity);
        }
    }
    for (int i = 1; i <= size; i++)
    {
        for (int j = 1; j <= size; j++)
        {
                points[(i + (size + 2) * (j))].SetHorizontalVelocity(points[(i + (size + 2) * (j))].horizontalVelocity -= 0.5f * size * (points[((i+1) + (size + 2) * (j))].previousHorizontalVelocity - points[((i-1) + (size + 2) * (j))].previousHorizontalVelocity));
                points[(i + (size + 2) * (j))].SetVerticalVelocity( points[(i + (size + 2) * (j))].verticalVelocity -= 0.5f * size * (points[(i + (size + 2) * (j+1))].previousHorizontalVelocity - points[(i + (size + 2) * (j - 1))].previousHorizontalVelocity));
        }
    }

    }

    void ProjectVelocity(float div)
    {
        float h;
        h = 1.0f / size;

        for(int i = 1; i<= size; i++)
        {
            for(int j = 1; j <= size; j++)
            {
                points[i + (size + 2) * j].diffusion = (float) -0.5 * h * (points[(i + 1) + (size + 2) * j].horizontalVelocity - points[(i - 1) + (size + 2) * j].horizontalVelocity
                    + points[i + (size + 2) * (j + 1)].verticalVelocity - points[i + (size + 2) * (j - 1)].verticalVelocity);


            }
        }

        for(int k = 0; k < 20; k++)
        {
            for(int i = 1; i <=size; i++)
            {
                for(int j =1; j <= size; j++)
                {
                    points[i + (size + 2) * j].SetDensity((points[i + (size + 2) * j].diffusion + points[(i - 1) + (size + 2) * j].density + points[(i + 1) + (size + 2) * j].density
                      + points[i + (size + 2) *( j - 1)].density + points[i + (size + 2) *( j + 1)].density) / 4);
                }
            }
        }

        for(int i =1; i <= size; i++)
        {
            for(int j = 1; j <= size; j++)
            {
                points[i + (size + 2) * j].SetHorizontalVelocity(points[i + (size + 2) * j].horizontalVelocity -= 0.5f * (points[i + 1 + (size + 2) * j].density - points[i - 1 + (size + 2) * j].density));
                points[i + (size + 2) * j].SetVerticalVelocity(points[i + (size + 2) * j].verticalVelocity -= 0.5f * (points[i + (size + 2) * j+ 1].density - points[i + (size + 2) * j - 1].density));
            }
        }
    }*/

    public void SliderChanged()
    {
        newsize = (int)slider.value;
    }

    public void VertVelChanged()
    {
        newVertVel = float.Parse(vertVel.text);
    }

    public void HorizVelChanged()
    {
         newHorzVel = float.Parse(horVel.text);
        Debug.Log("changed horz" + newHorzVel);
    }

    public void Reset()
    {
       
        for( int i =0; i < points.Count; i++)
        {
            points[i].SetDensity(0);
            points[i].SetPreviousDensity(0);
        }
        cube.transform.position = new Vector3(7, 0, 5);
    }

    public void Apply()
    {
 
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        points.Clear();

        UpdateValues();

        DrawGrid();
        SetNeighbours();

    }

    void UpdateValues()
    {
        size = newsize;
        verticalVelocity = newVertVel;
        horizontalVelocity = newHorzVel;
        Debug.Log("changed horz" + newHorzVel + "horz" + horizontalVelocity);
    }
}
