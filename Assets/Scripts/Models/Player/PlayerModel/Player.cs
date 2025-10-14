using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PlayerMovement))]
    
    public class Player : MonoBehaviour, IPunInstantiateMagicCallback
    {
        [SerializeField] private PlayerMovement _playerMovement;
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            PlayersHandler.Instance.OnPlayerSpawned(this, info.Sender.ActorNumber);
            Init();
        }
        
        private void Init()
        {
            _playerMovement.Init();
        }

        public void SetPositionFromRemote(Vector3 newPosition)
        {
            _playerMovement.SetPositionFromRemote(newPosition);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
#endif
    }
}