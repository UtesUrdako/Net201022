using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRoomPanel : MonoBehaviour
{
    [field: SerializeField] public Transform RoomListContent { get; private set; }
    [field: SerializeField] public Button ExitButton { get; private set; }

}
