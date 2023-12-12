using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _emptySlot;

    [SerializeField]
    private GameObject _infoCharacterSlot;

    [SerializeField]
    private TMP_Text _nameLabel;

    [SerializeField]
    private TMP_Text _levelLabel;

    [SerializeField]
    private TMP_Text _goldLabel;

    [SerializeField]
    private TMP_Text _healthLabel;

    [SerializeField]
    private TMP_Text _attackLabel;

    [SerializeField]
    private TMP_Text _expLabel;

    public Button SlotButton => _button;

    public void ShowInfoCharacterSlot(string name, string level, string gold, string health, string attack, string exp) 
    { 
        _nameLabel.text = name;
        _levelLabel.text = $"Level: {level}";
        _goldLabel.text = $"Gold: {gold}";
        _healthLabel.text = $"HP: {health}";
        _attackLabel.text = $"AP: {attack}";
        _expLabel.text = $"XP: {exp}";


        _infoCharacterSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        _infoCharacterSlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}

