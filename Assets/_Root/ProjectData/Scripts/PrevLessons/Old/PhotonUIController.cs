
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal sealed class PhotonUIController
{
    private const string STATUS_CONNECTED = "Connected";
    private const string STATUS_DISCONNECTED = "Disconnected";
    private const string MESSAGE_CONNECTED_TO_MASTER = "Connected to MasterServer";

    private const string PLAYER_ID = "TestUser";
    private const string PLAYER_NICK_NAME = "TestUser";

    private Button _photonButtonConnect;
    private Button _photonButtonDisconnect;
    private TMP_Text _photonStatus;
    private TMP_Text _photonMessage;

    public PhotonUIController(Button photonButtonConnect, Button photonButtonDisconnect, TMP_Text photonStatus, TMP_Text photonMessage)
    {
        _photonButtonConnect = photonButtonConnect;
        _photonButtonDisconnect = photonButtonDisconnect;
        _photonStatus = photonStatus;
        _photonMessage = photonMessage;

        Init();
    }

    private void ClearLabels()
    {
        _photonStatus.text = string.Empty;
        _photonMessage.text = string.Empty;
    }

    private void Init()
    {
        _photonButtonConnect.onClick.AddListener(ButtonConnectPressed);
        _photonButtonDisconnect.onClick.AddListener(ButtonDisconnectPressed);
        ClearLabels();
    }

    private void SetButtonsInteractable()
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonButtonConnect.interactable = false;
            _photonButtonDisconnect.interactable = true;
        }
        else
        {
            _photonButtonConnect.interactable = true;
            _photonButtonDisconnect.interactable = false;
        }
    }

    private void SetStatusText()
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonStatus.text = STATUS_CONNECTED;
            _photonStatus.color = Color.green;
        }
        else
        {
            _photonStatus.text = STATUS_DISCONNECTED;
            _photonStatus.color = Color.red;
        }
    }

    public void Update()
    {
        SetButtonsInteractable();
        SetStatusText();
    }

    public void Destroy()
    {
        _photonButtonConnect.onClick.RemoveAllListeners();
        _photonButtonDisconnect.onClick.RemoveAllListeners();
    }

    private void ButtonConnectPressed()
    {
        ClearLabels();
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.AuthValues = new AuthenticationValues(PLAYER_ID);
        PhotonNetwork.NickName = PLAYER_NICK_NAME;
        PhotonNetwork.GameVersion = PhotonNetwork.AppVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void ButtonDisconnectPressed()
    {
        ClearLabels();
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        _photonMessage.text = cause.ToString();
        Debug.Log(cause);
    }

    public void OnConnectedToMaster()
    {
        _photonMessage.text = MESSAGE_CONNECTED_TO_MASTER;
        Debug.Log("OnConnectedToMaster");
    }
}
