using UnityEngine;
using Mirror;
using System;
using TMPro;

namespace Mirror.Examples.NetworkRoom
{
    public class GameTimer : NetworkBehaviour
    {
        [SyncVar] public float timeRemaining = 60.0f;

        public TMP_Text timerText;

        public TMP_Text winnerText;


        // Update is called once per frame
        void Update()
        {
            if (!isServer) return;

            timeRemaining -= Time.deltaTime;

            timerText.text = Mathf.RoundToInt(timeRemaining).ToString();

            winnerText.gameObject.SetActive(false);

            if (timeRemaining <= 0)
            {
                EndGame();
                timeRemaining = 0;
            }
        }

        [Server]
        private void EndGame()
        {
            GameObject winner = null;
            uint highestScore = uint.MinValue;
            int winnerIndex = 0;

            winnerText.gameObject.SetActive(true);

            PlayerScore[] players = FindObjectsByType<PlayerScore>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                if (player.score > highestScore)
                {
                    highestScore = player.score;
                    winner = player.gameObject;
                    winnerIndex = player.index;
                }

                player.gameObject.GetComponent<CharacterController>().enabled = false;
            }

            RPCShowWin(winnerIndex);
        }

        [ClientRpc]
        void RPCShowWin(int index)
        {
            Debug.Log("Player " + index + " WINS!!!");
            winnerText.text = "Player " + index + " WINS!!!";
        }
    }
}