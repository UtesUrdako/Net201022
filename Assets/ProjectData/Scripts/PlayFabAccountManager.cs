using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _idLabel;

    [SerializeField]
    private TMP_Text _usernameLabel;

    [SerializeField]
    private TMP_Text _accCreatedLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }   

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _idLabel.text = $"PlayFab Id: {result.AccountInfo.PlayFabId}";
        _usernameLabel.text = $"Username: {result.AccountInfo.Username}";
        _accCreatedLabel.text = $"Account created: {result.AccountInfo.Created}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
