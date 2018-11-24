using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour {
    public int size;
    public GameObject pointData;
    List<PointData> points;
	// Use this for initialization
	void Start () {
        DrawGrid();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Debug.Log("cast");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                //agent.destination = hit.point;
                
                if(hit.collider.gameObject.GetComponent<PointData>())
                {
                    PointData point = hit.collider.gameObject.GetComponent<PointData>();
                   point.SetDensitySource(100.0f);
                    Debug.Log("mouse pos" + hit.point.x + hit.point.z);

                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0; i< size; i++)
        {
            for (int j = 0; j < size; j++)
            {
             //   Gizmos.DrawSphere(new Vector3(i, 0, j), 0.03f);
            }
        }
    }

    void DrawGrid()
    {
        for (int i = 0; i <= size; i++)
        {
            for (int j = 0; j <= size; j++)
            {
                Instantiate(pointData,new Vector3(i, 0,j), new Quaternion(1.0f,0,0,1));
                
            }
        }
    }

    void Density(float source, int b, float diffusion)
    {
        AddSource(source);
        //Swap()
        Diffuse(b, diffusion);
        //swap
       // Advect
    }

    void AddSource(float source)
    {
        int totalSize = (size + 2) * (size + 2);
        for (int i = 0; i < totalSize; i++)
        {
            points[i].density += Time.deltaTime * points[i].GetSource();
        }
    }

    void Diffuse( int b, float diffusion)
    {
        float a = Time.deltaTime * diffusion * size * size;
        LinearSolver( b, a, 1 + 4 * a);
    }

    void LinearSolver(int b, float a, float c)
    {
        for (int k = 0; k < 20; k++)
        {

            for (int i = 0; i <= size; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    points[(i + (size + 2) * j)].density = (points[(i + (size + 2) * j)].GetPreviousDensity() + a * (points[((i - 1) + (size + 2) * j)].density +
                    points[((i + 1) + (size + 2) * j)].density + points[(i + (size + 2) * (j - 1))].density + points[(i + (size + 2) * (j + 1))].density)) / c;
                }
            }

          //  SetBoundaryDensity(b); x vertical vel
        }
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
        
        for (int i = 1; i <= size; i++)
        {
            for (int j = 1; j <= size; j++)
            {
                float x = i - dt * points[(i + (size + 2) * (j))].GetHorizontalVelocity();
                float y = j - dt * points[(i + (size + 2) * (j))].GetVerticalVelocity();
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
                points[(i + (size + 2) * (j))].density = s0 * (t0 * points[(i0 + (size + 2) * (j0))].GetPreviousDensity() + t1 * points[(i0 + (size + 2) * (j1))].GetPreviousDensity()) +
                             s1 * (t0 * points[(i1 + (size + 2) * (j0))].GetPreviousDensity() + t1 * points[(i1 + (size + 2) * (j1))].GetPreviousDensity());
            }

        }

        SetBoundaryDensity(b);
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
}
