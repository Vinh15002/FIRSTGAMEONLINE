using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SkillPlayer02 : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject SkillEprefab;

    [SerializeField]
    private NetworkObject SkillSpace;



     [SerializeField]
    
    private float cooldownSkillE = 10f;

    [SerializeField]
    private float cooldownSpace = 1f;

    private NetworkVariable<float> _cooldownSkillSpace = new NetworkVariable<float>(5f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<float> _cooldownSkillE = new NetworkVariable<float>(10f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


     private void Awake() {
        _cooldownSkillE.OnValueChanged += ChangeValueE;
        _cooldownSkillSpace.OnValueChanged += ChangeValueSpace;
       
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SkillEnvent.changeBackGroundSkill?.Invoke(2);
    }

    private void ChangeValueSpace(float previousValue, float newValue)
    {
        if(!IsOwner) return;
        SkillEnvent.skillSpace?.Invoke(newValue,cooldownSpace );
    }

    private void ChangeValueE(float previousValue, float newValue)
    {
        if(!IsOwner) return;
        SkillEnvent.skillE?.Invoke(newValue,cooldownSkillE);
    }

    private void Update() {

         if(IsServer){
            _cooldownSkillE.Value = _cooldownSkillE.Value >=0 ? _cooldownSkillE.Value  - Time.deltaTime : 0;
            _cooldownSkillSpace.Value = _cooldownSkillSpace.Value >=0 ? _cooldownSkillSpace.Value  - Time.deltaTime : 0;
        }

        Skill01();
        Skill02();

    }

    private void Skill02()
    {
         if(Input.GetKeyDown(KeyCode.Space) && _cooldownSkillSpace.Value <= 0){
            if(IsOwner){
               
                Vector3 PositonMouse = Input.mousePosition;
                Vector3 MousePositionWorld = Camera.main.ScreenToWorldPoint(PositonMouse);
                Vector3 positionSpwan = new Vector3(MousePositionWorld.x, MousePositionWorld.y,0f);
                SpwanSkillSpcaeServerRpc(positionSpwan);

            }
                
        }
    }


    

    [ServerRpc]
    private void SpwanSkillSpcaeServerRpc(Vector3 positionSpwan)
    {
        NetworkObject game =  Instantiate(SkillSpace, positionSpwan, Quaternion.identity);
        game.Spawn();
        _cooldownSkillSpace.Value = cooldownSpace;
    }

    private void Skill01()
    {
        if(Input.GetKeyDown(KeyCode.E) && _cooldownSkillE.Value <= 0){
           
            if(IsOwner){
                Vector3 PositonMouse = Input.mousePosition;
                Vector3 MousePositionWorld = Camera.main.ScreenToWorldPoint(PositonMouse);
                Vector3 positionSpwan = new Vector3(MousePositionWorld.x, MousePositionWorld.y,0f);
                SpwanSkillEServerRpc(positionSpwan);
                
            }
                
            
        }
    }

    [ServerRpc]
    private void SpwanSkillEServerRpc(Vector3 positionSpwan)
    {

        NetworkObject game =  Instantiate(SkillEprefab, positionSpwan, Quaternion.identity);
        game.Spawn();
        _cooldownSkillE.Value = cooldownSkillE;

    }

}
