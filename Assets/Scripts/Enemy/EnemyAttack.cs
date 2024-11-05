using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{



    public UnityEvent<Collider2D> onDealDamage;
    public UnityEvent onDealDamageExit;
    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.CompareTag("Player")){
            onDealDamage?.Invoke(other);
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            onDealDamageExit?.Invoke();
        }
    }
}
