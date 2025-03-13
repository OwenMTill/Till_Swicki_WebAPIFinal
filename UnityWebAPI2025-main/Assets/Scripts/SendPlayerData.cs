using TMPro;
using UnityEngine;

public class SendPlayerData : MonoBehaviour
{
    public TMP_InputField name;
    public TMP_InputField score;
    public TMP_InputField level;
    public PostData post;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendData()
    {
        if(level.text != "" && name.text != "" && score.text != "")
        {
            int scoreData = int.Parse(score.text);
            int levelData = int.Parse(level.text);
            post.SetupPlayerData(name.text, scoreData,levelData);
        }
    }
}
