using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayFabPanelView : MonoBehaviour
{
    [SerializeField] private Button _playFabButton;
    [SerializeField] private Button _photonConnectionButton;
    [SerializeField] private TMP_InputField _inputField;

    public Button PlayFabLogInButton { get => _playFabButton; }
    public Button PhotonButton { get => _photonConnectionButton; }
    public TMP_InputField InputField { get => _inputField; }
}
