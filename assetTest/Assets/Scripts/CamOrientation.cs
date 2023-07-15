using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOrientation : MonoBehaviour {
    float mouseSpeed = 10;
    float mouseZ = 0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        mouseZ += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseZ = Mathf.Clamp(mouseZ, -5.0f, 40.0f);
        transform.localEulerAngles = new Vector3(-mouseZ - 5, -90, 0);
        
    }
}
