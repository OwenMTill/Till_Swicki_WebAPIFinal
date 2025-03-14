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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerData = GameObject.Find("DataHolder");
        holder = playerData.GetComponent<PlayerDataHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendData()
    {
        post.SetupPlayerData(holder.data.username,
                             holder.data.score,
                             holder.data.highscore,
                             holder.data.gamesplayed,
                             holder.data.win,
                             holder.data.loss);
    }
}
