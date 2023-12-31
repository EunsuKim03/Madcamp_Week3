using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasUI : MonoBehaviour
{
    // 캔버스 화면에 표시되는 3개의 텍스트
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bulletCountText;
    public TextMeshProUGUI bulletFullText;

    public Image timerBar;
    public Image bulletBar;
    public Image reloadingBar;

    float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletFullText.text = "/" + GameManager.bulletCountLimit.ToString();
        reloadingBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ((int)GameManager.timeSurvived).ToString();
        scoreText.text = GameManager.gameScore.ToString();
        bulletCountText.text = GameManager.bulletCount.ToString();
    
        timerBar.fillAmount = GameManager.timeSurvived / GameManager.limitTimer;

        Vector2 newBulletBarSize = new Vector2(300 * ((float)GameManager.bulletCount / GameManager.bulletCountLimit), 10);
        bulletBar.rectTransform.sizeDelta = newBulletBarSize;
        // timerBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);

        // 장전하는 중이다.
        if (GameManager.isReloading) {
            // 장전하기 시작하면 reloadingBar을 보이게 한다.
            if (reloadTime == 0) {
                reloadingBar.fillAmount = 1;
            }
            // 시간이 지나면서 reloadingBar의 길이가 줄어든다.
            reloadTime += Time.deltaTime;
            reloadingBar.fillAmount = 1 - (reloadTime / GameManager.reloadTimeLimit);

            // 만약 장전이 끝나면, 탄창을 다 채우고 시간을 초기화한다.
            if (reloadTime >= GameManager.reloadTimeLimit) {
                GameManager.bulletCount = GameManager.bulletCountLimit;
                reloadTime = 0f;
                GameManager.isReloading = false;
            }
        }
    }
}