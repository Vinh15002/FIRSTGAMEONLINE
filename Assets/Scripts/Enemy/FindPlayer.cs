using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class FindPlayer : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.CompareTag("Player")){
            gameObject.GetComponentInParent<EnemyMovement>().findTarget(other.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            gameObject.GetComponentInParent<EnemyMovement>().hasTarget = false;
        }
    }
}
