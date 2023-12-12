using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _idLabel;

    [SerializeField]
    private TMP_Text _usernameLabel;

    [SerializeField]
    private TMP_Text _accCreatedLabel;

    [SerializeField]
    private GameObject _newCharacterCreatePanel;

    [SerializeField]
    private Button _createCharacterButton;

    [SerializeField]
    private TMP_InputField _inputField;

    [SerializeField]
    private List<SlotCharacterWidget> _slots;

    private SlotCharacterWidget _currentSlot;
    private CharacterResult _currentCharacter;

    private string _characterName;

    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);

        GetCharacters();

        foreach (var slot in _slots)
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);

        _inputField.onValueChanged.AddListener(OnNameChanged);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), result =>
        {
            Debug.Log($"Character count: {result.Characters.Count}");
            ShowCharactersInSlot(result.Characters);
        }, OnError);
    }

    private void ShowCharactersInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
            {
                slot.ShowEmptySlot();
            }
        }
        else if (characters.Count > 0 && characters.Count <= _slots.Count) //как можно сделать этот блок лучше?
        {
            var i = 0; 
            foreach(var slot in _slots)
            {
                var _characterName = characters[i].CharacterName;
                PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest { CharacterId = characters[i].CharacterId },
                result =>
                {
                    var characterName = _characterName;
                    var level = result.CharacterStatistics["Level"].ToString();
                    var gold = result.CharacterStatistics["Gold"].ToString();
                    var health = result.CharacterStatistics["Health"].ToString();
                    var attack = result.CharacterStatistics["Attack"].ToString();
                    var exp = result.CharacterStatistics["Experience"].ToString();
                    slot.ShowInfoCharacterSlot(characterName, level, gold, health, attack, exp);
                }, OnError);
                i++;
            }              
        }
        else
        {
            Debug.Log("Add more slots for characters.");
        }
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(true);
    }

    private void CloseCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(false);
    }

    private void OnNameChanged(string changedName)
    {
        _characterName = changedName;
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_token"
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
            Debug.Log("");
        }, OnError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics =  new Dictionary<string, int>
            {
                {"Level", 1 },
                {"Gold", 0 },
                {"Health", 100 },
                {"Attack", 10 },
                {"Experience", 0 }
            }
        }, result =>
        {
            Debug.Log("Update completed");
            CloseCreateNewCharacter();
            GetCharacters();
        }, OnError);
    }       

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _idLabel.text = $"PlayFab Id: {result.AccountInfo.PlayFabId}";
        _usernameLabel.text = $"Username: {result.AccountInfo.Username}";
        _accCreatedLabel.text = $"Account created: {result.AccountInfo.Created}";
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        Debug.Log("Catalog received successfully");

        HandleCatalog(result.Catalog);
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach(var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"{item.DisplayName} was added to catalog");
        }
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
