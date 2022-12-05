using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 0.02f;


    // Update is called once per frame
    void Update()
    {
        float z = cameraSpeed * Input.GetAxis("Vertical");
        float x = cameraSpeed * Input.GetAxis("Horizontal");

        if (Input.GetKey("left shift")){

            float cameraSpeed_Ultra = 1.5f;

            z = cameraSpeed_Ultra * Input.GetAxis("Vertical");
            x = cameraSpeed_Ultra * Input.GetAxis("Horizontal");

            transform.Translate(0, 0, z);
            transform.Rotate(0, 0, x);
        }

        else if (Input.GetKey("r")){

            transform.Translate(0, 15 ,0);
            transform.Rotate(0,0,0);
        }

        else{
            transform.Translate(x, z, 0);
        }
    }

    
}
