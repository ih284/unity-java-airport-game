using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InputFieldManager : MonoBehaviour {
    [SerializeField] private TMP_Text hello;
    [SerializeField] private TMP_Text sayFlightNo;

    public void GetUserName(string newUsername)
    {
        PlayerPrefs.SetString("userName", newUsername);
    }
    public void GetFlightNo(string newFlightNo)
    {
        PlayerPrefs.SetString("FlightNo", newFlightNo);
    }
    public void DisplayInfo()
    {
        if (PlayerPrefs.GetString("userName") != ""){
        hello.text = "Welcome, " + PlayerPrefs.GetString("userName") + "!";}
        else {hello.text = "Welcome!";}
    }
    public void DisplayFlightInfo()
    {
        if(PlayerPrefs.GetString("FlightNo") != ""){
        sayFlightNo.text = "You're in gate " + PlayerPrefs.GetString("FlightNo") + "!";}
        else {sayFlightNo.text = "";}
        }
    }
