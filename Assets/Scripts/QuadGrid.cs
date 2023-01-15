using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGrid : MonoBehaviour
{
    // 地图宽度
    public int gridWidth;
    // 地图高度
    public int gridHeight;

    // 网格预制体
    public GridCell gridCell;

    // 将生成的网格预制体加入链表存储，后续遍历链表设置每个网格的相邻网格（距离为1）
    public List<GridCell> _gridCells;

    public void Awake()
    {
        // 创建网格地图
        createGridMap(gridWidth, gridHeight);
        // 设置网格邻居
        setGridCellNeighbors();
    }

    // 循环生成长x宽的网格地图
    public void createGridMap(int width, int height)
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                // 初始化网格预制体
                GridCell cell = Instantiate(gridCell);
                // 设置生成的网格父级
                cell.transform.parent = transform;
                // 设置每个网格的位置，使网格按照地图尺寸平铺
                cell.transform.position = new Vector3(j, 0, i);
                // 将生成的网格添加进链表进行存储，便于下一步计算相邻网格
                _gridCells.Add(cell);
            }
        }
    }

    // 设置网格邻居（四边形8个方向）
    public void setGridCellNeighbors()
    {
        // 遍历已存储网格
        for (int i = 0; i < _gridCells.Count; i++)
        {
            // 网格左侧邻居：当前网格为第一个即（1，1）时，不应存在左侧邻居，同时每行的第一个网格也不存在左侧邻居
            _gridCells[i].neighborLeft = i!=0&&(i+1)%gridWidth!=1 ?  _gridCells[i - 1] : null;
            // 网格右侧邻居：当前网格为每行中的最后一个时，不应存在右侧邻居
            _gridCells[i].neighborRight = (i + 1)%gridWidth!=0 && i+1 <= gridWidth * gridHeight ? _gridCells[i + 1] : null;
            // 网格顶部邻居：当网格处于最顶部一行时，不应存在顶部邻居
            _gridCells[i].neighborTop = i+1<=gridWidth*gridHeight-gridWidth ? _gridCells[i + gridWidth] : null;
            // 网格底部邻居：当网格处于最底部一行时，不应存在底部邻居
            _gridCells[i].neighborBottom = i+1-gridWidth>0 ? _gridCells[i - gridWidth] : null;
            // 网格右上邻居：条件同时满足 右侧条件&&顶部条件
            _gridCells[i].neighborTopRight = (i + 1)%gridWidth!=0 && i+1 <= gridWidth * gridHeight && i+1<=gridWidth*gridHeight-gridWidth ? _gridCells[i + 1 + gridWidth] : null;
            // 网格右下邻居：条件同时满足 右侧条件&&底部条件
            _gridCells[i].neighborBottomRight = (i + 1)%gridWidth!=0 && i+1 <= gridWidth * gridHeight && i+1-gridWidth>0 ? _gridCells[i + 1 - gridWidth] : null;
            // 网格左上邻居：条件同时满足 左侧条件&&顶部条件
            _gridCells[i].neighborTopLeft = i!=0&&(i+1)%gridWidth!=1 && i+1<=gridWidth*gridHeight-gridWidth ? _gridCells[i - 1 + gridWidth] : null;
            // 网格左下邻居：条件同时满足 左侧条件&&底部条件
            _gridCells[i].neighborBottomLeft = i!=0&&(i+1)%gridWidth!=1 && i+1-gridWidth>0 ? _gridCells[i - 1 - gridWidth] : null;
        }
    }
    
}
