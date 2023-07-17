using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn : MonoBehaviour
{
    public GameObject meteorFactory1; // 메테오 종류 1
    public GameObject meteorFactory2; // 메테오 종류 2
    public GameObject meteorFactory3; // 메테오 종류 3
    public GameObject meteorFactory4; // 메테오 종류 4
    public GameObject[] meteorFactory;
    public Transform meteorSpawnPosition; // 메테오를 생성하여 위치시키는 기준 장소 (구 중심)

    // 메테오가 스폰되는 주기
    public float spawnPeriod = 2f;
    // 메테오가 스폰된 이후로 지난 시간
    float spawnTime;
    // meteorSpawnPosition을 중심으로 반지름 radius의 범위 내에서 메테오가 랜덤으로 스폰된다.
    public float spawnRadius = 30f;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0;
        meteorFactory = new GameObject[4] {meteorFactory1, meteorFactory2, meteorFactory3, meteorFactory4};
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;

        // 스폰 주기가 되면 메테오를 스폰하고 스폰 시간을 초기화한다.
        if (spawnTime >= spawnPeriod) {
            spawnEnemy();
            spawnTime = 0f;
        }
    }

    private void spawnEnemy() 
    {
        // 기준점을 중심으로 구의 범위 내에서 메테오가 스폰될 랜덤 위치를 지정한다.
        Vector3 randEnemyPos = meteorSpawnPosition.position + (Random.insideUnitSphere * spawnRadius);

        // meteorFactory에서 메테오를 하나 생성한다.
        // 생성 위치는 randEnemyPos, 회전 방향은 기본값이다.
        int rand = Random.Range(0, 4);
        GameObject enemy = Instantiate(meteorFactory[rand], randEnemyPos, Quaternion.identity);

        // 메테오가 구의 중심을 향해 날아가도록 한다.
        enemy.transform.forward = meteorSpawnPosition.position - randEnemyPos;
    }
}
