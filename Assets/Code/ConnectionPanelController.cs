using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionPanelController : MonoBehaviour
{
    [SerializeField] private PlayFabPanelView _view;
    [SerializeField] private PlayFabLogin _login;
    [SerializeField] private Launcher _photonLauncher;

    private void Start()
    {
        if (!_view)
            return;


        if (_login)
        {
            _view.PlayFabLogInButton.onClick.AddListener(_login.LogIn);
            _login.OnLoginSuccesEvent.AddListener(DisplayPositiveText);
            _login.OnLoginErrorEvent.AddListener(DisplayNegativeText);
        }
        
        if (_photonLauncher)
        {
            _view.PhotonButton.onClick.AddListener(_photonLauncher.Connect);
            _photonLauncher.OnConnectedToMasterEvent.AddListener(DisplayPositiveText);
            _photonLauncher.OnJoinedRoomEvent.AddListener(DisplayPositiveText);
        }


        

        

    }

    private void DisplayPositiveText(string result)
    {
        _view.InputField.text = result;
        _view.InputField.textComponent.color = Color.green;
    }

    private void DisplayNegativeText(string result)
    {
        AddText(result);
        _view.InputField.textComponent.color = Color.red;
        
    }

    private void AddText(string text)
    {
        StringBuilder s = new StringBuilder();
        s.Append(_view.InputField.text);
        s.Append("\n");
        s.Append(text);

        _view.InputField.text = s.ToString();
    }

    private void OnDestroy()
    {
        _view.PlayFabLogInButton.onClick.RemoveListener(_login.LogIn);
        _login.OnLoginSuccesEvent.RemoveListener(DisplayPositiveText);
        _login.OnLoginErrorEvent.RemoveListener(DisplayNegativeText);
    }

}
