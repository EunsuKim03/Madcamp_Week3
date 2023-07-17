using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScan : MonoBehaviour
{
    private LineRenderer line;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        line.transform.position = player.position;
        line.SetPosition(0, player.position + player.forward * 100);
    }
}
