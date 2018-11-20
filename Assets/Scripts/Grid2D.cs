using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour {
    public int size = 10;
    public GameObject pointData;
    List<PointData> gridSize;
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
                  //  point.SetDensitySource(new Vector2(hit.point.x, hit.point.z));
                    Debug.Log("mouse pos" + hit.point.x + hit.point.z);

                }
            }
        }
    }

    void diffuse(int n, int b)
    {
        float a = Time.deltaTime * diffusion * n * n;
        for (int k = 0; k < 20; k++)
        {
            for (int i = 0; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    gridSize[(i + (n + 2) * j)].SetDensity(gridSize[((i) + (n + 2) * j)].GetPreviousDensity() + a * (gridSize[((i - 1) + (n + 2) * j)].GetDensity() + gridSize[((i + 1) + (n + 2) * j)].GetDensity() + gridSize[(i) + (n + 2) * (j - 1)].GetDensity() + gridSize[(i) + (n + 2) * (j + 1)].GetDensity()) / (1 + 4 * a));
                }
            }
            //  setBoundaries(n, b, x);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++)
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
                gridSize[((i) + (size +2) * j)] = gameObject.GetComponent<PointData>();
            }
        }
    }


}
