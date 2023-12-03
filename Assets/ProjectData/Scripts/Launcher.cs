using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    private Button _connectButton;

    [SerializeField]
    private TMP_Text _buttonText;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        _buttonText.text = "Disconnected";
        _connectButton.GetComponent<Image>().color = Color.red;
        //_connectButton.onClick.AddListener(Connect);
        _connectButton.onClick.AddListener(ChangeConnection);
    }

    /*private void Connect()
    {
        if (PhotonNetwork.IsConnected)
            return;

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = Application.version;
        _connectButton.onClick.RemoveAllListeners();
        _buttonText.text = "Connected";
        _connectButton.GetComponent<Image>().color = Color.green;
        _connectButton.onClick.AddListener(Disconnect);        
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
        _connectButton.onClick.RemoveAllListeners();
        _buttonText.text = "Disconnected";
        _connectButton.onClick.AddListener(Connect);
        _connectButton.GetComponent<Image>().color = Color.red;
    }*/

    private void ChangeConnection()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            _buttonText.text = "Disconnected";
            _connectButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
            _buttonText.text = "Connected";
            _connectButton.GetComponent<Image>().color = Color.green;
        }         
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.CreateRoom("NewName");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("You have been disconnected");

    }
}
