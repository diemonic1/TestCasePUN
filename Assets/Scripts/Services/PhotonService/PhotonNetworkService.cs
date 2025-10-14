using System;
using DefaultNamespace;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using Logger = DefaultNamespace.Logger;
using Player = Photon.Realtime.Player;

public class PhotonNetworkService : MonoBehaviourPunCallbacks
{
    public static PhotonNetworkService Instance => _instance ??= FindObjectOfType<PhotonNetworkService>();
    private static PhotonNetworkService _instance;
    
    public Action OnMyPlayerJoinGame;
    public Action OnOtherPlayerJoinGame;
    
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Logger.Instance.DebugMessageShow(Logger.ELogSource.PhotonNetworkManager, "Подключился к мастер-серверу");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }
    
    public override void OnJoinedRoom()
    {
        Logger.Instance.DebugMessageShow(Logger.ELogSource.PhotonNetworkManager, "Подключился к комнате");
        OnMyPlayerJoinGame?.Invoke();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnOtherPlayerJoinGame?.Invoke();
    }
}
