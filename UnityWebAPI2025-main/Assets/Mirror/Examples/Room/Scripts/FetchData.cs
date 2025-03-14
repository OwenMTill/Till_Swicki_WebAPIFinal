using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class FetchData : MonoBehaviour
{
    string serverUrl = "http://localhost:3000/player";
    List<PlayerData> playerList;
    public PlayerData player;
    GameObject playerData;
    public SendPlayerData send;

    private void Awake()
    {
        playerData = GameObject.Find("DataHolder");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartFetch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //Success
                string json = request.downloadHandler.text;
                Debug.Log($"Recieved the data: {json}");

                //Deserialize the data to use in unity
                //player = JsonUtility.FromJson<PlayerData>(json);
                playerList = JsonConvert.DeserializeObject<List<PlayerData>>(json);

                //Print out the player info
                //Debug.Log($"Name: {player.username}, Score: {player.score}, Level: {player.gamesplayed}");
                Debug.Log(playerList);
            }
            else
            {
                //Failed
                Debug.Log($"Error fetching data: {request.error}");
            }
        }     
    }

    public IEnumerator GetDataByID(string json, string playerid = "")
    {
        string url = serverUrl + "/" + playerid;
        Debug.Log(url);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log($"Success: {response}");

            player = JsonUtility.FromJson<PlayerData>(response);

                //Extract playerid
            string newPlayerId = ExtractPlayerId(response);
            if (!string.IsNullOrEmpty(newPlayerId))
            {
                Debug.Log("PlayerID: " + newPlayerId);
            }
            yield return null;
        }
        else 
        {
            //Handles Error
            Debug.Log("Error: " + request.error);
            yield return null;
        }
    }

    public IEnumerator GetDataByUsername(string json, string username = "")
    {
        string url = serverUrl + "/" + username;
        Debug.Log(url);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;

            player = JsonUtility.FromJson<PlayerData>(response);

            Debug.Log($"Username search Success: {response}");

            //Extract username
            string newUsername = ExtractUsername(response);

            SetPlayer();

            if (!string.IsNullOrEmpty(newUsername))
            {
                Debug.Log("Username: " + newUsername);
            }
            yield return null;
        }
        else
        {
            //Handles Error
            Debug.Log("Error: " + request.error);
            Debug.Log("Making new Player");
            send.SendData();
            yield return null;
        }
    }

    public void StartFetch()
    {
        StartCoroutine(GetData());
    }

    public void SetupPlayerSearchData(string name, string playerid)
    {
        player = new PlayerData();

        player.username = name;
        

        string json = JsonUtility.ToJson(player);
        Debug.Log(json);

        StartCoroutine(GetDataByID(json, playerid));
    }
    public void SetupPlayerSearchData(string name)
    {
        player = new PlayerData();

        player.username = name;

        string json = JsonUtility.ToJson(player);
        Debug.Log(json);

        StartCoroutine(GetDataByUsername(json, name));
    }

    public void SetPlayer()
    {
        playerData.GetComponent<PlayerDataHolder>().data = player;
    }

    string ExtractPlayerId(string jsonResponse)
    {
        int index = jsonResponse.IndexOf("\"playerid\":\"") + 12;
        if (index < 12) return "";
        int endIndex = jsonResponse.IndexOf("\"", index);
        return jsonResponse.Substring(index, endIndex - index);
    }
    string ExtractUsername(string jsonResponse)
    {
        int index = jsonResponse.IndexOf("\"username\":\"") + 12;
        if (index < 12) return "";
        int endIndex = jsonResponse.IndexOf("\"", index);
        return jsonResponse.Substring(index, endIndex - index);
    }
}

public class PlayerData 
{
    public string username;
    public int score;
    public int highscore;
    public int gamesplayed;
    public int win;
    public int loss;
}
