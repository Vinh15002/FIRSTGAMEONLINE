using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMovementComponent : NetworkBehaviour
{
    [SerializeField]

    public NetworkVariable<float> speed = new NetworkVariable<float>(7f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

   
    private int _tick = 0;

    private float _tickRate = 1f/60f;

    private float _tickDeltaTime = 0f;

    private const int BUFFER_SIZE = 1024;


    private InputState[] _inputStates = new InputState[BUFFER_SIZE];

    private TransformState[] _transformState = new TransformState[BUFFER_SIZE];

    public NetworkVariable<TransformState> ServerTransformState = new NetworkVariable<TransformState>();

    public TransformState _previousTransformState;


 
    private Camera camera;


    private void OnEnable() {
        camera = Camera.main;
        ServerTransformState.OnValueChanged += OnServerStateChange;
    }

    private void OnServerStateChange(TransformState previousValue, TransformState newValue)
    {
        
        _previousTransformState = previousValue;
    }

    public void ProcessLocalPlayerMovement(Vector2 MovementInput, Vector2 LookInput){
        _tickDeltaTime += Time.deltaTime;


        if(_tickDeltaTime > _tickRate){
            int bufferIndex = _tick % BUFFER_SIZE;

            if(!IsServer){
                
                MovePlayerServerRpc(_tick, MovementInput, LookInput);
                // MovePlayer(MovementInput);
                // RotatePlayer(LookInput);
               
            
            }
            else {
                MovePlayer(MovementInput);
                RotatePlayer(LookInput);


                TransformState state = new TransformState(){
                Tick = _tick,
                Position = transform.position,
                Rotation = transform.rotation,
                HasStartedMoving = true,
                };

                _previousTransformState = ServerTransformState.Value;
                ServerTransformState.Value = state;
               
            
            }

            InputState inputState = new InputState(){
                Tick = _tick,
                movementInput = MovementInput,
                lookInput = LookInput,
            };

            TransformState transformState = new TransformState(){
                Tick = _tick,
                Position = transform.position,
                Rotation = transform.rotation,
                HasStartedMoving = true,
            };

            _inputStates[bufferIndex] = inputState;
            _transformState[bufferIndex] = transformState;

            _tickDeltaTime -= _tickRate;
            _tick++;
        }

        
    }


    public void ProcessSimulatedPlayerMovemen(){
        _tickDeltaTime += Time.deltaTime;
        if(_tickDeltaTime > _tickRate){
            if(ServerTransformState.Value.HasStartedMoving){
                transform.position = ServerTransformState.Value.Position;
                transform.rotation = ServerTransformState.Value.Rotation;

            }
            _tickDeltaTime -= _tickRate;
            _tick++;
        }
    }
    [ServerRpc]
    private void MovePlayerServerRpc(int tick, Vector2 movementInput, Vector2 lookInput)
    {
        MovePlayer(movementInput);
        RotatePlayer(lookInput);


        TransformState state = new TransformState(){
            Tick = tick,
            Position = transform.position,
            Rotation = transform.rotation,
            HasStartedMoving = true,
        };

        _previousTransformState = ServerTransformState.Value;
        ServerTransformState.Value = state;
        if(Application.isFocused){
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        

    }

    private void RotatePlayer(Vector2 lookInput)
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, lookInput);
        Quaternion rotate = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f*_tickRate);
        GetComponent<Rigidbody2D>().MoveRotation(rotate);
    }

    private void MovePlayer(Vector2 movementInput)
    {
        GetComponent<Rigidbody2D>().velocity = movementInput*speed.Value;
    }
}
