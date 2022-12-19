using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    [field: SerializeField] public GameObject InfoPanel { get; private set; }
    [field: SerializeField] public GameObject LobbyPanel { get; private set; }
    [field: SerializeField] public GameObject RoomsPanel { get; private set; }
    [field:SerializeField] public PhotonLobbyManager PhotonLobbyManager { get; private set; }
    private void Awake()
    {
        InfoPanel.GetComponent<UiInformationPanel>().loginButton.onClick.AddListener(() =>
        {
            LobbyPanel.SetActive(true);
            InfoPanel.SetActive(false);
        });

        var lobby = LobbyPanel.GetComponent<UiCreateRoom>();
        lobby._listRoomsButtons.onClick.AddListener(() =>
        {
            LobbyPanel.SetActive(false);
            RoomsPanel.SetActive(true);
        });
        lobby._backButton.onClick.AddListener(() =>
        {
            LobbyPanel.SetActive(false);
            InfoPanel.SetActive(true);
        });

        RoomsPanel.GetComponent<UiRoomPanel>().ExitButton.onClick.AddListener(() =>
        {
            LobbyPanel.SetActive(true);
            RoomsPanel.SetActive(false);
        });
    }

}
