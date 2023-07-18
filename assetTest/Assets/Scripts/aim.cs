using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aim : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform myTransform;

    void Start()
    {
        // myTransform = GetComponent<Transform>();
        myTransform.localPosition = new Vector3(0.2f, 0f, 0f);
        Debug.Log("aim");
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.localPosition = new Vector3(0.2f, 0f, 0f);
    }
}
