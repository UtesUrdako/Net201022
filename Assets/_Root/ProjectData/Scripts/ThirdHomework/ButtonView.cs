using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal sealed class ButtonView : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _text;

    public void Init(string buttonText)
    {
        _text.text = buttonText;
    }
}
