using Mirror.Examples.MultipleAdditiveScenes;
using Mirror.Examples.NetworkRoom;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindPlayerData : MonoBehaviour
{
    public TMP_InputField name;
    public TMP_InputField playerid;
    public FetchData fetch;
    [SerializeField]
    public GameObject playerData;

    public NetworkRoomManagerExt roomManager;
    public int maxIndex = 0;
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
    private void Awake()
    {
        roomManager = GameObject.Find("NetworkRoomManager").GetComponent<NetworkRoomManagerExt>();
        maxIndex = roomManager.clientIndex;
    }
    void Start()
    {
        
        Debug.Log("Scene num: " + SceneManager.GetActiveScene().buildIndex);
        
        playerData = GameObject.Find("DataHolder");
        if(playerData != null && playerData.GetComponent<PlayerDataHolder>().username != " ")
        {
            for(int i = 0; i<maxIndex; i++)
            {
                name.text = playerData.GetComponent<PlayerDataHolder>().data[i].username;
            }
            
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
        //roomManager = GameObject.Find("NewworkRoomManager").GetComponent<NetworkRoomManagerExt>();
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            
            players = FindObjectsByType<Mirror.Examples.NetworkRoom.PlayerScore>(FindObjectsSortMode.None);
            //playerScore = GameObject.Find("GamePlayerReliable");
            foreach (Mirror.Examples.NetworkRoom.PlayerScore go in players)
            {
                Debug.Log(go.name);
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
            fetch.SetupPlayerSearchData(name.text, 0);
        }
    }

    public void SearchForPlayerWithPlayerScore(int index)
    {
        fetch.SetupPlayerSearchData(players[index].GetComponent<Mirror.Examples.NetworkRoom.PlayerScore>().username, index);
    }
}
