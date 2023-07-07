using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal sealed class WaitingInfoController
{
    private const string PREFAB_PATH = "Waiting";

    private GameObject _waitingWindow;
    private GameObject _prefab;
    
    public WaitingInfoController()
    {
        _prefab = Resources.Load<GameObject>(PREFAB_PATH);
    }

    public void Show()
    {
        if (_waitingWindow == null) _waitingWindow = GameObject.Instantiate(_prefab);
    }

    public void Destroy()
    {
        if (_waitingWindow != null) GameObject.Destroy(_waitingWindow);
    }
}
