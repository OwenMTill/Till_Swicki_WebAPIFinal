using Mirror;
using UnityEngine;

public class NetworkPlayerUsername : NetworkRoomPlayer
{
    public GameObject clientPlayerData;
    [SyncVar]
    public string username;

    [SyncVar]
    public uint gamePlayed;

    [SyncVar]
    public uint win;

    [SyncVar]
    public uint loss;
    private void Start()
    {
        clientPlayerData = GameObject.Find("DataHolder");
    }
    public override void OnGUI()
    {
        username = clientPlayerData.GetComponent<PlayerDataHolder>().data.username;
        gamePlayed = (uint)clientPlayerData.GetComponent<PlayerDataHolder>().data.gamesplayed;
        win = (uint)clientPlayerData.GetComponent<PlayerDataHolder>().data.win;
        loss = (uint)clientPlayerData.GetComponent<PlayerDataHolder>().data.loss;
        base.OnGUI();
        DrawPlayerReadyState();
    }

    void DrawPlayerReadyState()
    {
        GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

        GUILayout.Label($"{username}");

        if (readyToBegin)
            GUILayout.Label("Ready");
        else
            GUILayout.Label("Not Ready");

        if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
        {
            // This button only shows on the Host for all players other than the Host
            // Host and Players can't remove themselves (stop the client instead)
            // Host can kick a Player this way.
            GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        }

        GUILayout.EndArea();
    }

    void DrawPlayerReadyButton()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));

            if (readyToBegin)
            {
                if (GUILayout.Button("Cancel"))
                    CmdChangeReadyState(false);
            }
            else
            {
                if (GUILayout.Button("Ready"))
                    CmdChangeReadyState(true);
            }

            GUILayout.EndArea();
        }
    }
}
