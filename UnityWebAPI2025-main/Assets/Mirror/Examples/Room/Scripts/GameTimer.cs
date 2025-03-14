using UnityEngine;
using Mirror;
using System;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

namespace Mirror.Examples.NetworkRoom
{
    public class GameTimer : NetworkBehaviour
    {
        [SyncVar] public float timeRemaining = 60.0f;

        public TMP_Text winnerText;

        public GameObject routes;
        GameObject playerData;
        PlayerDataHolder holder;
        private void Awake()
        {
            playerData = GameObject.Find("DataHolder");
            holder = playerData.GetComponent<PlayerDataHolder>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isServer) return;

            timeRemaining -= Time.deltaTime;


            winnerText.text = "";
            winnerText.gameObject.SetActive(false);

            if (timeRemaining <= 0)
            {
                EndGame();
                timeRemaining = 0;
            }
        }

        void OnGUI()
        {
            GUI.Box(new Rect(Screen.width-100f, 50f, 50f, 25f), $"{Mathf.RoundToInt(timeRemaining)}");
        }

        [Server]
        private void EndGame()
        {
            GameObject winner = null;
            uint highestScore = uint.MinValue;
            string winnerName = "";

            winnerText.gameObject.SetActive(true);

            PlayerScore[] players = FindObjectsByType<PlayerScore>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                if (player.score >= highestScore)
                {
                    highestScore = player.score;
                    winner = player.gameObject;
                    winnerName = player.username;
                    Debug.Log("Winner name: " + winnerName);
                }

                player.gameObject.GetComponent<CharacterController>().enabled = false;
            }
            foreach (var player in players)
            {
                PlayerData tempPlayer = new PlayerData();
                Debug.Log("Player index:" + player.index);
                routes.GetComponent<FindPlayerData>().SearchForPlayerWithPlayerScore(player.index);

                Debug.Log("Games played" + routes.GetComponent<FetchData>().player.gamesplayed);
                tempPlayer = routes.GetComponent<FetchData>().player;

                //tempPlayer = playerData.GetComponent<PlayerDataHolder>().data;
                Debug.Log(tempPlayer.gamesplayed);
                tempPlayer.gamesplayed = (int)player.gamePlayed + 1;
                tempPlayer.score = (int)player.score;
                if (player.username != winnerName)
                {
                    tempPlayer.loss = (int)player.loss + 1;
                }
                else
                {
                    tempPlayer.win = (int)player.win + 1;
                }
                Debug.Log("Player name before the update: "+tempPlayer.username);
                routes.GetComponent<UpdatePlayerData>().SetupPlayerData(tempPlayer.username, tempPlayer.score, tempPlayer.highscore, tempPlayer.gamesplayed, tempPlayer.win, tempPlayer.loss);
            }
            RPCShowWin(winnerName);
        }

        [ClientRpc]
        void RPCShowWin(string name)
        {
            Debug.Log(name + " WINS!!!");
            winnerText.text = name  + " WINS!!!";
        }
    }
}