using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class PlayfabCatalogReader
{
    private const string PREFAB_PATH = "Button";

    private Transform _root;
    private GameObject _buutonPrefab;

    public PlayfabCatalogReader(Transform root)
    {
        _root = root;
        _buutonPrefab = Resources.Load<GameObject>(PREFAB_PATH);
    }

    public void ReadCatalog()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnComplete, OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Error: {errorMessage}");
    }

    private void OnComplete(GetCatalogItemsResult result)
    {
        Debug.Log($"Catalog request complete!");
        InstantiateCatalogButtons(result.Catalog);
    }

    private void InstantiateCatalogButtons(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            GameObject go = GameObject.Instantiate(_buutonPrefab, _root);
            ButtonView buttonView = go.GetComponent<ButtonView>();
            buttonView.Init(item.DisplayName);
        }
    }

}
