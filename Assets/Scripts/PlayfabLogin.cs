using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.AI;

public class PlayfabLogin : MonoBehaviour
{
   private void Start()
   {
      if (string.IsNullOrEmpty((PlayFabSettings.staticSettings.TitleId)))
         PlayFabSettings.staticSettings.TitleId = "BB5DE";

      var request = new LoginWithCustomIDRequest
      {
         CustomId = "Player 1",
         CreateAccount = true
      };
      
      PlayFabClientAPI.LoginWithCustomID(request,OnLoginSucces,OnLoginError);
   }

   private void OnLoginSucces(LoginResult result)
   {
      Debug.Log("Complete Login!");
   }

   private void OnLoginError(PlayFabError error)
   {
      var errorMessage = error.GenerateErrorReport();
      Debug.LogError(errorMessage);
   }
}
