using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FindEnemy : NetworkBehaviour
{




    [SerializeField]
    private float timeToDealDame = 1f;
    private float _timeToDealDame = 0f;


    public NetworkVariable<int> damage = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

    }



    public void OnEnable()
    {
        ChangeDamageEvent.changeDamgeBullet += OnChangeDamage;
    }

    private void OnChangeDamage(int dam)
    {
        damage.Value = dam;
    }

    public void OnDisable()
    {
        ChangeDamageEvent.changeDamgeBullet -= OnChangeDamage;
    }

    private void FixedUpdate() {
        if(IsServer){
            _timeToDealDame += Time.deltaTime;
        }
    }


    private void OnTriggerStay2D(Collider2D other) {
        if(!other.CompareTag("Enemy")) return;
        if(_timeToDealDame >= timeToDealDame){
            
            if (NetworkManager.Singleton.IsServer){
                TowerEvent.getPosition?.Invoke(other.transform.position);
                _timeToDealDame = 0;
            }
            Vector3 direction = other.transform.position - transform.position;
            ObjectPooling.Singleton.SpawnBullet(transform.position,transform.rotation, direction, 3, damage.Value);
            if(IsOwner){
                SendTheClientRpc(direction);
            }
        }
    }
    [ClientRpc]
    private void SendTheClientRpc(Vector3 direction)
    {
         ObjectPooling.Singleton.SpawnBullet(transform.position,transform.rotation, direction, 3, damage.Value);
    }

    
}
