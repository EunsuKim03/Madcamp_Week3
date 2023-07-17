using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbody는 힘과 토크를 받아 오브젝트가 사실적으로 움직인다. (물리 속성 부여)
[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    float meteorLifeTime; // 적이 스폰된 이후부터 시간이 얼마나 지났는지를 나타낸다.
    public int meteorScore = 10; // 메테오 하나를 맞출 때의 점수이다.
    public float meteorSpeed = 200;
    public float meteorLifeTimeLimit = 20f; // 메테오의 수명이 끝나면 사라진다.

    // Start is called before the first frame update
    void Start()
    {
        meteorLifeTime = 0f;

        // AddForce: 오브젝트에 일정한 힘을 주어 이동시킨다.
        // transform.forward: 현재 객체가 바라보는 방향을 가리킨다. (world가 아닌 local)
        GetComponent<Rigidbody>().AddForce(transform.forward * meteorSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // 시간이 누적된다.
        meteorLifeTime += Time.deltaTime;

        // 스폰된 이후로 일정 시간이 지나면 적이 자동으로 사라진다.
        if (meteorLifeTime >= meteorLifeTimeLimit) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        // 만약 총알이 적을 맞추면, 점수가 올라가고 적이 사라진다.
        if (other.gameObject.CompareTag("Bullet")) {
            GameManager.gameScore += meteorScore;
            Debug.Log(GameManager.gameScore);
            Destroy(gameObject);
        }
    }

    
}
