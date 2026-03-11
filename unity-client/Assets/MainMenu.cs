using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour

{
    public void PlayGameA()
    {
        string flightNo = PlayerPrefs.GetString("FlightNo", "Not Set");
        string userName = PlayerPrefs.GetString("userName", "Not Set");
        Debug.Log($"FlightNo: {flightNo}, userName: {userName}");
        SceneManager.LoadSceneAsync("SampleScene");
    }
    public void PlayGameB()
    {
        SceneManager.LoadSceneAsync("GameB");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
}




// public class MainMenu : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
