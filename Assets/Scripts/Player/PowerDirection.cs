using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerDirection : NetworkBehaviour
{
    private Camera _mainCamera;
    private Vector3 mousePositon;

    private Transform childTranform;
    private void Awake() {
        _mainCamera = Camera.main;
        childTranform = transform.GetChild(0);
    }
    private void FixedUpdate() {
        if(!IsOwner || !Application.isFocused) return;
        mousePositon = Input.mousePosition;
        Vector3 mouseWorldPostion = _mainCamera.ScreenToWorldPoint(mousePositon);
        Vector2 direction = mouseWorldPostion - childTranform.position;
        float rotZ = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg - 90;
        Quaternion quaternion = Quaternion.Euler(0,0,rotZ);
        SendRotationToServerRpc(quaternion);

    }

    [ServerRpc]
    private void SendRotationToServerRpc(Quaternion quaternion){
        childTranform.rotation = quaternion;
    }
}
