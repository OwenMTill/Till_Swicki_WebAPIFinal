using UnityEngine;
using Mirror;

public class SpawnManager : NetworkManager
{
    public Transform[] spawnPoints;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    Transform GetRandomSpawnPoint()
    {
        if(spawnPoints.Length == 0)
        {
            return transform;
        }
        return spawnPoints[(int)Mathf.Floor(Random.Range(0, spawnPoints.Length))];
    }
}
