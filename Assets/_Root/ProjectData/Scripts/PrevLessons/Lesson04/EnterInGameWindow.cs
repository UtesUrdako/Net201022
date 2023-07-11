using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _createAccountButton;
    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private Canvas _signInCanvas;

    private SignInWindow _signInWindow;
    private CreateAccountWindow _createAccountWindow;

    private void Start()
    {
        _signInButton.onClick.AddListener(OpenSignInWindow);
        _createAccountButton.onClick.AddListener(OpenCreateAccountWindow);
        
        _signInWindow = FindObjectOfType<SignInWindow>();
        _signInWindow.ActionOnBackButtonPressed += ResetUI;
        
        _createAccountWindow = FindObjectOfType<CreateAccountWindow>();
        _createAccountWindow.ActionOnBackButtonPressed += ResetUI;
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveAllListeners();
        _createAccountButton.onClick.RemoveAllListeners();

        _signInWindow.ActionOnBackButtonPressed -= ResetUI;
        _createAccountWindow.ActionOnBackButtonPressed -= ResetUI;
    }

    private void OpenSignInWindow()
    {
        _signInCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
    }

    private void OpenCreateAccountWindow()
    {
        _createAccountCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
    }

    private void ResetUI()
    {
        _signInCanvas.enabled = false;
        _createAccountCanvas.enabled = false;
        _enterInGameCanvas.enabled = true;
    }

}
