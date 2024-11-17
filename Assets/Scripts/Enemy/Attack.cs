using Assets.Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : NetworkBehaviour
{ 

    

    [SerializeField]
    private Transform spwanBomPosition;

    private Animator animator;


    private int damage;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        damage = GetComponent<Enemy>().Damage;

    }






    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void onDealDamage(Collider2D other){
      
        animator.SetBool("isAttack", true);
        //ObjectPooling.Singleton.SpawnBom(other.transform.position);
        ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"-{damage} HP", Color.red);
        

        if(!IsHost && IsOwner){
            SendInforClientRpc(other.transform.position);
        }
        if(NetworkManager.Singleton.IsServer){
            other.GetComponent<HealthController>().TakeDame(damage);
        }
            
        
       
    }
    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"-{damage} HP", Color.red);
    }

    public void OnDealDamageExit(){
        //animator.SetBool("isAttack", false);
        animator.SetBool("canMove", false);
    }

    public void onDealDame2(Collider2D other){
        
        animator.SetBool("isAttack", true);
        animator.SetBool("canMove", true);


       
        // if(AttackColdown <0.1f){
        //     animator.SetBool("isAttack", true);
        //     animator.SetBool("canMove", true);
        
        if(IsOwner){
            Vector2 direction = other.transform.position- transform.position;
            SpawnEnemyBomClientRpc(spwanBomPosition.position, Quaternion.identity, direction );
            if(!IsHost){
                ObjectPooling.Singleton.SpawnBomEnemy01(spwanBomPosition.position, Quaternion.identity, direction , damage);
            }
        }
           
            

        // }
        
        
    }




    [ClientRpc]
    public void SpawnEnemyBomClientRpc(Vector3 position, Quaternion identity, Vector2 direction){
       ObjectPooling.Singleton.SpawnBomEnemy01(position, Quaternion.identity, direction, damage);
    }


    



    
}
