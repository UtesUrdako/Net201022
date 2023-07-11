using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHPRotator : MonoBehaviour
{
    private void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 currentPosition = transform.position;

        transform.forward = currentPosition - cameraPosition;
    }
}
