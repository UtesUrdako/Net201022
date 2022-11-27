using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class Reqistration : MonoBehaviour
{
    [SerializeField] private InputField _userName_InputField;
    [SerializeField] private InputField _password_InputField;
    [SerializeField] private InputField _email_InputField;
    [SerializeField] private Button _button;
    [SerializeField] private Text _errorText;

    [SerializeField] private LoadingScreen _loadingScreen;
    private string _userName;
    private string _password;
    private string _email;

    private void Awake()
    {
        _userName_InputField.onValueChanged.AddListener(InitUserName);
        _email_InputField.onValueChanged.AddListener(UnitEmail);
        _password_InputField.onValueChanged.AddListener(UnitPassword);
        _button.onClick.AddListener(CreateUserAccount);
    }

    private void InitUserName(string val)
        => _userName = val;
    private void UnitEmail(string val)
        => _email = val;
    private void UnitPassword(string val)
        => _password = val;


    private void CreateUserAccount()
    {
        _loadingScreen.InitTokenSource();

        _loadingScreen.LoadingCircle();

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _userName,
            Password = _password,
            Email = _email,
            RequireBothUsernameAndEmail = true
        },
        result =>
        {
            _errorText.gameObject.SetActive(false) ;
            Debug.Log("GOOOD");
            _loadingScreen.StopCleaning();
            _loadingScreen.GoodScreen();

        },
        error =>
        {
            _errorText.gameObject.SetActive(true);
            _errorText.text = error.ToString();
            _loadingScreen.StopCleaning();
            _loadingScreen.BadScreen();
        }
        );
    }

}
