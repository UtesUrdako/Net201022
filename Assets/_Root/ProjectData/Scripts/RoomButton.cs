using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal sealed class RoomButton : MonoBehaviour
{
    public Button button;
    public TMP_Text text;

    private void Start()
    {
        button.onClick.AddListener(JoinRoom);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(text.text);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
