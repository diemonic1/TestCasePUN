using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameSceneBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            PhotonNetworkService.Instance.Connect();
        }
    }
}