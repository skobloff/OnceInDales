using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseMove : MonoBehaviour
{
    public Text testText;
    public krjTerrainHelper terrainHelper;
    private Vector3 MousePos;
    Quaternion originalRotation;
    float cameraAngleX = 0;
    float cameraAngleY = 0;
    

    // Use this for initialization
    void Start ()
    {
        originalRotation = transform.rotation;
    }
	
	// Update is called once per frame

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            cameraAngleX += 2.0F * Input.GetAxis("Mouse X");
            cameraAngleY += 2.0F * Input.GetAxis("Mouse Y");
            if (cameraAngleY > 0)
                cameraAngleY = 0;
            if (cameraAngleY < -90)
                cameraAngleY = -90;
            

            Quaternion rotationX = Quaternion.AngleAxis(cameraAngleX, Vector3.up);
            Quaternion rotationY = Quaternion.AngleAxis(cameraAngleY, Vector3.left); 

            transform.rotation = originalRotation * rotationX * rotationY;
        }
        float CameraMove = 2.0F * Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y = pos.y + CameraMove * Mathf.Sin(cameraAngleY / 180 * Mathf.PI);
        if (pos.y < 1)
            pos.y = 1;
        float Horizontal = CameraMove * Mathf.Cos(cameraAngleY / 180 * Mathf.PI);
        pos.x = pos.x + CameraMove * Mathf.Sin(cameraAngleX / 180 * Mathf.PI);
        pos.z = pos.z + CameraMove * Mathf.Cos(cameraAngleX / 180 * Mathf.PI);
        transform.position = pos;
        if(Input.GetKeyDown("f5"))
        {
            terrainHelper.reupdate();
        }
    }

}
