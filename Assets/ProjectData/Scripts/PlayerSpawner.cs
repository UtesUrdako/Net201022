using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    private void Start()
    {
        PhotonNetwork.Instantiate(_playerPrefab.name, transform.position, Quaternion.identity);
    }
}
