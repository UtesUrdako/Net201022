using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;

    private WaitingInfoController _waitingInfoContoller;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInButton.onClick.AddListener(SignIn);
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
        EnterInGameScene();
    }

    private void SignInError(PlayFabError error)
    {
        _waitingInfoContoller.Destroy();
        Debug.LogError($"Fail: {error.ErrorMessage}");
    }
}
