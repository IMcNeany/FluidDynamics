using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public int width;
    public int height;
    public GameObject pointData;
	// Use this for initialization
	void Start () {
        DrawGrid();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Gizmos.DrawSphere(new Vector3(i, 0, j), 0.03f);
            }
        }
    }

    void DrawGrid()
    {
        for (int i = 0; i <= width-2; i++)
        {
            for (int j = 0; j <= height-2; j++)
            {
                Instantiate(pointData,new Vector3(i+0.5f,0,j+0.5f), new Quaternion(0,0,0,1));
                
            }
        }
    }
}
