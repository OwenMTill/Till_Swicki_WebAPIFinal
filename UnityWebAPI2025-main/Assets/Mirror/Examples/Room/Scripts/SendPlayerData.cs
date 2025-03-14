using Mirror.Examples.NetworkRoom;
using TMPro;
using UnityEngine;

public class SendPlayerData : MonoBehaviour
{
    //public TMP_InputField name;
    //public TMP_InputField score;
    //public TMP_InputField level;
    GameObject playerData;
    PlayerDataHolder holder;
    public PostData post;
    public GameObject roomManager;
    NetworkRoomManagerExt room;

    void Awake()
    {
        roomManager = GameObject.Find("NetworkRoomManager");
        room = roomManager.GetComponent<NetworkRoomManagerExt>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        playerData = GameObject.Find("DataHolder");
        holder = playerData.GetComponent<PlayerDataHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        //roomManager = GameObject.Find("NewworkRoomManager").GetComponent<NetworkRoomManagerExt>();
    }

    public void SendData()
    {
        post.SetupPlayerData(holder.data[room.clientIndex].username,
                             holder.data[room.clientIndex].score,
                             holder.data[room.clientIndex].highscore,
                             holder.data[room.clientIndex].gamesplayed,
                             holder.data[room.clientIndex].win,
                             holder.data[room.clientIndex].loss);
    }
}
