using Unity.Services.Lobbies.Models;

public static class LobbyEvents{
    public delegate void lobbyUpdated(Lobby lobby);

    public static  lobbyUpdated OnLobbyUpdated;


    public delegate void LobbyUpdateUI();

    public static LobbyUpdateUI lobbyUpdateUI;

}