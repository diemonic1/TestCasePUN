using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DefaultNamespace
{
    public class PhotonEventLogger : MonoBehaviour, IOnEventCallback
    {
        void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
        void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

        private void Awake()
        {
            if (Logger.Instance.PhotonStandartMessagesLog)
            {
                PhotonNetwork.NetworkingClient.LoadBalancingPeer.DebugOut = DebugLevel.ALL;
                PhotonNetwork.LogLevel = PunLogLevel.Full;
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            Logger.Instance.DebugMessageShow(
                Logger.ELogSource.PhotonStandartMessages, 
                $"[Photon Event] Code={photonEvent.Code} Sender={photonEvent.Sender} CustomData={photonEvent.CustomData}",
                Logger.ELogColor.Blue
                );
        }
    }
}