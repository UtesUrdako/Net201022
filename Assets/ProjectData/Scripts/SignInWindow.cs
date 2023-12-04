using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Image _loadingImage;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        _loadingImage.enabled = true;
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password,
        }, result =>
        {
            Debug.Log($"Success: {_username}");
            _loadingImage.enabled = false;
            EnterInGameScene();
        }, error =>
        {
            _loadingImage.enabled = false;
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
    }
}
