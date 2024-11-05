

using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;


public class RelayManager: Singleton<RelayManager> {

    public string _joinCode;
    private string _ip;
    private int _port;

    private byte[] _connectionData;
    private System.Guid _allocationID;

    public async Task<Allocation> CreateRelay()
    {

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); // số lượng người chơi 4 - 1
        _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        _ip = dtlsEnpoint.Host;
        _port = dtlsEnpoint.Port;
        _allocationID = allocation.AllocationId;
        _connectionData = allocation.ConnectionData;
        return allocation;

    }

    public async Task<JoinAllocation> JoinRelay(string code){
        _joinCode = code;
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);
        
        RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        _ip = dtlsEnpoint.Host;
        _port = dtlsEnpoint.Port;
        _allocationID = allocation.AllocationId;
        _connectionData = allocation.ConnectionData;
        return allocation;
    }

    public string GetAllocation()
    {
        return _allocationID.ToString();
    }

    public string GetConnectionData()
    {
        return _connectionData.ToString();
    }
}