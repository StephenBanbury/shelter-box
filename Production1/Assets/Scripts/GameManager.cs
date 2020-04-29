
using System;
using System.Collections;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Tooltip("Heads-up display countdown timer text")]
    [SerializeField] private Text hudCountdownDisplay;
    [Tooltip("Heads-up display canvas")]
    [SerializeField] private GameObject hudCanvas;
    [Tooltip("Heads-up display text")]
    [SerializeField] private TMP_Text hudTextMesh;
    [SerializeField] private GameObject scorePanel;
    [Tooltip("Minutes allowed for game countdown")]
    [SerializeField] private int timeAllowed = 5;
    [Tooltip("Personalised message displayed in control room")]
    [SerializeField] private Text personalMessage;
    [SerializeField] private TMP_Text startButtonText;
    [SerializeField] private int startingBudget = 1000;
    [SerializeField] private Text pendingOpsText;
    [SerializeField] private Text successfulOpsText;
    [SerializeField] private Text failedOpsText;

    [SerializeField] private AudioSource lowFundsWarning;
    [SerializeField] private AudioSource squelchbeep;
    [SerializeField] private AudioSource gong;
    [SerializeField] private AudioSource successfulDeployment;
    [SerializeField] private AudioSource notEnoughMoneyLeft;
    [SerializeField] private AudioSource missionStatementPart1;
    [SerializeField] private AudioSource missionStatementPart2;
    [SerializeField] private AudioSource useKeyboard;
    [SerializeField] private AudioSource backgroundNoise1;
    [SerializeField] private AudioSource operationFailure;

    [Tooltip("Initial countdown setting for resource objects (seconds)")]
    public float initialResourceObjectCountdown;

    //[SerializeField] private GameObject entrance;

    public TMP_Text BudgetMeter;
    public TMP_Text ScoreMeter;

    // static DeploymentStatus deploymentStatus;
    public static bool countdownStarted;


    private static float hudDisplayTime;
    private static float countdown;
    private bool updatingResourceCountdown;
    private bool updatingFundRaisingEvent;

    private Scene scene;

    private int remainingBudget;
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
        remainingBudget = startingBudget;

        //AudioSource[] audioSources = GetComponents<AudioSource>();

        //budgetLow = audioSources[0];
        //squelchbeep = audioSources[1];
        //gong = audioSources[2];
        //missionStatementPart2 = audioSources[3];
        //notEnoughMoneyLeft = audioSources[4];
        //missionStatementPart1 = audioSources[5];
        //successfulDeployment = audioSources[6];
        //useKeyboard = audioSources[7];
        //backgroundNoise1 = audioSources[8];

        fundingEventLives = new List<string>();
        for (int i = 1; i <= FundRaisingEventManager.instance.numberOfEventsAllowed; i++)
        {
            fundingEventLives.Add($"Event{i}");
        }

        //Debug.Log(fundingEventLives);

        const bool startupConditions = true;

        HudOnOff(startupConditions);

        ScorePanelOnOff(startupConditions);


        AnimationManager.instance.ActivateMonitor("Monitor1", startupConditions);
        AnimationManager.instance.ActivateMonitor("Monitor2", startupConditions);
        AnimationManager.instance.ActivateMonitor("Monitor3", startupConditions);
        AnimationManager.instance.ActivateMonitor("Monitor4", startupConditions);
        AnimationManager.instance.BoxesThruFloor(startupConditions);
        
        OperationsManager.instance.SetRotateOperations(startupConditions);

        CurrentOpsShowHide(startupConditions);

        StartCountdown();

        UpdateBudgetDisplay();

        UpdateFundingEventLives();
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
            
            FundraisingCountdownEvent(seconds, 8);
            
            if (countdown <= hudDisplayTime && hudDisplayTime != 0)
            {
                HudOnOff(false);
                hudTextMesh.text = "";
                hudDisplayTime = 0;
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER!");

    }

    public int BudgetRemaining()
    {
        return remainingBudget;
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
            case "lowFundsWarning":
                lowFundsWarning.Play();
                break;
            case "operationFailure":
                operationFailure.Play();
                break;
        }
    }

    public void HudMessage(string messageText, int displayTimeSeconds)
    {
        hudDisplayTime = countdown - displayTimeSeconds;
        HudOnOff(true);
        hudTextMesh.text = messageText;
    }

    public void CurrentOpsShowHide(bool show)
    {
        float alpha = show ? 1f : 0;
        var currentOps = GameObject.Find("CurrentOperations");
        currentOps.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void HudOnOff(bool on)
    {
        hudCanvas.SetActive(on);
    }

    public void ScorePanelOnOff(bool on)
    {
        scorePanel.SetActive(on);
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
            //Debug.Log($"fundingEventLives: {fundingEventLives[i-1]}");
            var lifeObject = GameObject.Find(fundingEventLives[i-1]);
            var lifeObjectColor = i <= numberOfEventLivesLeft ? new Color(240, 255, 0, 255) : new Color(190, 205, 207, 255);

            lifeObject.GetComponent<Renderer>().material.color = lifeObjectColor;
        }
    }

    public void OpsStatusText(string pending, string success, string failed)
    {
        pendingOpsText.text = pending;
        successfulOpsText.text = success;
        failedOpsText.text = failed;
    }

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
            //var rand = Random.value;
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

    public void ReduceBudget(int value)
    {
        Debug.Log($"Reduce budget by: {value}");
        remainingBudget -= value;
        UpdateBudgetDisplay();
    }

    public void IncreaseBudget(int value)
    {
        Debug.Log($"Increase budget by: {value}");
        remainingBudget += value;
        UpdateBudgetDisplay();
    }

    public void UpdateBudgetDisplay()
    {
        // TODO try to get CultureInfo.CurrentCulture to work - on Android build £ becomes something else!
        //BudgetMeter.text = $"{BudgetRemaining.ToString("C", CultureInfo.CurrentCulture).Replace(".00", "")}";
        BudgetMeter.text = $"£{remainingBudget.ToString().Replace(".00", "")}";

        var percentRemaining = (float)((float)remainingBudget / (float)startingBudget) * 100;
        
        if (percentRemaining <= 10)
        {
            PlayAudio("lowFundsWarning");
            //StartCoroutine(BudgetWarning(3));
            HudMessage("WARNING: Low funds!!", 4);
        }

        IndicateBudget(percentRemaining);
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

    private void IndicateBudget(float percentRemaining)
    {
        for (var i = 1; i <= 5; i++)
        {
            var colour = new Color(0, 0, 0, 0);

            // colour indicator light if within budget remaining
            if (percentRemaining >= i * 16.7)
            {
                colour = new Color(240, 255, 0, 255);
            }
            GameObject.Find($"BudgetLife{i}").GetComponent<Renderer>().material.color = colour;
        }
    }

    //private IEnumerator BudgetWarning(int secondsDelay)
    //{
    //    //LightUpAllBudgetLights();
    //    PlayAudio("lowFundsWarning");
    //    yield return new WaitForSeconds(secondsDelay);
    //}

    //private void LightUpAllBudgetLights()
    //{
    //    var colour = new Color(255, 0, 0, 255);
    //    for (var i = 1; i <= 5; i++)
    //    {
    //        GameObject.Find($"BudgetLife{i}").GetComponent<Renderer>().material.color = colour;
    //    }
    //}

    private void UpdateScoreDisplay()
    {
        ScoreMeter.text = score.ToString();
    }

    private IEnumerator WaitForGameStart(int secondsDelay)
    {
        //PlayAudio("backgroundNoise1");

        yield return new WaitForSeconds(secondsDelay);
        
        //PlayAudio("missionStatementPart1");
        //AnimationManager.instance.FadeFireCurtain(true);
        //AnimationManager.instance.LowerStartButton(true);
    }

    #endregion

}
