
using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : MonoBehaviour
{
    public void StartServer(){
        NetworkManager.Singleton.StartServer();
    }
    public void StartHost(){
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(3269213171);
        NetworkManager.Singleton.StartHost();
        
    }
    public void StartClient(){
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(3666373149);

        NetworkManager.Singleton.StartClient();
    }


    

}
