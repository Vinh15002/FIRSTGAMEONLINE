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
    private int typeBullet;


    [SerializeField]
    private NetworkObject bulletPrefab;

    [SerializeField]
    private Transform offset;


    [SerializeField]
    private float timecooldown = 0.5f;
    private float _timecooldown = 0.5f;


    [SerializeField]
    public NetworkVariable<int> DamageFire =  new NetworkVariable<int>(7,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); 

    private bool canFire {
        get {return _timecooldown>timecooldown;}
    }

    private Rigidbody2D _rigibody;


   
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
        if(IsOwner){
            Vector3 position = offset.position;
            Quaternion rotation = transform.GetChild(0).transform.rotation;
            Vector3 direction = transform.GetChild(0).transform.up;

            //FireServerRpc(position, rotation, direction);

            //ObjectPooling.Singleton.SpawnBullet01(position,rotation, direction);
            FireServerRpc(position, rotation, direction, DamageFire.Value);

            // if(NetworkManager.Singleton.IsHost){
            //     FireClientRpc(position, rotation, direction);
            // }
        }
       
        

        
    }
    [ClientRpc]
    private void FireClientRpc(Vector3 position, Quaternion rotation, Vector3 direction, int DamageFire)
    {
        ObjectPooling.Singleton.SpawnBullet(position,rotation, direction,typeBullet,DamageFire);
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 position, Quaternion rotation, Vector3 direction, int DamageFire)
    {
        
        //ObjectPooling.Singleton.SpawnBullet(position,rotation, direction,typeBullet, DamageFire);
        FireClientRpc(position,rotation, direction, DamageFire);
    }

    // [ServerRpc]
    // public void FireServerRpc(Vector3 position,Quaternion rotation, Vector3 direction, ServerRpcParams serverRpcParams = default){
    //     // NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId,false,true,false,
    //     // position,rotation);
    //     //FireClientRpc(position,rotation,direction, serverRpcParams.Receive.SenderClientId);
    //     NetworkObject game = Instantiate(bulletPrefab, position, rotation);
    //     Debug.Log("Direction: " + direction);
    //     game.GetComponent<BulletMovement>().direction = direction;
    //     game.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);


    // }









}
