using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridEditor : MonoBehaviour
{
    public float drawHeightStep = 0.2f;
    public int drawHeight = 0;
    public int drawRadius = 1;

    public QuadGrid quadGrid;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setEditHeight()
    {
        var sliderHeight = GameObject.Find("GridEditorPanel/EditPanel/HeightSlider").GetComponent<Slider>().value;
        drawHeight = (int)sliderHeight;
        GameObject.Find("GridEditorPanel/EditPanel/HeightSlider/ValueLabelText").GetComponent<TMP_Text>().text = drawHeight.ToString();
    }
}
