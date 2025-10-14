using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayersHandler : MonoBehaviour, IOnEventCallback
    {
        public static PlayersHandler Instance => _instance ??= FindObjectOfType<PlayersHandler>();
        private static PlayersHandler _instance;
        
        [SerializeField] private Player _playerPrefab;
        
        private Dictionary<int, Player> _players = new();

        private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
        private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

        private void Awake()
        {
            PhotonNetworkService.Instance.OnMyPlayerJoinGame -= SpawnPlayer;
            PhotonNetworkService.Instance.OnMyPlayerJoinGame += SpawnPlayer;
        }

        public void OnPlayerSpawned(Player newPlayer, int actorNumber)
        {
            _players.Add(actorNumber, newPlayer);
        }

        private void SpawnPlayer()
        {
            Vector3 spawnPos = new Vector3(0, 1, 0);
            
            PhotonNetwork.Instantiate(
                _playerPrefab.name, 
                spawnPos, 
                Quaternion.identity
            );
        }

        public void OnPlayerMoved(Vector3 newPosition)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

            PhotonNetwork.RaiseEvent(PhotonEventCodes.MOVE_EVENT_CODE, newPosition, options, SendOptions.SendReliable);
        }
        
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == PhotonEventCodes.MOVE_EVENT_CODE
                && _players.ContainsKey(photonEvent.Sender))
            {
                _players[photonEvent.Sender].SetPositionFromRemote((Vector3)photonEvent.CustomData);
            }
        }
    }
}