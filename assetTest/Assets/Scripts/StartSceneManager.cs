using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour {

    public static float mouseSpeed = 5;
    public Button startButton;
    public Button rankButton;
    public Button senseButton;
    public Slider senseBar;
    public Button returnButton;
    public Button soloButton;
    public Button duoButton;
    public Button backButton;
    public GameObject StartUI;
    public GameObject SenseUI;
    public GameObject ModeUI;
    public GameObject Title;

    // Sound
    public AudioSource audioSource;
    public AudioClip buttonSound;


    private void Start() {
        audioSource.clip = buttonSound;
        // startButton = GetComponentInChildren<Button>();
        if (startButton != null) {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        // rankButton = GetComponentInChildren<Button>();
        if (rankButton != null) {
            rankButton.onClick.AddListener(OnRankButtonClick);
        }

        // senseButton = GetComponentInChildren<Button>();
        if (senseButton != null) {
            senseButton.onClick.AddListener(OnSenseButtonClick);
        }

        // returnButton = GetComponentInChildren<Button>();
        if (returnButton != null) {
            returnButton.onClick.AddListener(OnReturnButtonClick);
        }

        // soloButton = GetComponentInChildren<Button>();
        if (soloButton != null) {
            soloButton.onClick.AddListener(OnSoloButtonClick);
        }

        // duoButton = GetComponentInChildren<Button>();
        if (duoButton != null) {
            duoButton.onClick.AddListener(OnDuoButtonClick);
        }

        // backButton = GetComponentInChildren<Button>();
        if (backButton != null) {
            backButton.onClick.AddListener(OnBackButtonClick);
        }
        
        senseBar.value = mouseSpeed;
    }

    private void OnStartButtonClick() {
        StartUI.SetActive(false);
        ModeUI.SetActive(true);
        audioSource.Play();
    }

    private void OnRankButtonClick() {
        audioSource.Play();
        SceneManager.LoadScene("RankScene");
    }

    private void OnSenseButtonClick() {
        Title.SetActive(false);
        StartUI.SetActive(false);
        SenseUI.SetActive(true);
        audioSource.Play();
    }

    private void OnReturnButtonClick() {
        mouseSpeed = senseBar.value;
        Title.SetActive(true);
        SenseUI.SetActive(false);
        StartUI.SetActive(true);
        audioSource.Play();
    }

    private void OnSoloButtonClick() {
        audioSource.Play();
        SceneManager.LoadScene("GameScene");
    }

    private void OnDuoButtonClick() {
        audioSource.Play();
        SceneManager.LoadScene("RoomScene");
    }

    private void OnBackButtonClick() {
        ModeUI.SetActive(false);
        StartUI.SetActive(true);
        audioSource.Play();
    }
}