using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraFlollow : NetworkBehaviour
{
    Transform _mainCamera;
    private void Start() {
        _mainCamera = Camera.main.transform;
    }

    private void Update() {
        
        if(!IsOwner) return;
        _mainCamera.position = new Vector3(transform.position.x, transform.position.y, -10f);
       

    }
}
