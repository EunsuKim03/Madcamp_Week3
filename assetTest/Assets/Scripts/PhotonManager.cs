using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0"; // 이 게임의 버전
    private string userId = "Player A";
    
    private void Awake() {
        // 같은 룸의 유저들에게 자동으로 씬을 로딩한다.
        PhotonNetwork.AutomaticallySyncScene = true;
        // 같은 버전의 유저끼리만 접속할 수 있다.
        PhotonNetwork.GameVersion = version;
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;
        // 포톤 서버와의 통신 속도 확인 (기본값: 초당 30회)
        Debug.Log(PhotonNetwork.SendRate);
        // 포톤 서버에 접속
        PhotonNetwork.ConnectUsingSettings();
        // public void connectServer() {PhotonNetwork.ConnectUsingSettings();}
    }

    // 포톤 서버에 접속하면 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        // PhotonNetwork.LocalPlayer.NickName = NickNameInput.text; (public InputField NickNameInput)
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); // false
        PhotonNetwork.JoinLobby(); // 이제 로비에 입장한다.
    }

    // 로비에 입장하면 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); // true
        // 랜덤 룸에 접속한다.
        PhotonNetwork.JoinRandomRoom();
    }

    // 만약 랜덤 룸 입장에 실패 시 호출되는 콜백 함수 (룸이 없어도 입장에 실패한다.)
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

        // 플레이어 생성
        float rand = Random.Range(-10f, 10f);
        PhotonNetwork.Instantiate("BananaMan", new Vector3(rand, 0, rand), Quaternion.identity);

        // 룸에 접속한 사용자들의 정보 확인
        foreach (var player in PhotonNetwork.CurrentRoom.Players) {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}"); // 닉네임, 고유id
        }

        // DisconnectedPanel.SetActive(false); (public GameObject DisconnectedPanel)
    }

     void disconnect() {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) {
            PhotonNetwork.Disconnect();
        }
    } 
    
/*    public override void OnDisconnected(DisconnectCasue cause) {
        DisconnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
