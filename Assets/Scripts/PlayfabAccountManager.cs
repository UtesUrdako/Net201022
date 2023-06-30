using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] TMP_Text _titleLabel;

    private void Start()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, GetAccountInfoComplete, GetAccountInfoError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
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

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        Debug.LogError("OnGetCatalogSuccess");
        ShowItems(result.Catalog);
    }

    private void ShowItems(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            Debug.Log($"{item.ItemId}");
        }
    }
}
