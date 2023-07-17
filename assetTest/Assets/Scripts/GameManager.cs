using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float remainTime; // 남은 게임 시간
    public static int gameScore; // 게임 점수
    public static int bulletCount; // 남은 총알 개수
    public static bool isGameOver; // 게임 종료 여부
    public static bool isReloading; // 현재 장전중인지 아닌지

    public static float remainTimeLimit = 180f; // 게임 제한 시간 (언제 게임오버할 것인지)
    public static int bulletCountLimit = 30; // 최대 탄창 개수 (한 탄창 당 총알이 몇 발인지)
    public static float reloadTimeLimit = 2f; // 장전하는데 걸리는 시간 (장전 딜레이)

    // Start is called before the first frame update
    void Start()
    {
        remainTime = remainTimeLimit;
        gameScore = 0;
        bulletCount = 30;
        isGameOver = false;
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        remainTime -= Time.deltaTime;
        if (remainTime <= 0) {
            isGameOver = true;
        }
        if (bulletCount < 0) {
            bulletCount = 30;
        }
    }
}
