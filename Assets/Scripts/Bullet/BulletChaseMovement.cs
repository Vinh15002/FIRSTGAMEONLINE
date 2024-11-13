using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletChaseMovement : NetworkBehaviour
{

    [SerializeField]
    private float speed = 10f;

    private Rigidbody2D _rigidbody;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ChangeDirection(Vector3 Position)
    {
        Vector2 direction = (Position - transform.position).normalized;
        float RotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _rigidbody.velocity = direction*speed;
        _rigidbody.MoveRotation(RotZ);
    }

   




   

}
