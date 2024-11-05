using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Fire : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject bulletPrefab;

    [SerializeField]
    private Transform offset;


    [SerializeField]
    private float timecooldown = 0.5f;
    private float _timecooldown = 0.5f;

    private bool canFire {
        get {return _timecooldown>timecooldown;}
    }

    private Rigidbody2D _rigibody;

    private float speed = 10f;

   
    private void Awake() {
        _rigibody = GetComponent<Rigidbody2D>();
       
    }

    private void Update() {
        
        if(!IsOwner) return;
        _timecooldown += Time.deltaTime;
        if(Input.GetButtonDown("Fire1") && canFire){
            // _rigibody.velocity = Vector2.zero;
            onFire();

            _timecooldown = 0;
            
        }
    }


    public void onFire(){
        
        Vector3 position = offset.position;
        Quaternion rotation = transform.GetChild(0).transform.rotation;
        Vector2 direction = transform.GetChild(0).transform.up;
        
        FireServerRpc(position, rotation, direction);
        

        
    }


    [ServerRpc(RequireOwnership =false)]
    public void FireServerRpc(Vector3 position,Quaternion rotation, Vector2 direction, ServerRpcParams serverRpcParams = default){
        // NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId,false,true,false,
        // position,rotation);
        //FireClientRpc(position,rotation,direction, serverRpcParams.Receive.SenderClientId);
        NetworkObject game = Instantiate(bulletPrefab, position, rotation);
        game.GetComponent<Rigidbody2D>().velocity = direction*speed;
      
        game.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
    








}
