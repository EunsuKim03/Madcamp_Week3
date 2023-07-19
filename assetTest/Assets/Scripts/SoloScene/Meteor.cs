using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbody는 힘과 토크를 받아 오브젝트가 사실적으로 움직인다. (물리 속성 부여)
[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    float meteorLifeTime; // 적이 스폰된 이후부터 시간이 얼마나 지났는지를 나타낸다.
    public float meteorSpeedMin = 200; // 메테오가 날라오는 속도
    public float meteorSpeedMax = 700;
    public float meteorScaleMin = 7; // 메테오 크기 (스케일)
    public float meteorScaleMax = 12;
    float meteorScale;
    public float meteorLifeTimeLimit = 30f; // 메테오의 수명이 끝나면 사라진다.
    public GameObject explosion; // 메테오 적중시 폭발
    public GameObject body; // 메테오 몸체
    bool isShot = false;

    public AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        meteorLifeTime = 0f;

        // AddForce: 오브젝트에 일정한 힘을 주어 이동시킨다.
        // transform.forward: 현재 객체가 바라보는 방향을 가리킨다. (world가 아닌 local)
        float meteorSpeed = Random.Range(meteorSpeedMin, meteorSpeedMax);
        GetComponent<Rigidbody>().AddForce(transform.forward * meteorSpeed);

        // 최초 생성시에는 크기가 없다. 시간이 지나면서 커질 것이다.
        transform.localScale = Vector3.zero;
        meteorScale = Random.Range(meteorScaleMin, meteorScaleMax); // 메테오 최종 크기
    }

    // Update is called once per frame
    void Update()
    {
        // 시간이 누적된다.
        meteorLifeTime += Time.deltaTime;

        // 5초 동안 메테오의 크기가 점점 커지고, 최종적으로 meteorScale이 된다.
        if (meteorLifeTime < 5f) {
            transform.localScale = Vector3.one * meteorScale * meteorLifeTime / 5;
        }

        // 스폰된 이후로 일정 시간이 지나면 적이 자동으로 사라진다.
        if (meteorLifeTime >= meteorLifeTimeLimit) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        // 만약 총알이 적을 맞추면, 점수가 올라가고 적이 사라진다.
        if (other.gameObject.CompareTag("Bullet"))
        {
            explosionSound.Play();
            int meteorScore = (int) (1000 / (meteorScale * meteorScale)); // 메테오의 크기가 클수록 점수가 작다.
            if (!isShot) GameManager.gameScore += meteorScore; // 점수 증가
            isShot = true;

            Destroy(other.gameObject); // 총알 삭제
            explosion.SetActive(true); // 폭발 발생
            Destroy(body); // 메테오 몸체 삭제
            Invoke("DisableExplosion", 1f); // 1초 뒤 폭발 삭제
            Debug.Log("shot");
        }

        // 만약 플레이어가 메테오에 맞으면, 게임 오버가 된다.
        else if (other.gameObject.CompareTag("Player") && !isShot)
        {
            GameManager.isGameOver = true;
            Debug.Log("Game Over");
            }        
    }

    void DisableExplosion() {
        explosion.SetActive(false); // 폭발 삭제
        Destroy(gameObject); // 메테오 자체 삭제
    }    
}

