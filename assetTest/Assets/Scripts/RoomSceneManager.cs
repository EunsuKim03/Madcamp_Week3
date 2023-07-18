using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomSceneManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0"; // 이 게임의 버전
    private string userId;

    public Button enterButton; // 로비에서 방에 입장
    public Button backButton; // 로비에서 메뉴로 이동
    public Button readyButton; // 방에서 레디
    public Button exitButton; // 방에서 나가서 로비로 이동
    public Button startButton; // 게임 시작
    public GameObject Lobby; // 로비 창
    public GameObject Room; // 룸 창 (대기실)
    public GameObject loading; // 로딩 중
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
        PhotonNetwork.SendRate = 60;

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

        // 로비에 성공적으로 입장하기 전까지는 버튼을 비활성화한다.
        loading.SetActive(true);
        enterButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        userId = Random.Range(0, 100).ToString();
        PhotonNetwork.LocalPlayer.NickName = userId;
    }

    private void OnEnterButtonClick() { // 로비 -> 방
        PhotonNetwork.JoinRandomRoom(); // 랜덤 룸에 접속한다.

        // 룸 접속에 실패할 수 있으므로, OnJoinedRoom()에서 나머지를 처리한다.
    }

    private void OnBackButtonClick() { // 로비 -> 메뉴
        PhotonNetwork.LeaveLobby();
    }

    private void OnReadyButtonClick() { // 방 -> 레디
        if (Player1Name.text == userId) { // 내가 첫 번째이다.
            if (ready1.activeSelf) {
                ready1.SetActive(false); // 이미 레디를 했었으므로 해제한다.
                photonView.RPC("SetReadyState", RpcTarget.All, 1, false);
            }
            else {
                ready1.SetActive(true);
                photonView.RPC("SetReadyState", RpcTarget.All, 1, true);
            }
        }
        else { // 내가 두 번째이다.
            if (ready2.activeSelf) {
                ready2.SetActive(false); // 이미 레디를 했었으므로 해제한다.
                photonView.RPC("SetReadyState", RpcTarget.All, 2, false);
            }
            else {
                ready2.SetActive(true);
                photonView.RPC("SetReadyState", RpcTarget.All, 2, true);
            }
        }

    }

    private void OnExitButtonClick() { // 방 -> 로비
        PhotonNetwork.LeaveRoom();
    }

    private void OnStartButtonClick() { // 방 -> 게임 시작
        if (true) {//PhotonNetwork.IsMasterClient && ready1.activeSelf && ready2.activeSelf) { // 내가 방장이고, 두 플레이어가 모두 레디를 해야 한다.
            photonView.RPC("StartGame", RpcTarget.All);
        } 
    }


    // 포톤 서버에 접속하면 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        // PhotonNetwork.LocalPlayer.NickName = NickNameInput.text; (public InputField NickNameInput)
        Debug.Log("서버에 접속했다.");
        PhotonNetwork.JoinLobby(); // 로비에 입장한다.
    }

    // 로비에 입장하면 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"로비에 들어왔다. = {PhotonNetwork.InLobby}"); // true

        // 로비에 성공적으로 입장하면, 버튼을 활성화한다.
        loading.SetActive(false);
        enterButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    // 만약 랜덤 룸 입장에 실패 시 호출되는 콜백 함수 (만약 현재 룸이 없어도 입장에 실패한다.)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 Room 입장에 실패했다. {returnCode}:{message}");

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
        Debug.Log("Room을 새로 생성했다.");
        Debug.Log($"Room 이름 = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 롬에 입장하면 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"Room에 들어왔다 = {PhotonNetwork.InRoom}"); // true

        // 룸에 접속한 사용자들의 정보 확인
        if (PhotonNetwork.PlayerListOthers.Length == 1) { // 이미 다른 사람이 있으므로, 나는 방장이 아닌 참가자이다.
            Player1Name.text = PhotonNetwork.PlayerListOthers[0].NickName.ToString();
            Player2Name.text = userId;
            Player2Name.color = Color.yellow;
        }
        else { // 내가 방장이다.
            Player1Name.text = userId;
            Player1Name.color = Color.yellow;
        }
        Lobby.SetActive(false);
        Room.SetActive(true);
        
        foreach (var player in PhotonNetwork.CurrentRoom.Players) Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}"); // 닉네임, 고유id
    } 

    // 다른 플레이어가 방에 들어왔을 때 호출되는 콜백 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        Debug.Log("새로운 플레이어가 방에 들어왔다: " + newPlayer.NickName);
        if (Player1Name.text == "") {
            Player1Name.text = newPlayer.NickName;
            photonView.RPC("SetReadyState", RpcTarget.All, 2, ready2.activeSelf);
        }
        else {
            Player2Name.text = newPlayer.NickName;
            photonView.RPC("SetReadyState", RpcTarget.All, 1, ready1.activeSelf);
        }
    }

    // 다른 플레이어가 방에서 나갔을 때 호출되는 콜백 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (Player1Name.text == otherPlayer.NickName) { // 이제 내가 방장이 된다.
            Player1Name.text = userId;
            Player1Name.color = Color.yellow;
            ready1.SetActive(ready2.activeSelf);

            Player2Name.text = "";
            Player2Name.color = Color.white;
            ready2.SetActive(false);
            Debug.Log("플레이어 1이 방에서 나갔다: " + otherPlayer.NickName);
        }
        else {
            Player2Name.text = "";
            ready2.SetActive(false);
            Debug.Log("플레이어 2가 방에서 나갔다: " + otherPlayer.NickName);
        }
        
    }

    // 내가 방에서 나갔을 때 호출되는 콜백 함수
    public override void OnLeftRoom()
    {
        Debug.Log("방에서 나갔다.");
        Player1Name.text = "";
        Player2Name.text = "";
        Player1Name.color = Color.white;
        Player2Name.color = Color.white;
        ready1.SetActive(false);
        ready2.SetActive(false);
        Room.SetActive(false);
        Lobby.SetActive(true);
        PhotonNetwork.JoinLobby(); // 로비에 입장한다.
    }

    // 내가 로비에서 나갔을 때 호출되는 콜백 함수
    public override void OnLeftLobby()
    {
        Debug.Log("로비에서 나갔다.");
        PhotonNetwork.Disconnect(); // 연결을 해제한다.
    }

    // 서버와의 연결이 끊겼을 때 호출되는 콜백 함수
    public override void OnDisconnected(DisconnectCause cause) 
    {
        Debug.Log("서버 연결 종료");
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    private void SetReadyState(int index, bool readyState) {
        if (index == 1) ready1.SetActive(readyState);
        else ready2.SetActive(readyState);
    }

    [PunRPC]
    private void StartGame() {
        SceneManager.LoadScene("jiwoo");
    }
}
