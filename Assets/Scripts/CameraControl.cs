using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camControl();
    }

    public void camControl()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (camera.fieldOfView <= 100)
                camera.fieldOfView += 2;
            if (camera.orthographicSize <= 20)
                camera.orthographicSize += 0.5F;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (camera.fieldOfView > 2)
                camera.fieldOfView -= 2;
            if (camera.orthographicSize >= 1)
                camera.orthographicSize -= 0.5F;
        }
    }
}
