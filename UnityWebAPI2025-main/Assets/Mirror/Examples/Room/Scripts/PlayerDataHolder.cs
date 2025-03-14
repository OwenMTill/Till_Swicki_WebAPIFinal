using Mirror.Examples.NetworkRoom;
using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerDataHolder : MonoBehaviour
{
    public List<PlayerData> data;
    public GameObject roomManager;

    public int maxIndex = 0;
    public string username =" ";
    public int score =0 ;
    public int highScore = 0;
    public int gamePlayed = 0;
    public int win = 0;
    public int loss = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        PlayerDataHolder[] instances = GameObject.FindObjectsOfType<PlayerDataHolder>();
        if(instances.Length >1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        data = new List<PlayerData>();
    }

    private void Update()
    {
        //roomManager = GameObject.Find("NewworkRoomManager");
        maxIndex = roomManager.GetComponent<NetworkRoomManagerExt>().clientIndex;
        if (data != null) 
        {
            for (int i = 0; i < maxIndex; i++)
            {
                username = data[i].username;
                score = data[i].score;
                highScore = data[i].highscore;
                gamePlayed = data[i].gamesplayed;
                win = data[i].win;
                loss = data[i].loss;
            }
        }
        
        
    }
}
