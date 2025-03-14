using UnityEditor.UI;
using UnityEngine;

public class PlayerDataHolder : MonoBehaviour
{
    public PlayerData data;
    
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
    
    private void Update()
    {
        if(data != null)
        {
            username = data.username;
            score = data.score;
            highScore = data.highscore;
            gamePlayed = data.gamesplayed;
            win = data.win;
            loss = data.loss;
        }
        
    }
}
