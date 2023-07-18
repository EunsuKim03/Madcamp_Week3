using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;

public class RankServer : MonoBehaviour {
    public UrlObject URL;

    // SoloData[] soloDatas = new SoloData[9];
    // DuoData[] duoDatas = new DuoData[9];

    public TextMeshProUGUI[] soloTexts;

    public TextMeshProUGUI[] duoTexts;

    // Start is called before the first frame update
    void Start() {
        LoadRank();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void LoadRank() {
        LoadSoloRank();
        Invoke("LoadDuoRank", 1f);
    }

    void LoadSoloRank() {
        var url = string.Format("{0}/{1}", URL.host, URL.urlGetAll_Id);

        StartCoroutine(RankMain.GetRank_Id(url, (raw) =>
        {
            SoloData[] res = JsonConvert.DeserializeObject<SoloData[]>(raw);
        
            Debug.LogFormat("GetAll_Id Result:\n");

            Array.Sort(res, (x, y) => y.solo.CompareTo(x.solo));

            int i = 0;
            foreach (SoloData user in res)
            {
                if (i >= 9) {
                    break;
                }
                Debug.LogFormat("{0} : {1}", user.id, user.solo);

                // soloDatas[i] = new SoloData();
                // soloDatas[i].id = user.id;
                // soloDatas[i].solo = user.solo;

                soloTexts[i].text = string.Format("{0}. {1}: {2}", i+1, user.id, user.solo);

                i++;
            }
        }));

    }

    void LoadDuoRank() {
        var url = string.Format("{0}/{1}", URL.host, URL.urlGetAll_Duo);

        StartCoroutine(RankMain.GetRank_Duo(url, (raw) =>
        {
            DuoData[] res = JsonConvert.DeserializeObject<DuoData[]>(raw);
        
            Debug.LogFormat("GetAll_Duo Result:\n");

            Array.Sort(res, (x, y) => y.duoScore.CompareTo(x.duoScore));

            int i = 0;
            foreach (DuoData d in res)
            {
                if (i >= 9) {
                    break;
                }
                Debug.LogFormat("{0}. {1} & {2} : {3}", i+1, d.id1, d.id2, d.duoScore);

                duoTexts[i].text = string.Format("{0}. {1} & {2} : {3}", i+1, d.id1, d.id2, d.duoScore);

                i++;
            }
        }));
    }
}
