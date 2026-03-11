using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardHandler : MonoBehaviour
{
    // UPDATE THIS URL every time you restart ngrok unless you have a static domain
    [Header("Configuration")]
    public string ngrokBaseUrl = "https://ossicular-overcherished-wynell.ngrok-free.dev";
    public PlayerData[] sortedPlayers;
    public PlayerData extraPlayer;
    int gate;
    int playergate;
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private TMP_Text pc_Text;
    string leaderboardText;
    // Data structure matching your Java 'Player' entity
    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public int highScore;
        public int gateNumber;
    }

    // Wrapper needed because JsonUtility cannot handle top-level JSON arrays [...]
    [System.Serializable]
    public class PlayerListWrapper
    {
        public List<PlayerData> players;
    }

    public void Start()
    {
        gate = int.Parse(PlayerPrefs.GetString("FlightNo"));
        pc_Text.SetText("Player count = ");
        StartCoroutine(GetPlayerCount(gate));
    }
    private IEnumerator GetPlayerCount()
    {
        throw new NotImplementedException();
    }

    public void postScore()
    {
        gate = int.Parse(PlayerPrefs.GetString("FlightNo"));
        GetLeaderboard(gate);

    }

    #region POST Score
    public void UploadScore(string name, int score, int gateNumber)
    {
        StartCoroutine(PostScoreRoutine(name, score, gateNumber));
    }

    private IEnumerator PostScoreRoutine(string playerName, int playerScore, int gateNumber)
    {
        string fullUrl = $"{ngrokBaseUrl}/api/players/score";

        PlayerData data = new PlayerData
        {
            username = playerName,
            highScore = playerScore,
            gateNumber = gateNumber
        };
        extraPlayer = data;
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        Debug.Log(bodyRaw);
        using (UnityWebRequest www = new UnityWebRequest(fullUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            // Set Headers
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("ngrok-skip-browser-warning", "true");

            Debug.Log($"[POST] Sending to: {fullUrl}");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[POST] Failed ({www.responseCode}): {www.error}\nCheck if your Java server is running and @PostMapping(\"/score\") exists.");
            }
            else
            {
                Debug.Log($"[POST] Success: {www.downloadHandler.text}");
            }
        }
    }
    #endregion

    #region GET Leaderboard
    public void GetLeaderboard(int gateNumber)
    {
        StartCoroutine(FetchTopTenRoutine(gateNumber));
    }

    private IEnumerator FetchTopTenRoutine(int gateNumber)
    {
        // Path variable {gateNumber} matches your @GetMapping("/leaderboard/{gateNumber}")
        string fullUrl = $"{ngrokBaseUrl}/api/players/leaderboard/{gateNumber}";

        using (UnityWebRequest www = UnityWebRequest.Get(fullUrl))
        {
            www.SetRequestHeader("ngrok-skip-browser-warning", "true");
            www.SetRequestHeader("Accept", "application/json");

            Debug.Log($"[GET] Requesting: {fullUrl}");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[GET] Failed ({www.responseCode}): {www.error}\nTry visiting the URL in your browser to verify it exists.");
            }
            else
            {
                // Unity Fix: Wrap array response [...] into an object {"players": [...]}
                string rawJson = www.downloadHandler.text;
                string wrappedJson = "{\"players\":" + rawJson + "}";

                try
                {
                    PlayerListWrapper wrapper = JsonUtility.FromJson<PlayerListWrapper>(wrappedJson);
                    wrapper.players.Add(extraPlayer);
                    // SORTING: OrderByDescending puts highest score at index [0]
                    // Then we save it to the class-level array
                    sortedPlayers = wrapper.players
                        .OrderByDescending(p => p.highScore)
                        .ToArray();


                    Debug.Log($"[GET] Success! Stored {sortedPlayers.Length} players sorted by score.");
                    leaderboardText = "Gate " + gateNumber + " Leaderboard\n";
                    // Now you can easily check bounds or loop through them
                    foreach (var p in sortedPlayers)
                    {
                        Debug.Log($"Ranked: {p.username} | Score: {p.highScore}");
                        leaderboardText += "Name: " + p.username + " " + " Score: ";
                        leaderboardText += p.highScore;
                        leaderboardText += "\n";
                    }
                    leaderboardText += "\nClick to exit to menu";
                    m_Text.alignment = TextAlignmentOptions.Top;
                    m_Text.SetText(leaderboardText);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                }
            }
        }
    }
    #endregion
    private IEnumerator GetPlayerCount(int gateNumber)
    {
        string fullUrl = $"{ngrokBaseUrl}/api/players/count/{gateNumber}";

        using (UnityWebRequest www = UnityWebRequest.Get(fullUrl))
        {
            www.SetRequestHeader("ngrok-skip-browser-warning", "true");
            // Use text/plain or application/json depending on what the server sends
            www.SetRequestHeader("Accept", "*/*");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[COUNT] Failed: {www.error}");
            }
            else
            {
                string responseText = www.downloadHandler.text;

                // Try to parse the plain string directly to a long
                if (long.TryParse(responseText, out long count))
                {
                    Debug.Log($"Total players at gate {gateNumber}:  {count}");
                    pc_Text.SetText($"Total players at gate {gateNumber}:  {count+1}");
                }
                else
                {
                    Debug.LogError($"Could not parse count from: {responseText}");
                }
            }
        }
    }
}