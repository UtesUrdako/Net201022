using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _idLabel;

    [SerializeField]
    private TMP_Text _usernameLabel;

    [SerializeField]
    private TMP_Text _accCreatedLabel;

    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
    }    

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _idLabel.text = $"PlayFab Id: {result.AccountInfo.PlayFabId}";
        _usernameLabel.text = $"Username: {result.AccountInfo.Username}";
        _accCreatedLabel.text = $"Account created: {result.AccountInfo.Created}";
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        Debug.Log("Catalog received successfully");

        HandleCatalog(result.Catalog);
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach(var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"{item.DisplayName} was added to catalog");
        }
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
