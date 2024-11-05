using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    public NetworkVariable<float> speed = new NetworkVariable<float>(7f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public Vector2 moveInput;

    private Rigidbody2D _rigibody;
    
    [SerializeField]
    private NetworkMovementComponent _playerMovement;


    private Camera camera;

    
    private void Awake() {
        _rigibody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
       
    }

    private void Update() { 
  
        if(!IsOwner || !Application.isFocused) return;
        
        

        if(IsClient && IsLocalPlayer){
            
            onMoveNetwork();
            _playerMovement.ProcessLocalPlayerMovement(moveInput, moveInput);
        }
        else{
            //onMoveNetwork();
            _playerMovement.ProcessSimulatedPlayerMovemen();
        }
        // HandleDirection();
        // HandleMoveInput();
        


    }

    private void FixedUpdate() {
        if(IsOwner){
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     //if(!IsOwner || !Application.isFocused) return;
    //     if(other.CompareTag("Obstacle")){
    //         Debug.Log("?????");
    //         //gameObject.GetComponent<Collider2D>().isTrigger = false;
    //     }
    // }

    private void HandleMoveInput(){
        HandleMoveInputServerRpc(moveInput);
    }

    [ServerRpc]
    private void HandleMoveInputServerRpc(Vector2 moveInput)
    {
        
        _rigibody.velocity = moveInput*speed.Value;
        
    }
    

    private void onMoveNetwork(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        moveInput = new Vector2(x,y).normalized;

    }
    private void HandleDirection(){
        if(moveInput!=Vector2.zero){

        
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, moveInput);
            Quaternion rotate = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f*Time.deltaTime);
            HandleDirectionServerRpc(rotate);
            
        }

    }
    [ServerRpc]
    private void HandleDirectionServerRpc(Quaternion rotate)
    {
        _rigibody.MoveRotation(rotate);
    }




}
