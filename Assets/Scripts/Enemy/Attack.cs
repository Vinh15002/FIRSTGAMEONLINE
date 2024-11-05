using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : NetworkBehaviour
{

    [SerializeField]
    private NetworkObject prefabBom;



    [SerializeField]
    private int damage;


    [SerializeField]
    private Transform spwanBomPosition;

    private Animator animator;


    [SerializeField]
    private NetworkObject UIDamage;


    public float AttackColdown{
        get {return animator.GetFloat("cooldownAttack");}

        private set {
            animator.SetFloat("cooldownAttack", value);
        }

    }


    private void FixedUpdate() {
        if(AttackColdown > 0){
            AttackColdown -= Time.deltaTime;
        }
        
        
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void onDealDamage(Collider2D other){
        if(AttackColdown < 0){
            animator.SetBool("isAttack", true);
            if(!NetworkManager.Singleton.IsServer) return;
            other.GetComponent<HealthController>().TakeDame(damage);
            AttackColdown = 1.5f;
            UISpwanDamaeServerRpc(other.transform.position);
        }
       
    }
    [ServerRpc]
    private void UISpwanDamaeServerRpc(Vector3 position)
    {
        NetworkObject game =  Instantiate(UIDamage, position, Quaternion.identity);
        game.GetComponent<DamaePopup>().textValue.Value = $"-{damage}";
        game.Spawn();
    }

    public void OnDealDamageExit(){
        animator.SetBool("isAttack", false);
        animator.SetBool("canMove", false);
    }

    public void onDealDame2(Collider2D other){
        animator.SetBool("canMove", true);
        animator.SetBool("isAttack", true);
        if(AttackColdown <1){
            animator.SetBool("isAttack", true);
            
            if(!NetworkManager.Singleton.IsServer) return;
            SpawnEnemyBomServerRpc(spwanBomPosition.position, other.transform.position);
            AttackColdown = 3f;
        }
        
        
    }

    [ServerRpc]
    public void SpawnEnemyBomServerRpc(Vector3 position, Vector3 targetPosition){
        NetworkObject game =  Instantiate(prefabBom, position, Quaternion.identity);
        Vector2 direction = targetPosition - position;
        game.GetComponent<Transform>().up = direction;
        game.Spawn();
    }


    



    
}
