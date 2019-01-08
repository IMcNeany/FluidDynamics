using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour {
    public int size;
    public GameObject pointData;
    public List<PointData> points = new List<PointData>();
    float diffusionRate = 1.0f;
    int b = 1;
    public List<PointData> DiffuseNodes = new List<PointData>();
    public Material mat;
    int nodeCount = 0;
    bool hitOne = true;
    // Use this for initialization
    void Start () {
        DrawGrid();
        SetNeighbours();
       
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
                        point.SetDensitySource(100.0f);

                        //  Debug.Log("mouse pos" + hit.point.x + hit.point.z);
                       
                    }
               


              //  Diffuse( b, diffusionRate);
            }
        }

        Diffuse(b, diffusionRate);
    }

    void DrawGrid()
    {
        float spacing = 12 / size;
   
        float xValue = 0.5f;
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

                xValue += spacing;
            }
            xValue = 0.5f;
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

    void Density(int b, float diffusionRate)
    {
       // AddSource(source);
        //Swap()
        Diffuse(b, diffusionRate);
        //swap
        //Advect(b);
    }

    /*void AddSource(float source)
    {
        int totalSize = size * size;
        for (int i = 0; i < totalSize; i++)
        {
            points[i].density += Time.deltaTime * points[i].GetSource();
        }
    }*/

    void Diffuse( int b, float diffusion)
    {
        float a = Time.deltaTime * diffusion * size * size;
        Debug.Log(a + "a");
        LinearSolver( b, a, 1 + 4 * a);
    }

    void LinearSolver(int b, float a, float c)
    {

        for (int i = 0; i < size; i++)
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


    void SetBoundaryDensity(int b)
    {
        int i;

        for (i = 1; i <= size; i++)
        {
            points[(0 + (size + 2 * (i)))].density = b == 1 ? -points[((1) + (size + 2) * (i))].density : points[((1) + (size + 2) * (i))].density;

            points[((size + 1) + (size + 2) * (i))].density = b == 1 ? -points[((size) + (size + 2) * (i))].density : points[((size) + (size + 2) * (i))].density;

            points[(i + (size + 2) * (0))].density = b == 2 ? -points[(i + (size + 2) * 1)].density : points[(i + (size + 2) * 1)].density;

            points[(i + (size + 2) * (size + 1))].density = b == 2 ? -points[(i + (size + 2) * (size))].density : points[(i + (size + 2) * (size))].density;
        }
        points[(0 + (size + 2) * (0))].density = 0.5f * (points[(1 + (size + 2) * (0))].density + points[(0 + (size + 2) * (1))].density);
        points[(0 + (size + 2) * (size + 1))].density = 0.5f * (points[(1 + (size + 2) * (size + 1))].density + points[(0 + (size + 2) * (size))].density);
        points[((size + 1) + (size + 2) * (0))].density = 0.5f * (points[((size) + (size + 2) * (0))].density + points[((size + 1) + (size + 2) * (1))].density);
        points[((size + 1) + (size + 2) * (size + 1))].density = 0.5f * (points[(size + (size + 2) * (size + 1))].density + points[((size + 1) + (size + 2) * (size))].density);
     }

    void SetBoundaryHorizontalVelocity(int b)
    {

        for (int i = 1; i <= size; i++)
        {
            points[(0 + (size + 2 * (i)))].horizontalVelocity = b == 1 ? -points[((1) + (size + 2) * (i))].horizontalVelocity : points[((1) + (size + 2) * (i))].horizontalVelocity;

            points[((size + 1) + (size + 2) * (i))].horizontalVelocity = b == 1 ? -points[((size) + (size + 2) * (i))].horizontalVelocity : points[((size) + (size + 2) * (i))].horizontalVelocity;

            points[(i + (size + 2) * (0))].horizontalVelocity = b == 2 ? -points[(i + (size + 2) * 1)].horizontalVelocity : points[(i + (size + 2) * 1)].horizontalVelocity;

            points[(i + (size + 2) * (size + 1))].horizontalVelocity = b == 2 ? -points[(i + (size + 2) * (size))].horizontalVelocity : points[(i + (size + 2) * (size))].horizontalVelocity;
        }
        points[(0 + (size + 2) * (0))].horizontalVelocity = 0.5f * (points[(1 + (size + 2) * (0))].horizontalVelocity + points[(0 + (size + 2) * (1))].horizontalVelocity);
        points[(0 + (size + 2) * (size + 1))].horizontalVelocity = 0.5f * (points[(1 + (size + 2) * (size + 1))].horizontalVelocity + points[(0 + (size + 2) * (size))].horizontalVelocity);
        points[((size + 1) + (size + 2) * (0))].horizontalVelocity = 0.5f * (points[((size) + (size + 2) * (0))].horizontalVelocity + points[((size + 1) + (size + 2) * (1))].horizontalVelocity);
        points[((size + 1) + (size + 2) * (size + 1))].horizontalVelocity = 0.5f * (points[(size + (size + 2) * (size + 1))].horizontalVelocity + points[((size + 1) + (size + 2) * (size))].horizontalVelocity);
    }

    void SetBoundaryVerticalVelocity(int b)
    {

        for (int i = 1; i <= size; i++)
        {
            points[(0 + (size + 2 * (i)))].verticalVelocity = b == 1 ? -points[((1) + (size + 2) * (i))].verticalVelocity : points[((1) + (size + 2) * (i))].verticalVelocity;

            points[((size + 1) + (size + 2) * (i))].verticalVelocity = b == 1 ? -points[((size) + (size + 2) * (i))].verticalVelocity : points[((size) + (size + 2) * (i))].verticalVelocity;

            points[(i + (size + 2) * (0))].verticalVelocity = b == 2 ? -points[(i + (size + 2) * 1)].verticalVelocity : points[(i + (size + 2) * 1)].verticalVelocity;

            points[(i + (size + 2) * (size + 1))].verticalVelocity = b == 2 ? -points[(i + (size + 2) * (size))].verticalVelocity : points[(i + (size + 2) * (size))].verticalVelocity;
        }
        points[(0 + (size + 2) * (0))].verticalVelocity = 0.5f * (points[(1 + (size + 2) * (0))].verticalVelocity + points[(0 + (size + 2) * (1))].verticalVelocity);
        points[(0 + (size + 2) * (size + 1))].verticalVelocity = 0.5f * (points[(1 + (size + 2) * (size + 1))].verticalVelocity + points[(0 + (size + 2) * (size))].verticalVelocity);
        points[((size + 1) + (size + 2) * (0))].verticalVelocity = 0.5f * (points[((size) + (size + 2) * (0))].verticalVelocity + points[((size + 1) + (size + 2) * (1))].verticalVelocity);
        points[((size + 1) + (size + 2) * (size + 1))].verticalVelocity = 0.5f * (points[(size + (size + 2) * (size + 1))].verticalVelocity + points[((size + 1) + (size + 2) * (size))].verticalVelocity);
    }

    void SetBoundaryPreviousVerticalVelocity(int b)
    {

        for (int i = 1; i <= size; i++)
        {
            points[(0 + (size + 2 * (i)))].previousVerticalVelocity = b == 1 ? -points[((1) + (size + 2) * (i))].previousVerticalVelocity : points[((1) + (size + 2) * (i))].previousVerticalVelocity;

            points[((size + 1) + (size + 2) * (i))].previousVerticalVelocity = b == 1 ? -points[((size) + (size + 2) * (i))].previousVerticalVelocity : points[((size) + (size + 2) * (i))].previousVerticalVelocity;

            points[(i + (size + 2) * (0))].previousVerticalVelocity = b == 2 ? -points[(i + (size + 2) * 1)].previousVerticalVelocity : points[(i + (size + 2) * 1)].previousVerticalVelocity;

            points[(i + (size + 2) * (size + 1))].previousVerticalVelocity = b == 2 ? -points[(i + (size + 2) * (size))].previousVerticalVelocity : points[(i + (size + 2) * (size))].previousVerticalVelocity;
        }
        points[(0 + (size + 2) * (0))].previousVerticalVelocity = 0.5f * (points[(1 + (size + 2) * (0))].previousVerticalVelocity + points[(0 + (size + 2) * (1))].previousVerticalVelocity);
        points[(0 + (size + 2) * (size + 1))].previousVerticalVelocity = 0.5f * (points[(1 + (size + 2) * (size + 1))].previousVerticalVelocity + points[(0 + (size + 2) * (size))].previousVerticalVelocity);
        points[((size + 1) + (size + 2) * (0))].previousVerticalVelocity = 0.5f * (points[((size) + (size + 2) * (0))].previousVerticalVelocity + points[((size + 1) + (size + 2) * (1))].previousVerticalVelocity);
        points[((size + 1) + (size + 2) * (size + 1))].previousVerticalVelocity = 0.5f * (points[(size + (size + 2) * (size + 1))].previousVerticalVelocity + points[((size + 1) + (size + 2) * (size))].previousVerticalVelocity);
    }

    void SetBoundaryPreviousHorizontalVelocity(int b)
    {

        for (int i = 1; i <= size; i++)
        {
            points[(0 + (size + 2 * (i)))].previousHorizontalVelocity = b == 1 ? -points[((1) + (size + 2) * (i))].previousHorizontalVelocity : points[((1) + (size + 2) * (i))].previousHorizontalVelocity;

            points[((size + 1) + (size + 2) * (i))].previousHorizontalVelocity = b == 1 ? -points[((size) + (size + 2) * (i))].previousHorizontalVelocity : points[((size) + (size + 2) * (i))].previousHorizontalVelocity;

            points[(i + (size + 2) * (0))].previousHorizontalVelocity = b == 2 ? -points[(i + (size + 2) * 1)].previousHorizontalVelocity : points[(i + (size + 2) * 1)].previousHorizontalVelocity;

            points[(i + (size + 2) * (size + 1))].previousHorizontalVelocity = b == 2 ? -points[(i + (size + 2) * (size))].previousHorizontalVelocity : points[(i + (size + 2) * (size))].previousHorizontalVelocity;
        }
        points[(0 + (size + 2) * (0))].previousHorizontalVelocity = 0.5f * (points[(1 + (size + 2) * (0))].previousHorizontalVelocity + points[(0 + (size + 2) * (1))].previousHorizontalVelocity);
        points[(0 + (size + 2) * (size + 1))].previousHorizontalVelocity = 0.5f * (points[(1 + (size + 2) * (size + 1))].previousHorizontalVelocity + points[(0 + (size + 2) * (size))].previousHorizontalVelocity);
        points[((size + 1) + (size + 2) * (0))].previousHorizontalVelocity = 0.5f * (points[((size) + (size + 2) * (0))].previousHorizontalVelocity + points[((size + 1) + (size + 2) * (1))].previousHorizontalVelocity);
        points[((size + 1) + (size + 2) * (size + 1))].previousHorizontalVelocity = 0.5f * (points[(size + (size + 2) * (size + 1))].previousHorizontalVelocity + points[((size + 1) + (size + 2) * (size))].previousHorizontalVelocity);
    }

    void Advect( int b)
    {
      
        float dt = Time.deltaTime * size;
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Debug.Log(i + " i1 " + j + " j1");
                //Debug.Log(i * size)
                float x = i - dt * points[(i * size + j)].GetVerticalVelocity();
            
                float y = j - dt * points[(i * size + j)].GetHorizontalVelocity();
               
                if (x < 0.5f)
                {
                    x = 0.5f;
                   
                }
               // if (x > size + 0.5f)
                //{
                  //  x = size + 0.5f;
                    
                //}
              
                int i0 = (int)x;
                int i1 = i0 + 1;

                if (y < 0.5f)
                {
                    y = 0.5f;
                   
                }
                //if (y > size + 0.5f)
                //{
                //    y = size + 0.5f;
                    
              //  }
               
                int j0 = (int)y;
                int j1 = j0 + 1;
                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;
     

                if (j1 != size && i1 != size)
                {
                    
                    points[(i * size + (j))].density = s0 * ((t0 * points[(i0 * size + (j0))].GetPreviousDensity()) + (t1 * points[(i0 * size + (j1))].GetPreviousDensity()) + s1 * ((t0 * points[(i1 * size + (j0))].GetPreviousDensity()) + (t1 * points[(i1 * size + (j1))].GetPreviousDensity())));
                }

                // t0 i0 j1 top left
                //t0 i1 j1 bottom left
                //t0 i1 j1 bottom left
                //t1 i0j0 disappears
                //t1 i1 j0 top left
                //t1 i1 j1 dissapeaers
                //t1 i0 j1 disapppeears


                //  Debug.Log( i  + j + "i j " + i0  +j0 + " i0j0 " + i0 + j1 + " i0 j1 ");

                // Debug.Log(s0 * (t0 * points[(i0 * size + (j0))].GetPreviousDensity() + t1 * points[(i0 * size + (j1))].GetPreviousDensity()) +  s1 * (t0 * points[(i1 * size + (j0))].GetPreviousDensity() + t1 * points[(i1 * size + (j1))].GetPreviousDensity()));
                // Debug.Log("sdvent desnity" + points[(i + (size + 2) * (j))].density);
            }
           
        }
        //SetBoundaryDensity(b);
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
        }
    }

    SetBoundaryPreviousVerticalVelocity( 0);
    SetBoundaryPreviousHorizontalVelocity(0);
        //p = prev h vel
        //div = prec v vel
    //LinearSolver(N, 0, p, div, 1, 4);

    for (int i = 1; i <= size; i++)
    {
        for (int j = 1; j <= size; j++)
        {
            points[(i + (size + 2) * (j))].horizontalVelocity -= 0.5f * size * (points[((i+1) + (size + 2) * (j))].previousHorizontalVelocity - points[((i-1) + (size + 2) * (j))].previousHorizontalVelocity);
            points[(i + (size + 2) * (j))].verticalVelocity -= 0.5f * size * (points[(i + (size + 2) * (j+1))].previousHorizontalVelocity - points[(i + (size + 2) * (j - 1))].previousHorizontalVelocity);
        }
    }

        SetBoundaryHorizontalVelocity(1);
        SetBoundaryVerticalVelocity(2);

    }

    public void Reset()
    {
        for( int i =0; i < points.Count; i++)
        {
            points[i].SetDensity(0);
            points[i].SetPreviousDensity(0);
        }
    }
}
