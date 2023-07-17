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

    float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletFullText.text = "/" + GameManager.bulletCountLimit.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ((int)GameManager.remainTime).ToString();
        scoreText.text = GameManager.gameScore.ToString();
        bulletCountText.text = GameManager.bulletCount.ToString();
    
        timerBar.fillAmount = GameManager.remainTime / GameManager.remainTimeLimit;

        Vector2 newBulletBarSize = new Vector2(300 * ((float)GameManager.bulletCount / GameManager.bulletCountLimit), 10);
        bulletBar.rectTransform.sizeDelta = newBulletBarSize;
        // timerBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);

        if (GameManager.isReloading) {
            reloadTime += Time.deltaTime;
            if (reloadTime >= GameManager.reloadTimeLimit) {
                GameManager.bulletCount = GameManager.bulletCountLimit;
                reloadTime = 0f;
                GameManager.isReloading = false;
            }
        }
    }
}
