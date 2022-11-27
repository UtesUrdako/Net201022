using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogManager : MonoBehaviour
{
    private Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
    void Start()
    {
        var catologRequest = new GetCatalogItemsRequest();
        PlayFabClientAPI.GetCatalogItems(catologRequest, Success, Failure);
    }


    private void Success(GetCatalogItemsResult itemsResult)
    {
        foreach(var item in itemsResult.Catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"DisplayName : {item.DisplayName}");
        }
    }
    private void Failure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
