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
    public float spawnRadius = 80f;

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
        // 기준점을 중심으로 구의 범위 내에서 메테오가 스폰될 랜덤 위치를 지정하고자 한다.

        // 먼저 구 내의 랜덤 위치를 지정한다.
        Vector3 randSpherePos = Random.insideUnitSphere * spawnRadius; 

        // 랜덤 위치의 y가 항상 양수가 되도록 한다. (플레이어가 아래를 볼 수 없으므로)
        randSpherePos.y = (randSpherePos.y > 0) ? randSpherePos.y + 20 : randSpherePos.y * -1 + 20;

        // 만약 생성 위치와 플레이어 사이의 거리가 너무 가까우면, 거리를 2배로 늘린다.
        if (randSpherePos.x * randSpherePos.x + randSpherePos.y * randSpherePos.y + randSpherePos.z * randSpherePos.z < spawnRadius * spawnRadius / 4) {
            randSpherePos.x *= 2;
            randSpherePos.y *= 2;
            randSpherePos.z *= 2;
        }

        // 만약 생성 위치가 플레이어의 머리 위에 있으면, 거리를 2배로 늘린다. (플레이어가 위를 볼 수 없으므로)
        if (randSpherePos.x * randSpherePos.x + randSpherePos.z * randSpherePos.z < spawnRadius * spawnRadius * 3 / 4) {
            randSpherePos.x *= 2;
            randSpherePos.z *= 2;
        }

        // 실제 스폰 위치 = 기준 스폰 위치 + 상대적 랜덤 위치
        Vector3 randEnemyPos = meteorSpawnPosition.position + randSpherePos;

        // meteorFactory에서 메테오를 하나 선택해서 생성한다.
        // 생성 위치는 randEnemyPos, 회전 방향은 기본값이다.
        int rand = Random.Range(0, 4);
        GameObject enemy = Instantiate(meteorFactory[rand], randEnemyPos, Quaternion.identity);

        // 메테오가 구의 중심을 향해 날아가도록 한다. 
        enemy.transform.forward = meteorSpawnPosition.position - randEnemyPos;
    }
}
