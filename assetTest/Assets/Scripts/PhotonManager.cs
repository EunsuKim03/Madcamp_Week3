using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0"; // 이 게임의 버전
    private string userId = "Player A";

    public Button enterButton; // 로비에서 방에 입장
    public Button backButton; // 로비에서 메뉴로 이동
    public Button readyButton; // 방에서 레디
    public Button exitButton; // 방에서 나가서 로비로 이동
    public Button startButton; // 게임 시작
    public GameObject Lobby; // 로비 창
    public GameObject Room; // 룸 창 (대기실)
    public TextMeshProUGUI Player1Name; // 플레이어1 이름
    public GameObject ready1; // 플레이어1 준비 상태
    public TextMeshProUGUI Player2Name; // 플레이어2 이름
    public GameObject ready2; // 플레이어2 준비 상태
    

    private void Awake() 
    {
        // 같은 룸의 유저들에게 자동으로 씬을 로딩한다.
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리만 접속할 수 있다.
        PhotonNetwork.GameVersion = version;

        // 유저 아이디 할당
        PhotonNetwork.LocalPlayer.NickName = userId;

        // 포톤 서버와의 통신 속도 확인 (기본값: 초당 30회)
        Debug.Log(PhotonNetwork.SendRate);

        // 포톤 서버에 접속 (및 자동으로 로비에 입장)
        PhotonNetwork.ConnectUsingSettings();
    }


    // Start is called before the first frame update
    private void Start() 
    {
        // enterButton = GetComponentInChildren<Button>();
        if (enterButton != null) {
            enterButton.onClick.AddListener(OnEnterButtonClick);
        }

        // backButton = GetComponentInChildren<Button>();
        if (backButton != null) {
            backButton.onClick.AddListener(OnBackButtonClick);
        }

        // readyButton = GetComponentInChildren<Button>();
        if (readyButton != null) {
            readyButton.onClick.AddListener(OnReadyButtonClick);
        }

        // exitButton = GetComponentInChildren<Button>();
        if (exitButton != null) {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }

        // startButton = GetComponentInChildren<Button>();
        if (startButton != null) {
            startButton.onClick.AddListener(OnStartButtonClick);
        } 

        Player1Name.text = "";
        Player2Name.text = "";
    }

    private void OnEnterButtonClick() { // 로비 -> 방
        Lobby.SetActive(false);
        Room.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); // 랜덤 룸에 접속한다.
    }

    private void OnBackButtonClick() { // 로비 -> 메뉴
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect(); // 연결을 해제한다.
        SceneManager.LoadScene("StartScene");
    }

    private void OnReadyButtonClick() { // 방 -> 레디
        if (Player1Name.text == userId) { // 내가 첫 번째이다.
            if (ready1.activeSelf) ready1.SetActive(false); // 이미 레디를 했었으므로 해제한다.
            else ready1.SetActive(true);
        }
        else { // 내가 두 번째이다.
            if (ready2.activeSelf) ready2.SetActive(false); // 이미 레디를 했었으므로 해제한다.
            else ready2.SetActive(true);
        }

    }

    private void OnExitButtonClick() { // 방 -> 로비
        Lobby.SetActive(true);
        Room.SetActive(false);
        Player1Name.text = "";
        Player2Name.text = "";
        ready1.SetActive(false);
        ready2.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby(); // 로비에 입장한다.
    }

    private void OnStartButtonClick() { // 방 -> 게임 시작
        if (ready1.activeSelf && ready2.activeSelf) { // 두 플레이어가 모두 레디를 해야 한다.
            SceneManager.LoadScene("jiwoo");
            // 플레이어 생성
            float rand = Random.Range(-10f, 10f);
            PhotonNetwork.Instantiate("BananaMan", new Vector3(rand, 0, rand), Quaternion.identity);
        } 
    }


    // 포톤 서버에 접속하면 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        // PhotonNetwork.LocalPlayer.NickName = NickNameInput.text; (public InputField NickNameInput)
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); // false
        PhotonNetwork.JoinLobby(); // 로비에 입장한다.
    }

    // 로비에 입장하면 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); // true
    }

    // 만약 랜덤 룸 입장에 실패 시 호출되는 콜백 함수 (만약 현재 룸이 없어도 입장에 실패한다.)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Faild {returnCode}:{message}");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2; // 최대 접속자 수
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸 목록에 노출 여부

        // 룸 생성 및 입장?
        PhotonNetwork.CreateRoom(null, ro); // 방 이름이 null이면 서버가 방 이름을 자동으로 생성한다.
    }

    // 룸 생성이 성공적으로 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 롬에 입장하면 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}"); // true
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 룸에 접속한 사용자들의 정보 확인
        var player1 = PhotonNetwork.CurrentRoom.Players[1];
        // var player2 = PhotonNetwork.CurrentRoom.Players[2];
        Player1Name.text = "AAAA";
        // Player2Name.text = player2.NickName.ToString();
        
        foreach (var player in PhotonNetwork.CurrentRoom.Players) Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}"); // 닉네임, 고유id
    } 

    // 서버와의 연결이 끊겼을 때 호출되는 콜백 함수
    public override void OnDisconnected(DisconnectCause cause) 
    {
        Debug.Log("Disconnect");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
