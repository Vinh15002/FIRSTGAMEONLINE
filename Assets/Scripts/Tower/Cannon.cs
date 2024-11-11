using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;

public class Cannon : NetworkBehaviour
{
   



    private void OnEnable() {
        TowerEvent.getPosition += ChangeRotate;
    }

    private void OnDisable() {
        TowerEvent.getPosition -= ChangeRotate;
    }



    private void ChangeRotate(Vector3 position)
    {
        if(IsServer){
            Vector3 direction = transform.position - position;
            direction = direction.normalized;
            float rotZ = -(Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg+180);
            Quaternion quaternion = Quaternion.Euler(0,0,rotZ);
            GetComponent<Rigidbody2D>().MoveRotation(quaternion);
        }
    }
}
