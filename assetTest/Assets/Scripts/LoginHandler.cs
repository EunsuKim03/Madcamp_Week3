using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class LoginHandller : MonoBehaviour {

    // Title
    public GameObject title;

    // Data
    public UrlObject URL;
    public UserObject User;

    // Menu
    public Button moveToLogin;
    public Button moveToRegister;
    public GameObject menuUI;
    public GameObject loginUI;
    public GameObject registerUI;
    
    // Login
    public TextMeshProUGUI loginId;
    public TextMeshProUGUI loginPw;
    public TextMeshProUGUI loginResultText;
    public Button loginDone;
    public Button loginBack;

    // Register
    public TextMeshProUGUI registerId;
    public TextMeshProUGUI registerPw;
    public TextMeshProUGUI registerResultText;
    public Button registerDone;
    public Button registerBack;

    void Awake() {
        User.id = "";
        User.solo = 0;
    }
    void Start() {
        moveToLogin.onClick.AddListener(OnMoveToLoginClicked);
        moveToRegister.onClick.AddListener(OnMoveToRegisterClicked);
        loginDone.onClick.AddListener(OnLoginButtonClicked);
        registerDone.onClick.AddListener(OnRegisterButtonClicked);
        loginBack.onClick.AddListener(OnBackClicked);
        registerBack.onClick.AddListener(OnBackClicked);
    }

    // Menu Functions
    void OnMoveToRegisterClicked() {
        title.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        menuUI.SetActive(false);
    }

    void OnMoveToLoginClicked() {
        title.SetActive(false);
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        menuUI.SetActive(false);
    }

    void OnBackClicked() {
        title.SetActive(true);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuUI.SetActive(true);
    }
    


    // Login Functions
    void OnLoginButtonClicked()
    {
        string id = loginId.text;
        id = id.Substring(0, id.Length - 1);
        string password = loginPw.text;
        password = password.Substring(0, password.Length - 1);

        PerformLogin(id, password);   
        Invoke("delay", 1.0f);
    }

    void PerformLogin(string id, string password) {
        var url = string.Format("{0}/{1}", URL.host, URL.urlLogin);
        Debug.Log(url);

        var req = new Protocols.Packets.req_Login();
        req.id = id;
        req.pw = password;
        var json = JsonConvert.SerializeObject(req);
        Debug.Log(json);

        // Invoke("delay", 1.0f);
        StartCoroutine(RankMain.PostByIdPw(url, json, (raw) =>
        {
            SoloData res = JsonConvert.DeserializeObject<SoloData>(raw);
            if (res != null) {
                Debug.LogFormat("Login Succeed: {0} : {1}", res.id, res.solo);
                User.id = res.id;
                User.solo = res.solo;
                PlayerPrefs.SetString("id", res.id);
                PlayerPrefs.SetInt("solo", res.solo);
                PlayerPrefs.Save();
            }
            // UnityEditor.EditorUtility.SetDirty(User);
            // UnityEditor.AssetDatabase.SaveAssets();

            if (User.id == id) {
                loginResultText.text = "Login successed!";

                SceneManager.LoadScene("StartScene");
            }
            else {
                loginResultText.text = "Login failed!\nPlease check your ID and password.";
            }
        }));
    }

    // Register Functions
    void OnRegisterButtonClicked()
    {
        string id = registerId.text;
        id = id.Substring(0, id.Length - 1);
        string password = registerPw.text;
        password = password.Substring(0, password.Length - 1);

        PerformRegister(id, password);
        Invoke("delay", 1.0f);
    }

    void PerformRegister(string id, string password) {
        var url = string.Format("{0}/{1}", URL.host, URL.urlRegister);

        var req = new Protocols.Packets.req_Register();
        req.id = id;
        req.pw = password;
        var json = JsonConvert.SerializeObject(req);
        Debug.Log(json);


        StartCoroutine(RankMain.InsertUser(url, json, (raw) =>
        {
            Protocols.Packets.res_Register res = JsonConvert.DeserializeObject<Protocols.Packets.res_Register>(raw);
            if (res == null || !res.result) {
                registerResultText.text = "That Id already exists";
                Debug.LogFormat("Register failed");
            } else {
                registerResultText.text = "Register successed!";
                Debug.LogFormat("Register succeed");
                User.id = id;
                User.solo = 0;

                PlayerPrefs.SetString("id", id);
                PlayerPrefs.SetInt("solo", 0);
                PlayerPrefs.Save();

                SceneManager.LoadScene("StartScene");
            }
        }));


    }

    void delay() {
    }


}
