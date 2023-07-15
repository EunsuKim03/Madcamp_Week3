using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float mouseX = 0f;

    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        mouseX += Input.GetAxis("Mouse X") * 10;
        transform.eulerAngles = new Vector3(0, mouseX, 0);

        if (Input.GetMouseButtonDown(0)) {
            playerAnimator.SetBool("fire", true);
        }
        if (Input.GetMouseButtonUp(0)) {
            playerAnimator.SetBool("fire", false);
        }

    }

}
