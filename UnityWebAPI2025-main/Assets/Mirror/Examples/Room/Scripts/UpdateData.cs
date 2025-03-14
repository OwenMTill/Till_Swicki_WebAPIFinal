using UnityEngine;

public class UpdateData : MonoBehaviour
{
    GameObject playerData;
    PlayerDataHolder holder;
    public UpdatePlayerData update;
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

    public void UpdatePlayerData()
    {
        update.SetupPlayerData(holder.data.username,
                             holder.data.score,
                             holder.data.highscore,
                             holder.data.gamesplayed,
                             holder.data.win,
                             holder.data.loss);
    }
}
