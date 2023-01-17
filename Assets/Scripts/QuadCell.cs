using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuadCell : MonoBehaviour
{
    public float gridCellHeight = 0;
    // 声明当前网格的上下左右8个方向的相邻网格，在网格地图初始化时被赋值
    public QuadCell neighborTop;
    public QuadCell neighborBottom;
    public QuadCell neighborLeft;
    public QuadCell neighborRight;
    public QuadCell neighborTopLeft;
    public QuadCell neighborTopRight;
    public QuadCell neighborBottomRight;
    public QuadCell neighborBottomLeft;

    private GameObject gridEditorPanel;
    
    // 声明四边形网格的方向
    public enum QuadDirections
    {
        Top, Bottom, Left, Right, TopLeft, BottomLeft, TopRight, BottomRight
    }

}
