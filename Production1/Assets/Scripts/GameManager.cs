
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Com.MachineApps.PrepareAndDeploy;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Services;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Tooltip("Heads-up display countdown timer text")]
    [SerializeField] private Text hudCountdownDisplay;

    [Tooltip("Heads-up display text")]
    [SerializeField] private Text hudText;

    [Tooltip("Minutes allowed for game countdown")]
    [SerializeField] private int timeAllowed = 5;

    [Tooltip("Personalised message displayed in control room")]
    [SerializeField] private Text personalMessage;

    [SerializeField] private GameObject hudCanvas;

    [SerializeField] private TMP_Text startButtonText;

    [Tooltip("Initial countdown setting for resource objects (seconds)")]
    public float initialResourceObjectCountdown;

    //[SerializeField] private GameObject entrance;
    public int BudgetRemaining = 1000;
    public TMP_Text BudgetMeter;
    public TMP_Text ScoreMeter;

    // static DeploymentStatus deploymentStatus;
    public static bool countdownStarted;

    public string playerName;

    private static float hudDisplayTime;
    private static float countdown;
    private bool updatingResourceCountdown;
    private bool updatingFundRaisingEvent;

    private AudioSource horn;
    private AudioSource squelchbeep;
    private AudioSource gong;
    private AudioSource successfulDeployment;
    private AudioSource notEnoughMoneyLeft;
    private AudioSource missionStatementPart1;
    private AudioSource missionStatementPart2;
    private AudioSource useKeyboard;
    private AudioSource backgroundNoise1;

    private Scene scene;
    
    private List<string> fundingEventLives;

    private int score = 0;

    void Awake()
    {
        // Check that it exists
        if (instance == null)
        {
            //assign it to the current object
            instance = this;
        }

        // make sure that it is equal to the current object
        else if (instance != this)
        {
            // Destroy current game object - we only need one and we already have it
            Destroy(gameObject);
        }

        // don't destroy the object when changing scenes!
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        AudioSource[] audioSources = GetComponents<AudioSource>();

        horn = audioSources[0];
        squelchbeep = audioSources[1];
        gong = audioSources[2];
        missionStatementPart2 = audioSources[3];
        notEnoughMoneyLeft = audioSources[4];
        successfulDeployment = audioSources[6];
        missionStatementPart1 = audioSources[5];
        useKeyboard = audioSources[7];
        backgroundNoise1 = audioSources[8];

        fundingEventLives = new List<string>();
        for (int i = 1; i <= FundRaisingEventManager.instance.numberOfEventsAllowed; i++)
        {
            fundingEventLives.Add($"Event{i}");
        }

        //Debug.Log(fundingEventLives);

        HudOnOff(false);

        AnimationManager.instance.ActivateMonitor("monitor1", false);
        AnimationManager.instance.ActivateMonitor("monitor2", false);
        AnimationManager.instance.ActivateMonitor("monitor3", false);
        AnimationManager.instance.ActivateMonitor("monitor4", false);

        StartCountdown();

        UpdateBudgetDisplay();

        UpdateFundingEventLives();

        StartButtonText("Yes!\n Ready to help");

        StartCoroutine(WaitForGameStart(5));
    }

    void FixedUpdate()
    {
        if (countdownStarted)
        {
            countdown -= Time.deltaTime;

            float minutes = Mathf.Floor(countdown / 60);
            float seconds = countdown % 60;

            // TODO remove timer display - Debugging only
            hudCountdownDisplay.text = $"{minutes:0}:{seconds:00}";

            ReduceResourceCountdownStart(seconds, 10, 0.3f);
            
            FundraisingCountdownEvent(seconds, 5);
            
            if (countdown <= hudDisplayTime && hudDisplayTime != 0)
            {
                hudText.text = "";
                hudDisplayTime = 0;
            }
        }
    }
    
    public void StartCountdown()
    {
        countdown = (timeAllowed * 60);
        countdownStarted = true;
    }

    public string CurrentScene()
    {
        return scene.name;
    }

    public void LoadAppropriateScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //public DeploymentStatus GetDeploymentStatus()
    //{
    //    return deploymentStatus;
    //}

    public void PlayAudio(string audioFile)
    {
        switch (audioFile)
        {
            case "notEnoughMoneyLeft":
                notEnoughMoneyLeft.Play();
                break;
            case "successfulDeployment":
                successfulDeployment.Play();
                break;
            case "missionStatementPart1":
                missionStatementPart1.Play();
                break;
            case "missionStatementPart2":
                missionStatementPart2.Play();
                break;
            case "useKeyboard":
                useKeyboard.Play();
                break;
            case "backgroundNoise1":
                backgroundNoise1.loop = true;
                backgroundNoise1.volume = 0.2f;
                backgroundNoise1.Play();
                break;
        }
    }

    

    public void HudMessage(string messageText, int displayTimeSeconds)
    {
        hudDisplayTime = countdown - displayTimeSeconds;
        hudText.text = messageText;
    }

    public void HudOnOff(bool on)
    {
        hudCanvas.SetActive(on);
    }

    public void PersonalMessage(string playerName)
    {
        var message = $"Welcome {playerName}. \r\nThank you for your assistance.";
        personalMessage.text = message;
    }

    public void StartButtonText(string text)
    {
        startButtonText.text = text;
    }

    public void UpdateFundingEventLives()
    {
        var numberOfEventLivesLeft = 
            FundRaisingEventManager.instance.numberOfEventsAllowed - FundRaisingEventManager.instance.numberOfEventsUsed;

        for (int i = 1; i <= fundingEventLives.Count; i++)
        {
            Debug.Log($"fundingEventLives: {fundingEventLives[i-1]}");

            var lifeObject = GameObject.Find(fundingEventLives[i-1]);
            var lifeObjectColor = i <= numberOfEventLivesLeft ? new Color(255, 0, 0, 255) : new Color(190, 205, 207, 255);

            lifeObject.GetComponent<Renderer>().material.color = lifeObjectColor;
        }

    }


    //public void GameOver()
    //{
    //    StartCoroutine(EndGameAfterDelay(10));
    //}

    //IEnumerator EndGameAfterDelay(int seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    Application.Quit();
    //}

    #region Budget

    private void ReduceResourceCountdownStart(float seconds, int regularity, float reduction)
    {
        // Reduce countdown start for resources before timeout when grabbed
        if (Math.Floor(seconds) % regularity == 0 && !updatingResourceCountdown)
        {
            initialResourceObjectCountdown -= reduction;
            updatingResourceCountdown = true;
        }
        else if (Math.Floor(seconds) % regularity > 0)
        {
            updatingResourceCountdown = false;
        }
    }

    private void FundraisingCountdownEvent(float seconds, int regularity)
    {
        if (Math.Floor(seconds) % regularity == 0 && !updatingFundRaisingEvent)
        {
            // Chance of showing next fund-raising event
            // % 50 percent chance
            var rand = Random.value;
            //bool showNextEvent = rand > 0.5;

            bool showNextEvent = true;

            if (showNextEvent)
            {
                FundRaisingEventManager.instance.NextEvent();
            }

            updatingFundRaisingEvent = true;
        }
        else if (Math.Floor(seconds) % regularity > 0)
        {
            updatingFundRaisingEvent = false;
        }

    }

    public void UpdateBudgetDisplay()
    {
        BudgetMeter.text = $"Remaining\n{BudgetRemaining.ToString("C", CultureInfo.CurrentCulture).Replace(".00", "")}";
    }

    public void ReduceBudget(int value)
    {
        Debug.Log($"Reduce budget by: {value}");
        BudgetRemaining -= value;
        UpdateBudgetDisplay();
    }

    public void IncreaseBudget(int value)
    {
        Debug.Log($"Increase budget by: {value}");
        BudgetRemaining += value;
        UpdateBudgetDisplay();
    }

    private void UpdateScoreDisplay()
    {
        ScoreMeter.text = score.ToString();
    }

    public void UpdateScore(int value)
    {
        Debug.Log($"Update score by: {value}");
        score += value;
        UpdateScoreDisplay();
    }

    public int GetResourceCost(Resource resource)
    {
        var fundRaisingEventService = new FundRaisingEventService();
        var resourceCost = fundRaisingEventService.ResourceCosts();
        var cost = resourceCost[resource];

        return cost;
    }

    #endregion

    #region Private methods

    private IEnumerator WaitForGameStart(int secondsDelay)
    {
        //PlayAudio("backgroundNoise1");

        yield return new WaitForSeconds(secondsDelay);
        
        PlayAudio("missionStatementPart1");
        AnimationManager.instance.FadeFireCurtain(true);
        AnimationManager.instance.LowerStartButton(true);
    }

    #endregion

}
