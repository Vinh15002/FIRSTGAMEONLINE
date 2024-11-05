using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{



    [SerializeField]
    private int _currentHealth ;
    [SerializeField]
    private int _maxHealth;

    private Animator animator;


    private float PresentOfHealth{
        get {return (float)_currentHealth/_maxHealth;}
    }

    private bool IsDying{
        get{return _currentHealth<=1;}
    }

    public UnityEvent<float> onDame;
    public UnityEvent onDie;

    [ContextMenu("TAKEDAME")]
    public void TakeDame(int damage){
        if(IsDying) return;
        
        
        
        _currentHealth -= damage;
        onDame.Invoke(PresentOfHealth);



        if(_currentHealth<=1){
            
            onDieEmenyServerRpc();
        }
        else {
            onHitEmenyServerRpc();
            
            
        }

    }
    [ServerRpc]
    private void onHitEmenyServerRpc()
    {
        animator.SetTrigger("isHit");
    }

    
    [ServerRpc]
    public void onDieEmenyServerRpc(){
        
        animator.SetTrigger("isDying");
        onDie.Invoke();
        if(!NetworkManager.Singleton.IsServer) return;
        GetComponent<NetworkObject>().Despawn();
    }
    private void Awake() {
        animator = GetComponent<Animator>();
    }
}
