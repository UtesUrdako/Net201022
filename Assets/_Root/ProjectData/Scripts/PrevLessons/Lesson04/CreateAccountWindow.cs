using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _mailField;

    [SerializeField] private Button _createAccountButton;
    [SerializeField] private Button _backButton;

    public event Action ActionOnBackButtonPressed = delegate { };

    private WaitingInfoController _waitingInfoContoller;

    private string _mail;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountButton.onClick.AddListener(CreateAccount);
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnDestroy()
    {
        _mailField.onValueChanged.RemoveAllListeners();
        _createAccountButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
    }

    private void OnBackButtonPressed()
    {
        ActionOnBackButtonPressed.Invoke();
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
        EnterLobbyScene();
    }

    private void CreateAccountError(PlayFabError error)
    {
        _waitingInfoContoller.Destroy();
        Debug.LogError($"Fail: {error.ErrorMessage}");
    }
}
