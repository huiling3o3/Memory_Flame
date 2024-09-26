using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_TimeCounter : MonoBehaviour
{
    //UI
    [Header("[Amt. of time_stops left]")]
    public GameObject TimeStopCountUI;
    public TextMeshProUGUI TimeStopCountText;
    private int TimeStopValue;

    [Header("[Remaining time before Resume]")]
    public Slider TimerBar;
    [Header("[Gradient to adjust color val]")]
    public Gradient gradient;
    public Image fill;
    private int TimerCountdownValue;

    // Ref Time Manager script
    private GameObject timeManager;
    //private TimeManager timeManagerScript;

    // Ref Game Manager script
    private GameObject gameManager;
    //private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        // initialising Time Manager to get the countdown values
        timeManager = GameObject.Find("TimeManager");
        //timeManagerScript = timeManager.GetComponent<TimeManager>();

        // initialising Game Manager to get the countdown values
        gameManager = GameObject.Find("GameManager");
        //gameManagerScript = gameManager.GetComponent<GameManager>();

        //Set the max value of the time countdown to the slider
        //Debug.Log("Get countdown timer max value: " + timeManagerScript.GetTotalCountdownTime());
        //TimerBar.maxValue = timeManagerScript.GetTotalCountdownTime();
        fill.color = gradient.Evaluate(1f);


        //if (gameManagerScript.CheckScene() > 2) // if level is not tutorial 
        //{
        //    //hide the time countdown bar
        //    TimerBar.gameObject.SetActive(false);
        //    // set everything true
        //    TimeStopCountUI.SetActive(true);

        //}
        //else
        //{
        //    // if tutorial set everything false
        //    TimerBar.gameObject.SetActive(false);
        //    TimeStopCountUI.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // Set the values from time manager first
        SetTimeStopCount();
        SetTimerCountdown();

        // update ui
        UpdateTimerCountdownDisplay();
        UpdateTimeStopCountDisplay();
    }

    public void TurnTimeStopCountUIOn()
    {
        TimeStopCountUI.SetActive(true);
    }

    public void TurnCountdownUIOn()
    {

        TimerBar.gameObject.SetActive(true);
    }

    public void TurnCountdownUIOff()
    {
        TimerBar.gameObject.SetActive(false);
    }

    // set value of amt of time stop left
    void SetTimeStopCount()
    {
        //TimeStopValue = timeManagerScript.GetAbilityCurrentUsage();
    }

    // set value of timer countdown
    void SetTimerCountdown()
    {
        //TimerCountdownValue = timeManagerScript.GetAbilityCountdownTime();      
    }

    void UpdateTimeStopCountDisplay()
    {
        // update the ui text to display the timer in seconds
        TimeStopCountText.text =  "x" + TimeStopValue.ToString();
    }

    void UpdateTimerCountdownDisplay()
    {
        // Update the UI text to display the timer in seconds
        TimerBar.value = TimerCountdownValue;
        fill.color = gradient.Evaluate(TimerBar.normalizedValue);
    }
}
