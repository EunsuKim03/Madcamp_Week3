using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float timeSurvived; // 버틴 시간
    public static int gameScore; // 게임 점수
    public static int bulletCount; // 남은 총알 개수
    public static bool isGameOver; // 게임 종료 여부
    public static bool isReloading; // 현재 장전중인지 아닌지

    public static float limitTimer = 180f; // 타이머 UI가 꽉찰 때의 값
    public static int bulletCountLimit = 30; // 최대 탄창 개수 (한 탄창 당 총알이 몇 발인지)
    public static float reloadTimeLimit = 2f; // 장전하는데 걸리는 시간 (장전 딜레이)

    // UI
    public GameObject pauseUI; // 일시정지 UI
    public GameObject player; // 플레이어 
    public Camera mainCam; // 메인 캠
    public Camera subCam; // 서브 캠
    public GameObject subCamObj; // 서브 캠 오브젝트
    public Canvas canvas; // 캔버스
    public Button exitButton; // 나가기 버튼
    public GameObject overUI; // 게임오버 UI
    public Button restartButton; // 재시작 버튼



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        timeSurvived = 0;
        gameScore = 0;
        bulletCount = 30;
        isGameOver = false;
        isReloading = false;

        exitButton.onClick.AddListener(OnClickExitButton);
        restartButton.onClick.AddListener(OnClickRestartButton);
    }

    // Update is called once per frame
    void Update()
    {
        timeSurvived += Time.deltaTime;

        // ESC 눌렀을 시 pause
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseUI.activeSelf) {
                subCamObj.SetActive(false);
                pauseUI.SetActive(false);
                player.SetActive(true);
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = mainCam;
                Time.timeScale = 1f;
            } else {
                subCamObj.SetActive(true);
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = subCam;
                pauseUI.SetActive(true);
                player.SetActive(false);
                Time.timeScale = 0f;
            }
        }

        if (isGameOver) {
            GameOver();
        }
    }

    void OnClickExitButton() {
        SceneManager.LoadScene("StartScene");
    }

    void GameOver() {
        subCamObj.SetActive(true);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = subCam;
        overUI.SetActive(true);
        player.SetActive(false);
        Time.timeScale = 0f;
    }

    void OnClickRestartButton() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}