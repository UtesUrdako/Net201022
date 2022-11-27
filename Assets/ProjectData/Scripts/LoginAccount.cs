using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoginAccount : MonoBehaviour
{
    [SerializeField] private InputField _userName_InputField;
    [SerializeField] private InputField _password_InputField;

    [SerializeField] private Button _button;
    [SerializeField] private Text _errorText;

    [SerializeField] private LoadingScreen _loadingScreen;
    private string _userName;
    private string _password;

    private void Awake()
    {
        _userName_InputField.onValueChanged.AddListener(InitUserName);
        _password_InputField.onValueChanged.AddListener(UnitPassword);
        _button.onClick.AddListener(Login);
    }

    private void InitUserName(string val)
        => _userName = val;
    private void UnitPassword(string val)
        => _password = val;


    private void Login()
    {
        _loadingScreen.InitTokenSource();

        _loadingScreen.LoadingCircle();

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _userName,
            Password = _password,
        },
        result =>
        {
            _errorText.gameObject.SetActive(false);
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
