using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour {
    public Button startButton;
    public Button rankButton;
    public Button soloButton;
    public Button duoButton;
    public Button backButton;
    public GameObject StartUI;
    public GameObject ModeUI;


    private void Start() {
        // startButton = GetComponentInChildren<Button>();
        if (startButton != null) {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        // rankButton = GetComponentInChildren<Button>();
        if (rankButton != null) {
            rankButton.onClick.AddListener(OnRankButtonClick);
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
        
    }

    private void OnStartButtonClick() {
        StartUI.SetActive(false);
        ModeUI.SetActive(true);
    }

    private void OnRankButtonClick() {
        
    }

    private void OnSoloButtonClick() {
        SceneManager.LoadScene("GameScene");
    }

    private void OnDuoButtonClick() {
        
    }

    private void OnBackButtonClick() {
        ModeUI.SetActive(false);
        StartUI.SetActive(true);
    }
}