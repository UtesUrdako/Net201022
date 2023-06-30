using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _mailField;

    [SerializeField] private Button _createAccountButton;

    private WaitingInfoController _waitingInfoContoller;

    private string _mail;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void UpdateMail(string mail)
    {
        _mail = mail;
    }

    private void CreateAccount()
    {
        _waitingInfoContoller = new WaitingInfoController();
        _waitingInfoContoller.Show();
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = _mail;
        request.Username = _username;
        request.Password = _password;

        PlayFabClientAPI.RegisterPlayFabUser(request, CreateAccountComplete, CreateAccountError);
    }

    private void CreateAccountComplete(RegisterPlayFabUserResult result)
    {
        _waitingInfoContoller.Destroy();
        Debug.Log($"Success: {_username}");
        EnterInGameScene();
    }

    private void CreateAccountError(PlayFabError error)
    {
        _waitingInfoContoller.Destroy();
        Debug.LogError($"Fail: {error.ErrorMessage}");
    }
}
