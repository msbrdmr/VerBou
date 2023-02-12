using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera playerFPSCamera;
    public float camRotationz;



    private void Update()
    {
        playerFPSCamera.transform.rotation = new Quaternion(playerFPSCamera.transform.rotation.x, playerFPSCamera.transform.rotation.y, 0, 0);
    }
}
