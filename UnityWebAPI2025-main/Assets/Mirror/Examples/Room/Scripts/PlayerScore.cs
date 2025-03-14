using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar]
        public int index;

        [SyncVar]
        public uint score;

        [SyncVar]
        public uint gamePlayed;

        [SyncVar]
        public uint win;

        [SyncVar]
        public uint loss;

        [SyncVar]
        public string username;

        void OnGUI()
        {
            GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"{username}: {score:0000000}");
        }
    }
}
