using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasHPController : MonoBehaviour
{
    public event Action<int> ActionOnHPUpdate = delegate { };

    private const string HP_KEY = "HP";

    [SerializeField] TMP_Text _hpUI;

    private void Start()
    {
        UpdateHPText(string.Empty);
        UpdateHP();
    }

    private void UpdateHPText(string value)
    {
        _hpUI.text = value;

        if (value != string.Empty)
            ActionOnHPUpdate.Invoke(int.Parse(value));
    }

    private void UpdateHP()
    {
        GetUserDataRequest request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, GetUserDataComplete, GetUserDataError);
    }

    private void GetUserDataError(PlayFabError error)
    {
        Debug.LogError(error.ErrorMessage);
    }

    private void GetUserDataComplete(GetUserDataResult result)
    {
        if (result.Data.ContainsKey(HP_KEY))
        {
            string HPValue = result.Data[HP_KEY].Value;
            UpdateHPText(HPValue);
        }
        else
        {
            CreateNewHPRecord();
        }
    }

    private void CreateNewHPRecord()
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest();
        request.Data = new Dictionary<string, string>
        {
            { HP_KEY, "100" }
        };

        PlayFabClientAPI.UpdateUserData(request, UpdateUserDataComplete, UpdateUserDataError);
    }

    private void UpdateUserDataError(PlayFabError error)
    {
        Debug.LogError(error.ErrorMessage);
    }

    private void UpdateUserDataComplete(UpdateUserDataResult result)
    {
        UpdateHP();
    }
}
