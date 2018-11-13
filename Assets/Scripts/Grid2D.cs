using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour {
    public int width;
    public int height;
    public GameObject pointData;
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
                    point.SetDensitySource(new Vector2(hit.point.x, hit.point.z));
                    Debug.Log("mouse pos" + hit.point.x + hit.point.z);

                }
            }
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
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                Instantiate(pointData,new Vector3(i, 0,j), new Quaternion(1.0f,0,0,1));
                
            }
        }
    }


}
