using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float zoomMod;

    public float minZoomLimit;
    public float maxZoomLimit;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize /= zoomMod;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize *= zoomMod;
        }

        if (Camera.main.orthographicSize < minZoomLimit)
        {
            Camera.main.orthographicSize = minZoomLimit;
        }

        if (Camera.main.orthographicSize > maxZoomLimit)
        {
            Camera.main.orthographicSize = maxZoomLimit;
        }
    }
}