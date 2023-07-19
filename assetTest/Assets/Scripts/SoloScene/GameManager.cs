using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

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
    public GameObject newRecord; // 최고 점수 달성 시 뜨는 UI

    // User Data
    public UserObject userObject; // 유저 정보
    public UrlObject URL; // URL 정보

    // Sound
    public AudioSource bgmSource;
    public AudioSource buttonSource;
    // public AudioSource overSource;


    // Start is called before the first frame update
    void Start()
    {
        userObject.id = PlayerPrefs.GetString("id", "DefaultID");
        userObject.solo = PlayerPrefs.GetInt("solo", 0);

        Debug.Log(userObject.id);
        Time.timeScale = 1f;
        timeSurvived = 0;
        gameScore = 0;
        bulletCount = 30;
        isGameOver = false;
        isReloading = false;

        exitButton.onClick.AddListener(OnClickExitButton);
        restartButton.onClick.AddListener(OnClickRestartButton);

        bgmSource.Play();

    }

    // Update is called once per frame
    void Update()
    {
        timeSurvived += Time.deltaTime;

        // ESC 눌렀을 시 pause
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape)) {
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
        buttonSource.Play();
        SceneManager.LoadScene("StartScene");
    }

    void GameOver() {
        // overSource.Play();
        bgmSource.Stop();
        subCamObj.SetActive(true);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = subCam;
        overUI.SetActive(true);

        // 최고 점수 달성 시 서버 통해 DB에 업데이트
        if (gameScore > userObject.solo) {
            newRecord.SetActive(true);
            userObject.solo = gameScore;

            // 서버 통신
            var url = string.Format("{0}/{1}", URL.host, URL.urlUpdateSolo);
            Debug.Log(url);

            var req = new Protocols.Packets.req_UpdateSolo();
            req.id = userObject.id;
            req.newScore = gameScore;
            var json = JsonConvert.SerializeObject(req);
            Debug.Log(json);

            StartCoroutine(RankMain.UpdateSolo(url, json, (raw) => {
                // var res = JsonConvert.DeserializeObject<Protocols.Packets.res>(raw);
                Debug.LogFormat("UPDATED {0} -> {1}", req.id, gameScore);
            }));
        } else {
            // newRecord.SetActive(false);
        }
        player.SetActive(false);
        Time.timeScale = 0f;

    }

    void OnClickRestartButton() {
        buttonSource.Play();
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}