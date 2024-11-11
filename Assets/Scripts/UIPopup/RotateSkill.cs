using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RotateSkill : NetworkBehaviour
{
    private float RotZ = 0;

    [SerializeField]
    private float speed = 10;
   
    private void FixedUpdate() {
       
        transform.rotation = Quaternion.Euler(0,0,RotZ);
        RotZ += speed;
        
     
        
        
    }
}
