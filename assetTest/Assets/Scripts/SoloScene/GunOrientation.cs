using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOrientation : MonoBehaviour {
    float mouseSpeed = StartSceneManager.mouseSpeed;
    float mouseZ = 0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        mouseZ += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseZ = Mathf.Clamp(mouseZ, -10f, 40.0f);
        transform.localEulerAngles = new Vector3(0, 0, -10 - mouseZ);
        
    }
}
