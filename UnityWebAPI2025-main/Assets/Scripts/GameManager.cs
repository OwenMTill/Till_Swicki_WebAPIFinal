using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [SyncVar] public GameObject itPlayer;

    public override void OnStartClient()
    {
        //Checks if player is first in the instance
        if (NetworkServer.connections.Count == 1)
        {
            itPlayer = NetworkServer.connections[0].identity.gameObject;
            //Come back and set player to it
            itPlayer.GetComponent<PlayerTag>().isIt = true;
        }
    }
}
