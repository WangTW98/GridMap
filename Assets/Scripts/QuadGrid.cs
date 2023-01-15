using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGrid : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;

    public GridCell gridCell;

    public List<GridCell> _gridCells;

    public void Awake()
    {
        createGridMap(gridWidth, gridHeight);
        setGridCellNeighbors();
    }

    public void createGridMap(int width, int height)
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GridCell cell = Instantiate(gridCell);
                cell.transform.parent = transform;
                cell.transform.position = new Vector3(j, 0, i);
                _gridCells.Add(cell);
            }
        }
    }

    public void setGridCellNeighbors()
    {
        for (int i = 0; i < _gridCells.Count; i++)
        {
            _gridCells[i].neighborLeft = i!=0&&(i+1)%gridWidth!=1 ?  _gridCells[i - 1] : null;
            _gridCells[i].neighborRight = (i + 1)%gridWidth!=0 && i+1 <= gridWidth * gridHeight ? _gridCells[i + 1] : null;
            _gridCells[i].neighborTop = i+1<=gridWidth*gridHeight-gridWidth ? _gridCells[i + gridWidth] : null;
            _gridCells[i].neighborBottom = i+1-gridWidth>0 ? _gridCells[i - gridWidth] : null;
            _gridCells[i].neighborTopRight = (i + 1)%gridWidth!=0 && i+1 <= gridWidth * gridHeight && i+1<=gridWidth*gridHeight-gridWidth ? _gridCells[i + 1 + gridWidth] : null;
            _gridCells[i].neighborBottomRight = (i + 1)%gridWidth!=0 && i+1 <= gridWidth * gridHeight && i+1-gridWidth>0 ? _gridCells[i + 1 - gridWidth] : null;
            _gridCells[i].neighborTopLeft = i!=0&&(i+1)%gridWidth!=1 && i+1<=gridWidth*gridHeight-gridWidth ? _gridCells[i - 1 + gridWidth] : null;
            _gridCells[i].neighborBottomLeft = i!=0&&(i+1)%gridWidth!=1 && i+1-gridWidth>0 ? _gridCells[i - 1 - gridWidth] : null;
        }
    }
    
}
