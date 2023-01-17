using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    public float gridCellHeight = 0;
    // 声明一个mesh网格并赋值给MeshFilter
    public Mesh mesh;
    
    // 声明网格顶点，用于绘制mesh网格
    public Vector3[] vertexs = new Vector3[16];
    
    private Vector3[] normals = new Vector3[16];
    private Vector2[] uv = new Vector2[16];
    private Vector4[] tangents = new Vector4[16];
    
    // 声明三角顶点，用于绘制mesh网格
    // 四边形均有两个三角形绘制而成
    public int[] triangles = new int[54];

    // 声明当前网格的上下左右8个方向的相邻网格，在网格地图初始化时被赋值
    public GridCell neighborTop;
    public GridCell neighborBottom;
    public GridCell neighborLeft;
    public GridCell neighborRight;
    public GridCell neighborTopLeft;
    public GridCell neighborTopRight;
    public GridCell neighborBottomRight;
    public GridCell neighborBottomLeft;

    private GameObject gridEditorPanel;
    
    // 声明四边形网格的方向
    public enum QuadDirections
    {
        Top, Bottom, Left, Right, TopLeft, BottomLeft, TopRight, BottomRight
    }
    
    // 设置顶点位置
    Vector3[] setVertex()
    {
        //约定第一个顶点在原点,从左向右，从下到上的顺序排序
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
        // 将网格顶点按顺时针顺序存储到数组中
        // triangles[三角形顶点] = 网格顶点
        // 每三个顶点组成一个三角形，顶点连接顺序为顺时针时，生成的平面朝Y轴正向，逆时针则超Y轴负方向
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
        // editHeight = GameObject.Find("GridEditorPanel").GetComponent<GridEditor>().drawHeight;
        // 初始化网格
        setMesh();
        setPlantHeight(gridCellHeight);
        // updateEdge(QuadDirections.Top, 1);
        // 重绘网格
        redrawMesh();
    }

    // 首次初始化网格，以默认属性方式生成
    public void setMesh () {
        vertexs = setVertex();
        triangles = setTriangle();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertexs;
        mesh.triangles = triangles;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.back;
        }
        mesh.normals = normals;
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(vertexs[i].x, vertexs[i].z);
        }
        mesh.uv = uv;
        for (int i = 0; i < tangents.Length; i++)
        {
            tangents[i] = new Vector4(1f, 0f, 0f, -1f);
        }
        mesh.tangents = tangents;
        this.AddComponent<BoxCollider>();
        this.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0, 0.5f);
        this.GetComponent<BoxCollider>().size = new Vector3(1, 0, 1);
    }

    // 更改网格属性，即重新读取网格顶点信息，重新绘制三角形
    public void redrawMesh()
    {
        mesh.vertices = vertexs;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.normals = normals;
        mesh.uv = uv;
    }

    // 设置网格高度
    public void setPlantHeight(float height)
    {
        vertexs[0].y = height;
        vertexs[1].y = height;
        vertexs[2].y = height;
        vertexs[3].y = height;
        // 网格高度发生改变，则相应的碰撞盒发生改变
        this.GetComponent<BoxCollider>().center = new Vector3(0.5f, height, 0.5f);
        gridCellHeight = height;
    }

    // 更新网格的4条外部边高度
    // 当网格高度改变，相邻网格高度未改变时，临边高度不变
    // 当网格高度改变，相邻网格高度与当前网格一致，临边高度与网格高度一致
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
        
        redrawMesh();
    }
    
    public void updatePoint(QuadDirections direction, float height)
    {
        switch (direction)
        {
            case QuadDirections.TopLeft:
                vertexs[7].y = height;
                break;
            case QuadDirections.TopRight:
                vertexs[6].y = height;
                break;
            case QuadDirections.BottomLeft:
                vertexs[4].y = height;
                break;
            case QuadDirections.BottomRight:
                vertexs[5].y = height;
                break;
        }
        
        redrawMesh();
    }
    
    private void OnMouseOver()
    {
        showNeighbors();
    }

    private void OnMouseExit()
    {
        hideNeighbors();
    }

    private void OnMouseDown()
    {
        editGrid();
    }

    public void edgeCalc()
    {
        if (neighborTop && neighborTop.gridCellHeight >= gridCellHeight)
        {
            updateEdge(QuadDirections.Top, gridCellHeight);
            neighborTop.updateEdge(QuadDirections.Bottom,gridCellHeight);
        }
        else if(neighborTop && neighborTop.gridCellHeight < gridCellHeight)
        {
            updateEdge(QuadDirections.Top, neighborTop.gridCellHeight);
            neighborTop.updateEdge(QuadDirections.Bottom,neighborTop.gridCellHeight);
        }
        
        if (neighborBottom && neighborBottom.gridCellHeight >= gridCellHeight)
        {
            updateEdge(QuadDirections.Bottom, gridCellHeight);
            neighborBottom.updateEdge(QuadDirections.Top,gridCellHeight);
        }
        else if(neighborBottom && neighborBottom.gridCellHeight < gridCellHeight)
        {
            updateEdge(QuadDirections.Bottom, neighborBottom.gridCellHeight);
            neighborBottom.updateEdge(QuadDirections.Top,neighborBottom.gridCellHeight);
        }
        
        if (neighborLeft && neighborLeft.gridCellHeight >= gridCellHeight)
        {
            updateEdge(QuadDirections.Left, gridCellHeight);
            neighborLeft.updateEdge(QuadDirections.Right,gridCellHeight);
        }
        else if(neighborLeft && neighborLeft.gridCellHeight < gridCellHeight)
        {
            updateEdge(QuadDirections.Left, neighborLeft.gridCellHeight);
            neighborLeft.updateEdge(QuadDirections.Right,neighborLeft.gridCellHeight);
        }
        
        if (neighborRight && neighborRight.gridCellHeight >= gridCellHeight)
        {
            updateEdge(QuadDirections.Right, gridCellHeight);
            neighborRight.updateEdge(QuadDirections.Left,gridCellHeight);
        }
        else if(neighborRight && neighborRight.gridCellHeight < gridCellHeight)
        {
            updateEdge(QuadDirections.Right, neighborRight.gridCellHeight);
            neighborRight.updateEdge(QuadDirections.Left,neighborRight.gridCellHeight);
        }
    }
    
    public void pointCalc()
    {
        if (neighborTopLeft)
        {
            float[] pointHeights = { neighborTopLeft.gridCellHeight, neighborTop.gridCellHeight, neighborLeft.gridCellHeight};
            Array.Sort(pointHeights);
            updatePoint(QuadDirections.TopLeft, pointHeights[0]);
            neighborTopLeft.updatePoint(QuadDirections.BottomRight,  pointHeights[0]);
            neighborTop.updatePoint(QuadDirections.BottomLeft,  pointHeights[0]);
            neighborLeft.updatePoint(QuadDirections.TopRight,  pointHeights[0]);
        }
        if (neighborBottomLeft)
        {
            float[] pointHeights = { neighborBottomLeft.gridCellHeight, neighborBottom.gridCellHeight, neighborLeft.gridCellHeight};
            Array.Sort(pointHeights);
            updatePoint(QuadDirections.BottomLeft, pointHeights[0]);
            neighborBottomLeft.updatePoint(QuadDirections.TopRight, pointHeights[0]);
            neighborLeft.updatePoint(QuadDirections.BottomRight, pointHeights[0]);
            neighborBottom.updatePoint(QuadDirections.TopLeft, pointHeights[0]);
        }
        if (neighborTopRight)
        {
            float[] pointHeights = { neighborTopRight.gridCellHeight, neighborTop.gridCellHeight, neighborRight.gridCellHeight};
            Array.Sort(pointHeights);
            updatePoint(QuadDirections.TopRight, pointHeights[0]);
            neighborTopRight.updatePoint(QuadDirections.BottomLeft, pointHeights[0]);
            neighborTop.updatePoint(QuadDirections.BottomRight, pointHeights[0]);
            neighborRight.updatePoint(QuadDirections.TopLeft, pointHeights[0]);
        }
        if (neighborBottomRight)
        {
            float[] pointHeights = { neighborBottomRight.gridCellHeight, neighborBottom.gridCellHeight, neighborRight.gridCellHeight};
            Array.Sort(pointHeights);
            updatePoint(QuadDirections.BottomRight, pointHeights[0]);
            neighborBottomRight.updatePoint(QuadDirections.TopLeft, pointHeights[0]);
            neighborBottom.updatePoint(QuadDirections.TopRight, pointHeights[0]);
            neighborRight.updatePoint(QuadDirections.BottomLeft, pointHeights[0]);
        }
        
        redrawMesh();
    }

    // 测试方法，当鼠标悬浮在网格上时，通过颜色变化展示相邻网格
    public void showNeighbors()
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

    // 测试方法，当鼠标移开后，还原网格颜色
    public void hideNeighbors()
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

    public void editGrid()
    {
        if (GameObject.Find("GridEditorPanel/EditPanel/MaterialDrawer/MaterialDrawerToggle").GetComponent<Toggle>().isOn)
        {
            var materialName = GameObject.Find("GridEditorPanel/EditPanel/MaterialDrawer/Label").GetComponent<TMP_Text>().text;
            Material mat = Resources.Load<Material>("Materials/MapStyle/"+materialName+"/Material");
            this.GetComponent<Renderer>().material = mat;
        }
        
        if(GameObject.Find("GridEditorPanel/EditPanel/HeightSlider/GridHeightToggle").GetComponent<Toggle>().isOn)
        {
            setPlantHeight(GameObject.Find("GridEditorPanel").GetComponent<GridEditor>().drawHeightStep*GameObject.Find("GridEditorPanel").GetComponent<GridEditor>().drawHeight);
            edgeCalc();
            pointCalc();
        }
    }
}
