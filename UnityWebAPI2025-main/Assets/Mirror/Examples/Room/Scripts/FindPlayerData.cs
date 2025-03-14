using Mirror.Examples.MultipleAdditiveScenes;
using Mirror.Examples.NetworkRoom;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindPlayerData : MonoBehaviour
{
    public TMP_InputField name;
    public TMP_InputField playerid;
    public FetchData fetch;
    [SerializeField]
    GameObject playerData;
    //GameObject playerScore;
    [SerializeField]
    Mirror.Examples.NetworkRoom.PlayerScore[] players;
    bool ranOnce = false;

    //private void Awake()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 2)
    //    {
    //        Debug.Log("AAAAAAAAAAAAAAAA");
    //        playerScores = GameObject.FindGameObjectsWithTag("Player");
    //        foreach (GameObject go in playerScores)
    //        {
    //            Debug.Log(go.name);
    //        }

    //    }
    //}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Scene num: " + SceneManager.GetActiveScene().buildIndex);
        playerData = GameObject.Find("DataHolder");
        if(playerData != null && playerData.GetComponent<PlayerDataHolder>().username != " ")
        {
            name.text = playerData.GetComponent<PlayerDataHolder>().data.username;
        }

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            players = FindObjectsByType<Mirror.Examples.NetworkRoom.PlayerScore>(FindObjectsSortMode.None);
            //playerScore = GameObject.Find("GamePlayerReliable");
            foreach (Mirror.Examples.NetworkRoom.PlayerScore go in players)
            {
                Debug.Log(go.name);
            }    
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 && !ranOnce)
        {
            
            players = FindObjectsByType<Mirror.Examples.NetworkRoom.PlayerScore>(FindObjectsSortMode.None);
            //playerScore = GameObject.Find("GamePlayerReliable");
            foreach (Mirror.Examples.NetworkRoom.PlayerScore go in players)
            {
                
            }
            if(players.Length >0)
            {
                ranOnce = true;
            }
        }
    }
    public void SearchForPlayer()
    {
        if (playerid.text != string.Empty)
        {
            fetch.SetupPlayerSearchData(name.text, playerid.text);
        }
        else
        {
            fetch.SetupPlayerSearchData(name.text);
        }
    }

    public void SearchForPlayerWithPlayerScore(int index)
    {
        fetch.SetupPlayerSearchData(players[index].GetComponent<Mirror.Examples.NetworkRoom.PlayerScore>().username);
    }
}
