using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.EnemySpawn;
using Assets.Scripts.Events;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : NetworkBehaviour
{




  

    private Animator animator;
    private int currentHealth;
    private int maxHealth;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        currentHealth = GetComponent<Enemy>().CurrentHealth;
        maxHealth = GetComponent<Enemy>().MaxHealth;
        animator = GetComponent<Animator>();

    }

    public void OnEnable()
    {
       
        
    }

   
   


    
    private bool IsDying{
        get{return currentHealth <= 1;}
    }

    public UnityEvent onDie;
    public UnityEvent<int,int> OnChangebar;

    [ContextMenu("TAKEDAME")]
    public void TakeDame(int damage){
        if(IsDying) return;
       



        GetComponent<Enemy>().CurrentHealth -= damage;
        currentHealth -= damage;

        OnChangebar?.Invoke(currentHealth, maxHealth);
        //ChangeHealthBarEnemy.changeHealth?.Invoke(PresentOfHealth);

        //onHitClientRpc();




        if (currentHealth <= 1)
        {
            onDieEmeny();
            //onHitEmenyServerRpc();
        }
        else {
            animator.SetTrigger("isHit");

        }

    
  
    }


    public void onDieEmeny(){
        
      
        animator.SetTrigger("isDying");
        DropItem.dropItemRate?.Invoke(100, transform.position);

        StartCoroutine(DestroyObject());
       
    }

    public IEnumerator DestroyObject(){
        onDie.Invoke();
        yield return new WaitForSeconds(0.7f);
        NetworkObject.Despawn();
        //SetDestroy();
        //SendTheServerRpc();
       
    }

    //[ServerRpc]
    //private void SendTheServerRpc()
    //{
    //    SendTheClientRpc();
       
    //}

    //[ClientRpc]
    //private void SendTheClientRpc()
    //{
    //    SetDestroy();
    //}

    private void Awake() {
        animator = GetComponent<Animator>();
    }
}
