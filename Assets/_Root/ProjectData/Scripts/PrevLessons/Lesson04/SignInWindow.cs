using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _backButton;

    public event Action ActionOnBackButtonPressed = delegate { };

    private WaitingInfoController _waitingInfoContoller;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInButton.onClick.AddListener(SignIn);
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
    }

    private void OnBackButtonPressed()
    {
        ActionOnBackButtonPressed.Invoke();
    }

    private void SignIn()
    {
        _waitingInfoContoller = new WaitingInfoController();
        _waitingInfoContoller.Show();

        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = _username;
        request.Password = _password;

        PlayFabClientAPI.LoginWithPlayFab(request, SignInComplete, SignInError);
    }

    private void SignInComplete(LoginResult result)
    {
        _waitingInfoContoller.Destroy();
        Debug.Log($"Success: {_username}");
        EnterLobbyScene();
    }

    private void SignInError(PlayFabError error)
    {
        _waitingInfoContoller.Destroy();
        Debug.LogError($"Fail: {error.ErrorMessage}");
    }
}
