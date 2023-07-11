using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal sealed class CharacterDataView : MonoBehaviour
{
    private const string STAT_XP_KEY = "XP";
    private const string STAT_HEALTH_KEY = "Health";
    private const string STAT_DAMAGE_KEY = "Damage";
    private const string NEW_CHARACTER_TOKEN = "character_token";
    private const string PREFIX_NAME = "Name: ";
    private const string PREFIX_XP = "XP: ";
    private const string PREFIX_HEALTH = "Health: ";
    private const string PREFIX_DAMAGE = "Damage: ";
    private const int CHARACTER_NAME_MIN_LENGTH = 2;

    [SerializeField] GameObject _rootPanel;
    [SerializeField] GameObject _characterInfoPanel;
    [SerializeField] GameObject _createCharacterPanel;
    [SerializeField] Button _buttonCreateCharacter;
    [SerializeField] TMP_InputField _characterNameInputField;
    [SerializeField] TMP_Text _characterNameLabel;
    [SerializeField] TMP_Text _characterXP;
    [SerializeField] TMP_Text _characterHealth;
    [SerializeField] TMP_Text _characterDamage;

    private string _currentCharacterName;

    private void Start()
    {
        _buttonCreateCharacter.onClick.AddListener(OnCreateCharacterButtonPressed);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _currentCharacterName = string.Empty;

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            _rootPanel.SetActive(true);
            UpdateCharacterUI();
        }
        else
        {
            _rootPanel.SetActive(false);
        }
    }

    private void UpdateCharacterUI()
    {
        ListUsersCharactersRequest request = new ListUsersCharactersRequest();
        PlayFabClientAPI.GetAllUsersCharacters(request, GetAllUsersCharactersComplete, PlayFabErrorRequest);
    }

    private void PlayFabErrorRequest(PlayFabError error)
    {
        Debug.LogError(error.ErrorMessage);
    }

    private void GetAllUsersCharactersComplete(ListUsersCharactersResult result)
    {
        Debug.Log("GetAllUsersCharactersComplete");
        if (result.Characters.Count <= 0)
        {
            _characterInfoPanel.SetActive(false);
            _createCharacterPanel.SetActive(true);
        }
        else
        {
            _characterInfoPanel.SetActive(true);
            _createCharacterPanel.SetActive(false);

            _currentCharacterName = result.Characters[0].CharacterName;
            
            UpdateCharacterInfo(result.Characters[0].CharacterId);
        }
    }

    private void UpdateCharacterInfo(string characterId)
    {
        GetCharacterStatisticsRequest request = new GetCharacterStatisticsRequest();
        request.CharacterId = characterId;

        PlayFabClientAPI.GetCharacterStatistics(request, GetCharacterStatisticsComplete, PlayFabErrorRequest);
    }

    private void GetCharacterStatisticsComplete(GetCharacterStatisticsResult result)
    {
        Debug.Log("GetCharacterStatisticsComplete");
        _characterNameLabel.text = PREFIX_NAME + _currentCharacterName;
        UpdateLabelText(_characterXP, result.CharacterStatistics, STAT_XP_KEY, PREFIX_XP);
        UpdateLabelText(_characterHealth, result.CharacterStatistics, STAT_HEALTH_KEY, PREFIX_HEALTH);
        UpdateLabelText(_characterDamage, result.CharacterStatistics, STAT_DAMAGE_KEY, PREFIX_DAMAGE);
    }

    private void UpdateLabelText(TMP_Text label, Dictionary<string, int> dictionary, string statisticKey, string prefix)
    {
        if (dictionary.ContainsKey(statisticKey))
        {
            label.text = prefix + dictionary[statisticKey].ToString();
        }
        else
        {
            label.text = string.Empty;
        }


    }

    private void OnCreateCharacterButtonPressed()
    {
        if (_characterNameInputField.text.Length < CHARACTER_NAME_MIN_LENGTH)
        {
            Debug.LogError("Character name too short.");
            return;
        }
        CreateNewCharacter(_characterNameInputField.text);
    }

    private void CreateNewCharacter(string characterName)
    {
        GrantCharacterToUserRequest request = new GrantCharacterToUserRequest();
        request.CharacterName = characterName;
        request.ItemId = NEW_CHARACTER_TOKEN;

        PlayFabClientAPI.GrantCharacterToUser(request, GrantCharacterToUserComplete, PlayFabErrorRequest);
    }

    private void GrantCharacterToUserComplete(GrantCharacterToUserResult result)
    {
        Debug.Log("GrantCharacterToUserComplete");
        CreateNewCharacterStatistics(result.CharacterId);
    }

    private void CreateNewCharacterStatistics(string characterID)
    {
        UpdateCharacterStatisticsRequest request = new UpdateCharacterStatisticsRequest();
        request.CharacterId = characterID;
        request.CharacterStatistics = CreateRandomCharacterStatistic();
        PlayFabClientAPI.UpdateCharacterStatistics(request, UpdateCharacterStatisticsComplete, PlayFabErrorRequest);
    }

    private void UpdateCharacterStatisticsComplete(UpdateCharacterStatisticsResult result)
    {
        Debug.Log("UpdateCharacterStatisticsComplete");
        UpdateUI();
    }

    private Dictionary<string, int> CreateRandomCharacterStatistic()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        result.Add(STAT_XP_KEY, Random.Range(1, 1000));
        result.Add(STAT_HEALTH_KEY, Random.Range(1, 100));
        result.Add(STAT_DAMAGE_KEY, Random.Range(1, 100));
        return result;
    }

    private void OnDestroy()
    {
        _buttonCreateCharacter.onClick.RemoveAllListeners();
    }
}
