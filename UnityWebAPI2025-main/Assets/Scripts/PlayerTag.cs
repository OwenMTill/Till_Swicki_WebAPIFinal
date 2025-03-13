using UnityEngine;
using Mirror;
public class PlayerTag : NetworkBehaviour
{
    [SyncVar] public bool isIt = false;
    [SyncVar] public bool isFrozen = false;

    private void OnCollisionEnter(Collision other)
    {
        if (!isServer) return; //only the server handles tagging

        PlayerTag otherPlayer = other.gameObject.GetComponent<PlayerTag>();

        if(otherPlayer != null)
        {
            if(isIt && !otherPlayer.isFrozen)
            {
                otherPlayer.FreezePlayer();
                
            }
            else if (!isIt && isFrozen && !otherPlayer.isFrozen)
            {
                UnFreezePlayer();
               
            }
        }

    }


    [Server]
    public void FreezePlayer()
    {
        isFrozen = true;
        RPCUpdateState(isFrozen);
    }

    [Server]
    public void UnFreezePlayer()
    {
        isFrozen = false;
        RPCUpdateState(isFrozen);
    }

    [ClientRpc]
    void RPCUpdateState(bool frozen)
    {
        isFrozen = frozen;
        GetComponent<Renderer>().material.color = frozen ? Color.blue : Color.gray;
        GetComponent<PlayerController>().isFrozen = frozen;
    }
}
