using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour {
    public Button startButton;

    private void Start() {
        startButton = GetComponentInChildren<Button>();

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
    }

    private void OnStartButtonClick() {
        SceneManager.LoadScene("SampleScene");
    }
}