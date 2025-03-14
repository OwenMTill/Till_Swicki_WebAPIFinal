using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UpdatePlayerData : MonoBehaviour
{
    string serverUrl = "http://localhost:3000/updatePlayer";
    PlayerData player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupPlayerData(string name, int score, int highscore, int gamesPlayed, int win, int loss)
    {
        player = new PlayerData();

        player.username = name;
        player.score = score;
        player.highscore = highscore;
        player.gamesplayed = gamesPlayed;
        player.win = win;
        player.loss = loss;

        string json = JsonUtility.ToJson(player);
        Debug.Log(json);
        StartCoroutine(UpdatePlayer(json));
    }

    IEnumerator UpdatePlayer(string json)
    {
        Debug.Log(player.username);
        string url = serverUrl + "/" + player.username;
        Debug.Log(url);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Web " + request.result);

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);
            //Success
            Debug.Log($"Data Sent: {request.downloadHandler.text}");

            string newPlayerId = ExtractUsername(response);
            Debug.Log("New player id:" + newPlayerId);
        }
        else
        {
            //failed
            Debug.LogError($"Error sending data: {request.error}");
        }
    }

    string ExtractUsername(string jsonResponse)
    {
        int index = jsonResponse.IndexOf("\"username\":\"") + 12;
        if (index < 12) return "";
        int endIndex = jsonResponse.IndexOf("\"", index);
        return jsonResponse.Substring(index, endIndex - index);
    }
}
