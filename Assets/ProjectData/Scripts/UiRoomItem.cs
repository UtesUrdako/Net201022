using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRoomItem : MonoBehaviour
{
    [field: SerializeField] public Button RoomButton { get; private set; }
    [field: SerializeField] public TMPro.TMP_Text NameRoom { get; private set; }
}
