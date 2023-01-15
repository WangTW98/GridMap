using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Mesh mesh;
    
    public Vector3[] vertexs = new Vector3[16];
    
    public int[] triangles = new int[54];

    
    public GridCell neighborTop;
    public GridCell neighborBottom;
    public GridCell neighborLeft;
    public GridCell neighborRight;
    
    public GridCell neighborTopLeft;
    public GridCell neighborTopRight;
    public GridCell neighborBottomRight;
    public GridCell neighborBottomLeft;
    
    
    public enum QuadDirections
    {
        Top, Bottom, Left, Right
    }
    //设置顶点位置
    Vector3[] setVertex()
    {
        //约定第一个顶点在原点,从左向右，从上到下的顺序排序
        vertexs[0] = new Vector3(0.2f, 0, 0.2f);
        vertexs[1] = new Vector3(0.8f, 0, 0.2f);
        vertexs[2] = new Vector3(0.8f, 0, 0.8f);
        vertexs[3] = new Vector3(0.2f, 0, 0.8f);
        vertexs[4] = new Vector3(0, 0, 0);
        vertexs[5] = new Vector3(1, 0, 0);
        vertexs[6] = new Vector3(1, 0, 1);
        vertexs[7] = new Vector3(0, 0, 1);
        vertexs[8] = new Vector3(0.2f, 0, 0);
        vertexs[9] = new Vector3(0.8f, 0, 0);
        vertexs[10] = new Vector3(1, 0, 0.2f);
        vertexs[11] = new Vector3(1, 0, 0.8f);
        vertexs[12] = new Vector3(0.8f, 0, 1);
        vertexs[13] = new Vector3(0.2f, 0, 1);
        vertexs[14] = new Vector3(0, 0, 0.8f);
        vertexs[15] = new Vector3(0, 0, 0.2f);

        return vertexs;
    }

    int[] setTriangle()
    {
        //在顺序数组中保存的就是顶点在顶点数组中的下标位置
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;
        
        triangles[6] = 15;
        triangles[7] = 8;
        triangles[8] = 4;
        triangles[9] = 15;
        triangles[10] = 0;
        triangles[11] = 8;
        
        triangles[12] = 8;
        triangles[13] = 1;
        triangles[14] = 9;
        triangles[15] = 8;
        triangles[16] = 0;
        triangles[17] = 1;
        
        triangles[18] = 9;
        triangles[19] = 10;
        triangles[20] = 5;
        triangles[21] = 9;
        triangles[22] = 1;
        triangles[23] = 10;
        
        
        triangles[24] = 1;
        triangles[25] = 11;
        triangles[26] = 10;
        triangles[27] = 1;
        triangles[28] = 2;
        triangles[29] = 11;
        
        triangles[30] = 12;
        triangles[31] = 6;
        triangles[32] = 11;
        triangles[33] = 12;
        triangles[34] = 11;
        triangles[35] = 2;
        
        
        triangles[36] = 3;
        triangles[37] = 12;
        triangles[38] = 2;
        triangles[39] = 3;
        triangles[40] = 13;
        triangles[41] = 12;
        
        
        triangles[42] = 14;
        triangles[43] = 13;
        triangles[44] = 3;
        triangles[45] = 14;
        triangles[46] = 7;
        triangles[47] = 13;
        
        
        triangles[48] = 15;
        triangles[49] = 3;
        triangles[50] = 0;
        triangles[51] = 15;
        triangles[52] = 14;
        triangles[53] = 3;
        return triangles;
    }

    public void Awake()
    {
        setMesh();
        // setPlantHeight(1);
        // updateEdge(QuadDirections.Top, 1);
        updateMesh();
    }

    // Use this for initialization
    public void setMesh () {
        vertexs = setVertex();
        triangles = setTriangle();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertexs;
        mesh.triangles = triangles;
        this.AddComponent<BoxCollider>();
        this.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0, 0.5f);
        this.GetComponent<BoxCollider>().size = new Vector3(1, 0, 1);
    }

    public void updateMesh()
    {
        mesh.vertices = vertexs;
        mesh.triangles = triangles;
    }

    public void setPlantHeight(float height)
    {
        vertexs[0].y = height;
        vertexs[1].y = height;
        vertexs[2].y = height;
        vertexs[3].y = height;
        
        this.GetComponent<BoxCollider>().center = new Vector3(0.5f, height, 0.5f);
    }

    public void updateEdge(QuadDirections direction, float height)
    {
        switch (direction)
        {
            case QuadDirections.Top:
                vertexs[12].y = height;
                vertexs[13].y = height;
                break;
            case QuadDirections.Bottom:
                vertexs[8].y = height;
                vertexs[9].y = height;
                break;
            case QuadDirections.Left:
                vertexs[14].y = height;
                vertexs[15].y = height;
                break;
            case QuadDirections.Right:
                vertexs[10].y = height;
                vertexs[11].y = height;
                break;
        }
    }

    private void OnMouseOver()
    {
        this.GetComponent<Renderer>().materials[0].color = Color.red;
        if (neighborTop)
        {
            neighborTop.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborBottom)
        {
            neighborBottom.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborLeft)
        {
            neighborLeft.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborRight)
        {
            neighborRight.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborTopLeft)
        {
            neighborTopLeft.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborTopRight)
        {
            neighborTopRight.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborBottomRight)
        {
            neighborBottomRight.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }

        if (neighborBottomLeft)
        {
            neighborBottomLeft.GetComponent<Renderer>().materials[0].color = Color.magenta;
        }
    }
    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().materials[0].color = Color.white;
        if (neighborTop)
        {
            neighborTop.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborBottom)
        {
            neighborBottom.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborLeft)
        {
            neighborLeft.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborRight)
        {
            neighborRight.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborTopLeft)
        {
            neighborTopLeft.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborTopRight)
        {
            neighborTopRight.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborBottomRight)
        {
            neighborBottomRight.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        if (neighborBottomLeft)
        {
            neighborBottomLeft.GetComponent<Renderer>().materials[0].color = Color.white;
        }
    }
}
