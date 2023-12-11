using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField]
    private Button _loginButton;
    [SerializeField]
    private TMP_Text _loginResultText;

    private const string AuthGuidKey = "auth_guid_key";

    private void Start()
    {
        _loginButton.onClick.AddListener(LogIn);

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "F6970";       

    }

    private void LogIn()
    {
        var accountExists = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !accountExists
        };

        PlayFabClientAPI.LoginWithCustomID(request, 
            result => 
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(result);
            }, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login completed");
        _loginResultText.color = Color.green;
        _loginResultText.text = "You have successfully logged in!";
        SetUserData(result.PlayFabId);
        //MakePurchase();
        GetInvetory();
        SceneManager.LoadScene(1);
    }

    private void GetInvetory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => ShowInventory(result.Inventory), OnLoginError);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log($"{firstItem.DisplayName}");
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        }, result =>
        {
            Debug.Log("Item consumed");
        }, OnLoginError);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "main",
            ItemId = "health_potion",
            Price = 1,
            VirtualCurrency = "SC"

        }, result =>
        {
            Debug.Log("Purchase completed");
        }, OnLoginError);
    }

    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { "time_receive_daily_reward", DateTime.UtcNow.ToString() }
            }
        }, result =>
        {
            Debug.Log("SetUserData");
            GetUserData(playFabId, "time_receive_daily_reward");
        }, OnLoginError);        
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId,
        }, result =>
        {
            if (result.Data.ContainsKey(keyData))
                Debug.Log($"{keyData}: {result.Data[keyData].Value}");
        }, OnLoginError);
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
        _loginResultText.color = Color.red;
        _loginResultText.text =  $"Login to account was interrupted due to an error: {errorMessage}";
    }
}
