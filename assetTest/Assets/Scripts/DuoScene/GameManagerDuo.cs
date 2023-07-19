using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;

public class GameManagerDuo : MonoBehaviourPunCallbacks
{
    public static float timeSurvived; // 버틴 시간
    public static int gameScore; // 게임 점수
    public static int bulletCount; // 남은 총알 개수
    public static bool isGameOver; // 게임 종료 여부
    int gameOverCount;
    public static bool isReloading; // 현재 장전중인지 아닌지

    public static bool isMeteorSpawn = true; // 메테오를 스폰할 것인지 아닌지

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

    // DuoData
    public DuoObject duoObject;
    public UrlObject URL;

    // Sound
    public AudioSource bgmSource;
    public AudioSource buttonSource;



    // Start is called before the first frame update
    void Start()
    {   
        // 플레이어를 생성한다.
        int pos = PhotonNetwork.IsMasterClient ? 3 : -3;
        PhotonNetwork.Instantiate("BananaManDuo", new Vector3(pos, 0, pos), Quaternion.identity);

        Time.timeScale = 1f;
        timeSurvived = 0;
        gameScore = 0;
        bulletCount = 30;
        isGameOver = false;
        isReloading = false;

        exitButton.onClick.AddListener(OnClickExitButton);
        restartButton.onClick.AddListener(OnClickRestartButton);

        subCamObj.SetActive(false);
        pauseUI.SetActive(false);
        player.SetActive(true);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCam;

        gameOverCount = 0;
        bgmSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timeSurvived += Time.deltaTime;
        if (bulletCount < 0) {
            bulletCount = 30;
        }

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

        if (isGameOver && gameOverCount == 0) {
            GameOver();
            gameOverCount = 1;
        }
    }

    void OnClickExitButton() {
        //Application.Quit();
        buttonSource.Play();
        Time.timeScale = 1f;
        PhotonNetwork.LeaveRoom();
    }

    void GameOver() {
        bgmSource.Stop();
        Debug.Log(PhotonNetwork.NetworkClientState);
        overUI.SetActive(true);
        subCamObj.SetActive(true);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = subCam;

        if (gameScore > duoObject.duoScore) {
            duoObject.duoScore = gameScore;

            var url = string.Format("{0}/{1}", URL.host, URL.urlUpdateDuo);

            var req = new Protocols.Packets.req_UpdateDuo();
            req.id1 = duoObject.id1;
            req.id2 = duoObject.id2;
            req.newScore = gameScore;
            var json = JsonConvert.SerializeObject(req);
            Debug.Log(json);

            StartCoroutine(RankMain.UpdateDuo(url, json, (raw) =>
            {
                Debug.LogFormat("UPDATED {0} & {1} -> {2}", duoObject.id1, duoObject.id2, gameScore);
            }));

        }

        player.SetActive(false);
        Time.timeScale = 0f;
    }

    void OnClickRestartButton() {
        buttonSource.Play();
        Time.timeScale = 1f;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        Debug.Log("정상적으로 게임에서 나왔다.");
        SceneManager.LoadScene("StartScene");
    }
}