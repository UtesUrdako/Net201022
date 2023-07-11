using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] TMP_Text _titleLabel;
    [SerializeField] Transform _itemsRoot;

    private void Start()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, GetAccountInfoComplete, GetAccountInfoError);

        PlayfabCatalogReader playfabCatalogReader = new PlayfabCatalogReader(_itemsRoot);
        playfabCatalogReader.ReadCatalog();
    }

    private void GetAccountInfoComplete(GetAccountInfoResult result)
    {
        _titleLabel.text = $"Playfab ID: {result.AccountInfo.PlayFabId}";
        _titleLabel.text += $"\nUser name: {result.AccountInfo.Username}";
        _titleLabel.text += $"\nCreated: {result.AccountInfo.Created}";
    }

    private void GetAccountInfoError(PlayFabError error)
    {
        string errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
