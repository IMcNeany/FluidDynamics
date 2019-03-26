using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid2D : MonoBehaviour {
    public int size = 4;
    public GameObject pointData;
    public List<PointData> points = new List<PointData>();
    public Slider slider;
    public InputField vertVel;
    public InputField horVel;
    public GameObject cube;
    float verticalVelocity = 0.0f;
    float horizontalVelocity = 0.0f;
    float diffusionRate = 0.5f;
    float viscosity = 0.2f;
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
    
        for(int i = 0; i < points.Count; i++)
        {
            points[i].SetDensity(points[i].density -= Time.deltaTime * points[i].GetPreviousDensity());
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
       for (int g = 0; g < 20; g++)
        {
            //for size of points linear solve

            // points[i].diffuse(a, c);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int k = i + 1;
                    int l = i - 1;
                    int m = j - 1;
                    int n = j + 1;

                    if (l < 0)
                    {
                        l = i;
                    }
                    if (m < 0)
                    {
                        m = j;
                    }
                    if (k >= size)
                    {
                        k = i;
                    }
                    if (n >= size)
                    {
                        n = i;
                    }

                   points[(i + (size) * (j))].SetDensity(a* (points[(i + (size) * (j))].GetPreviousDensity() +  (points[((k) + (size) * (j))].density + points[((l) + (size) * (j))].density +
                   (points[(i + (size) * (n))].density + points[(i + (size) * (m))].density)))/c);
                }
            }

        }
    }

    void AdvectDensity()
    {
        float dt0 = Time.deltaTime * points.Count;
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k <= size; k++)
            {


                float x = i - dt0 * points[k].GetHorizontalVelocity();
                float y = k - dt0 * points[k].GetVerticalVelocity();

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
                        j1 = 0;
                    }*/
             
                if (j0 == size)
                {
                    j0 = size - 1;
                }
                if(j1 >= size)
                {
                    j1 = j0;
                }
                if(i0 == size)
                {
                    i0 = size - 1;
                }
               if(i1 >= size)
                {
                    i1 = i0;
                }
               

                points[i].SetDensity(s0 * (t0 * points[(i0) + (size) * (j0)].GetPreviousDensity() + t1 * points[(i0) + (size) * (j1)].GetPreviousDensity())
                          + s1 * (t0 * points[(i1) + (size) * (j0)].GetPreviousDensity() + t1 * points[(i1) + (size) * (j1)].GetPreviousDensity()));
            }
        }
   
    }

    void VelocityStep()
    {
        AddVelocitySource();
        SwapHorizontalVelocity();
        DiffuseHorizontalVelocity();
        SwapVerticalVelocity();
        DiffuseVerticalVelocity();
        ProjectVelocity();
        SwapHorizontalVelocity();
        SwapVerticalVelocity();
        AdvectHorizontalVelocity();
        AdvectVerticalVelocity();
        ProjectVelocity();
    }

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
        VelocityStep();
        DensityStep();
      
    }


    void AddVelocitySource()
    {
        // int densSize = (size) + (size);
        //poss denssize but idkk
        for (int i = 0; i < points.Count; i++)
        {
            points[i].SetHorizontalVelocity(points[i].horizontalVelocity += Time.deltaTime * points[i].previousHorizontalVelocity);
            points[i].SetVerticalVelocity(points[i].verticalVelocity += Time.deltaTime * points[i].previousVerticalVelocity);
        }
    }

    void SwapHorizontalVelocity()
    {
        for (int i = 0; i < points.Count; i++)
        {
            float tmp = points[i].previousHorizontalVelocity;
            points[i].previousHorizontalVelocity = points[i].horizontalVelocity;
            points[i].SetHorizontalVelocity(tmp);
        }
    }
    void SwapVerticalVelocity()
    {
        for (int i = 0; i < points.Count; i++)
        {
            float tmp = points[i].previousVerticalVelocity;
            points[i].previousVerticalVelocity = points[i].verticalVelocity;
            points[i].SetVerticalVelocity(tmp);
        }
    }

    void DiffuseHorizontalVelocity()
    {
        float a = Time.deltaTime * viscosity * size * size;
        HorizontalVelocityLinearSolver(a, 1 + 4 * a);
    }

    void HorizontalVelocityLinearSolver(float a, float c)
    {
        for (int f = 0; f < 20; f++)
        {
            //for size of points linear solve
            for (int i = 0; i < size; i++)
            {
                //    points[i].DiffuseHorizontalVelocity(a, c);

                for (int j = 0; j < size; j++)
                {
                    int k = i + 1;
                    int l = i - 1;
                    int m = j - 1;
                    int n = j + 1;

                    if (l < 0)
                    {
                        l = i;
                    }
                    if (m < 0)
                    {
                        m = j;
                    }
                    if (k >= size)
                    {
                        k = i;
                    }
                    if (n >= size)
                    {
                        n = i;
                    }

                    points[(i + (size) * (j))].SetHorizontalVelocity(a * (points[(i + (size) * (j))].previousHorizontalVelocity+ (points[((k) + (size) * (j))].GetHorizontalVelocity() + points[((l) + (size) * (j))].GetHorizontalVelocity() +
                    (points[(i + (size) * (n))].GetHorizontalVelocity() + points[(i + (size) * (m))].GetHorizontalVelocity()))) / c);
                }
            }

        }
    }

    void DiffuseVerticalVelocity()
    {
        float a = Time.deltaTime * viscosity * size * size;
        VerticalVelocityLinearSolver(a, 1 + 4 * a);
    }

    void VerticalVelocityLinearSolver(float a, float c)
    {
        for (int f = 0; f < 20; f++)
        {
            //for size of points linear solve
            for (int i = 0; i < size; i++)
            {
              //  points[i].DiffuseVerticalVelocity(a, c);
              for (int j =0; j <size; j++)
                {
                    int k = i + 1;
                    int l = i - 1;
                    int m = j - 1;
                    int n = j + 1;

                    if (l < 0)
                    {
                        l = i;
                    }
                    if (m < 0)
                    {
                        m = j;
                    }
                    if (k >= size)
                    {
                        k = i;
                    }
                    if (n >= size)
                    {
                        n = i;
                    }

                    points[(i + (size) * (j))].SetVerticalVelocity(a * (points[(i + (size) * (j))].previousVerticalVelocity + (points[((k) + (size) * (j))].GetVerticalVelocity() + points[((l) + (size) * (j))].GetVerticalVelocity() +
                    (points[(i + (size) * (n))].GetVerticalVelocity() + points[(i + (size) * (m))].GetVerticalVelocity()))) / c);
                }
            }

        }
    }

    void ProjectVelocity()
    {
        float h = 1.0f / size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int k = i + 1;
                int l = i - 1;
                int m = j - 1;
                int n = j + 1;

                if(l < 0)
                {
                    l = i;
                }
                if(m < 0)
                {
                    m = j;
                }
                if(k >= size)
                {
                    k = i;
                }
                if (n >= size)
                {
                    n = i;
                }


                points[(i + (size) * (j))].previousVerticalVelocity = -0.5f * h* (points[((k) + (size) * (j))].horizontalVelocity
                    - points[((l) + (size) * (j))].horizontalVelocity + points[(i + (size) * (n))].verticalVelocity - points[(i + (size) * (m))].verticalVelocity) / size;
                points[(i + (size) * (j))].previousHorizontalVelocity = 0;
                Debug.Log(points[(i + (size) * (j))].previousVerticalVelocity + "prev velocity" + points[(i + (size) * (j))].previousHorizontalVelocity);
            }
        }
        for (int a = 0; a < 20; a++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int k = i + 1;
                    int l = i - 1;
                    int m = j - 1;
                    int n = j + 1;

                    if (l < 0)
                    {
                        l = i;
                    }
                    if (m < 0)
                    {
                        m = j;
                    }
                    if (k >= size)
                    {
                        k = i;
                    }
                    if (n >= size)
                    {
                        n = i;
                    }


                    points[(i + (size) * (j))].previousHorizontalVelocity = (points[((l) + (size) * (j))].previousVerticalVelocity
                        + points[((k) + (size) * (j))].previousHorizontalVelocity + points[(i + (size) * (m))].previousHorizontalVelocity + points[(i + (size) * (n))].previousHorizontalVelocity) / 4;
                }
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int k = i + 1;
                int l = i - 1;
                int m = j - 1;
                int n = j + 1;

                if (l < 0)
                {
                    l = i;
                }
                if (m < 0)
                {
                    m = j;
                }
                if (k >= size)
                {
                    k = i;
                }
                if (n >= size)
                {
                    n = i;
                }

                points[(i + (size) * (j))].SetHorizontalVelocity(points[(i + (size) * (j))].horizontalVelocity -= 0.5f * size * (points[((k) + (size) * (j))].previousHorizontalVelocity - points[((l) + (size) * (j))].previousHorizontalVelocity)/h);
                points[(i + (size) * (j))].SetVerticalVelocity(points[(i + (size) * (j))].verticalVelocity -= 0.5f * size * (points[(i + (size) * (n))].previousHorizontalVelocity - points[(i + (size) * (m))].previousHorizontalVelocity)/h);
            }
        }
    }

    void AdvectHorizontalVelocity()
    {
        float dt0 = Time.deltaTime * size;
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k <= size; k++)
            {
                float x = i - dt0 * points[i].previousHorizontalVelocity;
                float y = k - dt0 * points[i].previousVerticalVelocity;

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
                if (j0 == size)
                {
                    j0 = size - 1;
                }
                if (j1 >= size)
                {
                    j1 = j0;
                }
                if (i0 == size)
                {
                    i0 = size - 1;
                }
                if (i1 >= size)
                {
                    i1 = i0;
                }

                points[i].SetHorizontalVelocity(s0 * (t0 * points[(i0) + (size) * (j0)].previousHorizontalVelocity + t1 * points[(i0) + (size) * (j1)].previousHorizontalVelocity)
                          + s1 * (t0 * points[(i1) + (size) * (j0)].previousHorizontalVelocity + t1 * points[(i1) + (size) * (j1)].previousHorizontalVelocity));
            }
        }
    }

    void AdvectVerticalVelocity()
    {
        float dt0 = Time.deltaTime * size;
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k <= size; k++)
            {
                float x = i - dt0 * points[i].previousHorizontalVelocity;
                float y = k - dt0 * points[i].previousVerticalVelocity;

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
                if (j0 == size)
                {
                    j0 = size - 1;
                }
                if (j1 >= size)
                {
                    j1 = j0;
                }
                if (i0 == size)
                {
                    i0 = size - 1;
                }
                if (i1 >= size)
                {
                    i1 = i0;
                }

                points[i].SetVerticalVelocity(s0 * (t0 * points[(i0) + (size ) * (j0)].previousVerticalVelocity + t1 * points[(i0) + (size) * (j1)].previousVerticalVelocity)
                          + s1 * (t0 * points[(i1) + (size) * (j0)].previousVerticalVelocity + t1 * points[(i1) + (size) * (j1)].previousVerticalVelocity));
            }
        }
    }
    void DrawGrid()
    {
        float spacing = 12.0f / size;
        Debug.Log(spacing + "spacing " + size + "size" 
            + 12/size);
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
                int randomNumber = Random.Range(0, 100);
                newPoint.GetComponent<PointData>().SetDensity(randomNumber);
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
