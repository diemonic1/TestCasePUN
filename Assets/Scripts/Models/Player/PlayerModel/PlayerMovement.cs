using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PhotonView _view;
    
    private Vector3 _targetPosition;
    
    private const float Speed = 5f;

    public void Init()
    {
        _targetPosition = transform.position;
        
        PhotonNetworkService.Instance.OnOtherPlayerJoinGame -= SendPosition;
        PhotonNetworkService.Instance.OnOtherPlayerJoinGame += SendPosition;
    }
    
    private void Update()
    {
        if (_view.IsMine)
            HandleInput();

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * 10f);
    }

    public void SetPositionFromRemote(Vector3 newPosition)
    {
        _targetPosition = newPosition;
    }
    
    private void HandleInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f)
        {
            _targetPosition = transform.position + new Vector3(h, 0, v) * Speed * Time.deltaTime;
            transform.position = _targetPosition;

            SendPosition();
        }
    }

    private void SendPosition()
    {
        PlayersHandler.Instance.OnPlayerMoved(_targetPosition);
    }

    private void OnDestroy()
    {
        PhotonNetworkService.Instance.OnOtherPlayerJoinGame -= SendPosition;
    }
}
