using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Unity.VisualScripting;
using Photon.Realtime;

internal sealed class UICanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _playfabButtonRequest;
    [SerializeField] private Button _photonButtonConnect;
    [SerializeField] private Button _photonButtonDisconnect;
    [SerializeField] private TMP_Text _playfabStatus;
    [SerializeField] private TMP_Text _playfabMessage;
    [SerializeField] private TMP_Text _photonStatus;
    [SerializeField] private TMP_Text _photonMessage;

    private PlayfabUIController _playfabUIController;
    private PhotonUIController _photonUIController;

    private void Start()
    {
        _playfabUIController = new PlayfabUIController(_playfabButtonRequest, _playfabStatus, _playfabMessage);
        _photonUIController = new PhotonUIController(_photonButtonConnect, _photonButtonDisconnect, _photonStatus, _photonMessage);
    }

    private void OnDestroy()
    {
        _playfabUIController.Destroy();
        _photonUIController.Destroy();
    }

    private void Update()
    {
        _photonUIController.Update();
    }

    public override void OnConnectedToMaster()
    {
        _photonUIController.OnConnectedToMaster();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _photonUIController.OnDisconnected(cause);
    }
}
