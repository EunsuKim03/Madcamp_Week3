using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbody는 힘과 토크를 받아 오브젝트가 사실적으로 움직인다. (물리 속성 부여)
[RequireComponent(typeof(Rigidbody))]

// 총알이 생성되면 앞으로 날라간다.
public class Bullet : MonoBehaviour
{
    // 총알이 날라가는 속도
    public float bulletSpeed = 1000;
    float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        // AddForce: 오브젝트에 일정한 힘을 주어 이동시킨다.
        // transform.forward: 현재 객체가 바라보는 방향을 가리킨다. (world가 아닌 local)
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        lifeTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= 30) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        // 만약 총알이 벽이나 바닥에 닿으면, 1초 뒤에 이 총알을 삭제한다.
        if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject, 1);
        }
    }
}
