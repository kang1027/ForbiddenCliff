using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject groupBG;
    public GameObject findRoom;
    public GameObject createRoom;
    public InputField nameInput;
    public InputField findRoomCode;
    public InputField creatrRoomCode;
    public Text stateText;
    public Button findRoomBtn;
    public Button creatrRoomBtn;

    private bool isInRoom;
    private string roomCode;
    private bool isLoadScene;
    private void Awake()
    {
        Screen.SetResolution(450, 800, false);
    }
    private void Update()
    {
        if (isInRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                if (!isLoadScene)
                {
                    StartCoroutine("StartGame");
                }
            }

        }
    }
    IEnumerator StartGame()
    {
        isLoadScene = true;
        for(int i = 3; i >=1; i--)
        {
            stateText.text = $"{roomCode} 방에서게임을 찾았습니다. \n{i}초 후 게임이 시작됩니다.";
            yield return new WaitForSeconds(1);
        }
        stateText.text = "게임을 시작하는 중입니다...";

        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
        PhotonNetwork.LoadLevel("MultiGame");
    }
    public void StartGroupGame()
    {
        AudioManager.isClick = true;
        groupBG.SetActive(true);
        findRoom.SetActive(false);
        createRoom.SetActive(false);
        findRoomBtn.interactable = true;
        creatrRoomBtn.interactable = true;
        Connection();
    }
    public void ShowFindRoom() { findRoom.SetActive(true); AudioManager.isClick = true; }
    public void ShowCreateRoom() { createRoom.SetActive(true); AudioManager.isClick = true; }
    public void ExitPanel(int panelNum) //  groupBG : 0   finRoom : 1   createRoom : 2
    {
        AudioManager.isClick = true;
        if (panelNum == 0)
        {
            groupBG.SetActive(false);
            findRoom.SetActive(false);
            createRoom.SetActive(false);
            PhotonNetwork.Disconnect();
        }
        else if (panelNum == 1) findRoom.SetActive(false);
        else if (panelNum == 2) createRoom.SetActive(false);
    }

    public void Connection()
    {
        findRoomBtn.interactable = false;
        creatrRoomBtn.interactable = false;
        stateText.text = "서버에 연결 중...";
        PhotonNetwork.ConnectUsingSettings();
        
    }
    public override void OnConnectedToMaster()
    {
        stateText.text = "로비에 연결 중...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        stateText.text = "로비에 연결되었습니다.";
        findRoomBtn.interactable = true;
        creatrRoomBtn.interactable = true;
    }
    public void FindRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void FindRoom()
    {
        stateText.text = $"{findRoomCode.text} 방을 찾고 있습니다.";
        PhotonNetwork.JoinRoom(findRoomCode.text);
    }
    public void CreateRoom()
    {
        stateText.text = $"{creatrRoomCode.text} 방을 만들고 있습니다.";
        PhotonNetwork.CreateRoom(creatrRoomCode.text, new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnJoinedRoom()
    {
        roomCode = PhotonNetwork.CurrentRoom.Name;
        isInRoom = true;
        stateText.text = $"{roomCode} 방에 연결되었습니다.";
        findRoom.SetActive(false);
        createRoom.SetActive(false);
        creatrRoomBtn.interactable = false;
        findRoomBtn.interactable = false;
        
        
    }

    public override void OnCreatedRoom()
    {
        stateText.text = $"{creatrRoomCode.text} 방을 생성했습니다.";
    }
    


    public override void OnDisconnected(DisconnectCause cause) => stateText.text = "인터넷 연결이 끊겼습니다.";
    public override void OnJoinRoomFailed(short returnCode, string message) => stateText.text = $"{findRoomCode.text} 방을 찾지 못했습니다.";
    public override void OnCreateRoomFailed(short returnCode, string message) => stateText.text = $"{creatrRoomCode.text} 방을 만들지 못했습니다.";
    public override void OnJoinRandomFailed(short returnCode, string message) => stateText.text = "현재 존재하는 방이 없습니다.";
}
