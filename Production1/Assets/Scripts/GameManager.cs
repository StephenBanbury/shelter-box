
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Com.MachineApps.PrepareAndDeploy;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[Tooltip("Heads-up display countdown timer text")]
    //[SerializeField] private Text hudCountdownDisplay;
    [Tooltip("Heads-up display canvas")]
    [SerializeField] private GameObject hudCanvas;
    [Tooltip("Heads-up display text")]
    [SerializeField] private TMP_Text hudTextMesh;
    [Tooltip("Minutes allowed for game countdown")]
    [SerializeField] private int timeAllowed = 5;
    [Tooltip("Personalised message displayed in control room")]
    [SerializeField] private Text personalMessage;
    [SerializeField] private TMP_Text startButtonText;
    [SerializeField] private int startingBudget = 1000;

    [SerializeField] private Text pendingOpsText;
    [SerializeField] private Text successfulOpsText;
    [SerializeField] private Text failedOpsText;

    [SerializeField] private Text pendingFundraisersText;
    [SerializeField] private Text currentFundraiserText;
    [SerializeField] private Text completedFundraisersText;

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

    [Tooltip("Initialises in debug mode to allow viewing of certain conditions from the IDE")]
    [SerializeField] private bool debugStartSettings;
    
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject budgetLives;

    [SerializeField] private TMP_Text budgetText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private bool resetLeaderBoard;
    [SerializeField] private bool resetAllPlayerPrefs;

    [Tooltip("Collider to block exit once within action zone")]
    [SerializeField] private GameObject exitBlocker;

    [SerializeField] private HighscoreTable highscoresTable;
    [SerializeField] private InputManager inputManager;

    // static DeploymentStatus deploymentStatus;
    public static bool countdownStarted;

    private static float hudDisplayTime;

    private static float countdown;
    //private bool updatingResourceCountdown;
    private bool updatingFundRaisingEvent;

    private Scene scene;

    private int remainingBudget;
    private List<string> fundingEventLives;

    private int score = 0;
    private int highScore;

    private ScoreService scoreService;

    private ScoresRegister scoresRegister;

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

        //DontDestroyOnLoad(gameObject);

        scoreService = new ScoreService(10);
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        Debug.Log($"Scene: {scene.name}");

        if (scene.name == "PrepRoom")
        {
            remainingBudget = startingBudget;

            Resets();
            GetHighScore();
            UpdateScoreDisplay();
            UpdateHighscoreDisplay();
            InitialiseFundingEventLives();

            AnimationManager.instance.ActivateMonitor("Monitor1", false);
            AnimationManager.instance.ActivateMonitor("Monitor2", false);
            AnimationManager.instance.ActivateMonitor("Monitor3", false);
            AnimationManager.instance.ActivateMonitor("Monitor4", false);
            AnimationManager.instance.BoxesThruFloor(false);
            OperationsManager.instance.SetRotateOperations(false);

            HudOnOff(false);
            BudgetLivesOnOff(false);
            ScorePanelOnOff(false);
            CurrentOpsChartShowHide(false);
            FundraisingEventsChartShowHide(false);
            ActivateExitBlocker(false);

            StartCountdown();

            if (debugStartSettings) inputManager.EngageGame();
        }
    }

    void FixedUpdate()
    {
        StartCoroutine(CountdownTasks());
    }


    #region Public Methods

    public void GameOver(string reason)
    {
        Debug.Log($"GAME OVER: {reason}");
        
        var playerName = PlayerManager.instance.Player;
        scoreService.AddHighscoreEntry(score, playerName);

        StartCoroutine(GameOver(reason, 4));
    }
    
    public void UpdateScoresRegister(ScoreType scoreType)
    {
        switch (scoreType)
        {
            case ScoreType.ItemAssigned:
                scoresRegister.ItemAssigned++;
                break;
            case ScoreType.OperationSuccessful:
                scoresRegister.OperationSuccessful++;
                break;
            case ScoreType.FundsRaised:
                scoresRegister.FundsRaised++;
                break;
            case ScoreType.ItemDropped:
                scoresRegister.ItemDropped++;
                break;
            case ScoreType.ItemNotRequired:
                scoresRegister.ItemNotRequired++;
                break;
            case ScoreType.ItemAlreadyAssigned:
                scoresRegister.ItemAlreadyAssigned++;
                break;
            case ScoreType.GameSuccessfullyCompleted:
                scoresRegister.GameSuccessfullyCompleted++;
                break;
            case ScoreType.OperationFailed:
                scoresRegister.OperationFailed++;
                break;
        }
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

    public void ActivateExitBlocker(bool active)
    {
        exitBlocker.SetActive(active);
    }

    public void CurrentOpsChartShowHide(bool show)
    {
        float alpha = show ? 1f : 0;
        var currentOps = GameObject.Find("CurrentOperations");
        currentOps.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void FundraisingEventsChartShowHide(bool show)
    {
        float alpha = show ? 1f : 0;
        var fundingEvents = GameObject.Find("FundraisingEvents");
        fundingEvents.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void HudOnOff(bool on)
    {
        hudCanvas.SetActive(on);
    }

    public void ScorePanelOnOff(bool on)
    {
        scorePanel.SetActive(on);
    }

    public void BudgetLivesOnOff(bool on)
    {
        budgetLives.SetActive(on);
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
            FundRaisingEventManager.instance.NumberOfEventsAllowed -
            FundRaisingEventManager.instance.NumberOfEventsUsed;

        //Debug.Log($"fundingEventLives count: {fundingEventLives.Count}");

        for (int i = 1; i <= fundingEventLives.Count; i++)
        {
            var lifeObjectColor = i <= numberOfEventLivesLeft
                ? new Color(240, 255, 0, 255)
                : new Color(190, 205, 207, 255);

            var lifeObjectName = fundingEventLives[i - 1];
            var lifeObject = GameObject.Find(lifeObjectName);

            //Debug.Log($"lifeObjectName: {lifeObjectName}");

            var renderer = lifeObject.GetComponent<Renderer>();
            renderer.material.color = lifeObjectColor;
        }

        // TODO check if enough funds to deploy any item
        CheckForSufficientFunds(numberOfEventLivesLeft);
    }

    public void OpsStatusText(string pending, string success, string failed)
    {
        pendingOpsText.text = pending;
        successfulOpsText.text = success;
        failedOpsText.text = failed;
    }

    public void FundraisingEventStatusText(string pending, string current, string completed)
    {
        pendingFundraisersText.text = pending;
        currentFundraiserText.text = current;
        completedFundraisersText.text = completed;
    }

    //private void ReduceResourceCountdownStart(float seconds, int regularity, float reduction)
    //{
    //    // Reduce countdown start for resources before timeout when grabbed
    //    if (Math.Floor(seconds) % regularity == 0 && !updatingResourceCountdown)
    //    {
    //        initialResourceObjectCountdown -= reduction;
    //        updatingResourceCountdown = true;
    //    }
    //    else if (Math.Floor(seconds) % regularity > 0)
    //    {
    //        updatingResourceCountdown = false;
    //    }
    //}

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
        budgetText.text = $"£{remainingBudget.ToString().Replace(".00", "")}";

        var numberOfEventLivesLeft =
            FundRaisingEventManager.instance.NumberOfEventsAllowed -
            FundRaisingEventManager.instance.NumberOfEventsUsed;

        CheckForSufficientFunds(numberOfEventLivesLeft);
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
        var resourceCosts = fundRaisingEventService.ResourceCosts();
        var cost = resourceCosts[resource];

        return cost;
    }

    #endregion


    #region Private methods

    private IEnumerator CountdownTasks()
    {
        if (countdownStarted)
        {
            countdown -= Time.deltaTime;
            float seconds = countdown % 60; ;

            // TODO Make into a coroutine
            FundraisingCountdownEvent(seconds, 8);

            if (countdown <= hudDisplayTime && hudDisplayTime != 0)
            {
                HudOnOff(false);
                hudTextMesh.text = "";
                hudDisplayTime = 0;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator GameOver(string reason, int secondsDelay)
    {
        HudMessage($"Game Over! - {reason}", 10);
        yield return new WaitForSeconds(secondsDelay);
        HudMessage("After wait", 5);
        Initiate.Fade("WaitingRoom", Color.green, 2.0f);
    }

    private void CheckForSufficientFunds(int numberOfLivesLeft)
    {
        if (numberOfLivesLeft == 0) 
        {
            // Not enough funds left for cheapest item required by any remaining ops and no fundraising opportunities left = Game Over

            var cheapestRequiredResourceItem = CheapestRequiredResourceItem();

            Debug.Log(
                $"Number of lives left: {numberOfLivesLeft}, Remaining budget: {remainingBudget}, Cheapest required resource item: {cheapestRequiredResourceItem}");

            if (remainingBudget < cheapestRequiredResourceItem)
            {
                Debug.Log("Not enough funds left and no way of raising money");
                GameOver("You do not have enough funds left and no way of raising money!");
            }
        }
        else
        {
            // Funds are low (less than threshold, based on cheapest item) but fundraising event opportunities  remain
            var cheapestResourceItem = CheapestResourceItem();
            var threshold = cheapestResourceItem + cheapestResourceItem / 100 * 50;

            Debug.Log(
                $"Remaining budget ({remainingBudget}) is less than threshold ({threshold})");

            if (remainingBudget <= threshold)
            {
                Debug.Log("low funds warning");
                PlayAudio("lowFundsWarning");
                HudMessage("WARNING: Low funds!!", 4);
            }
        }
    }

    private int CheapestResourceItem()
    {
        int lowestCost = 99999999;

        var fundRaisingEventService = new FundRaisingEventService();
        var resourceCosts = fundRaisingEventService.ResourceCosts();
        foreach (var resourceCost in resourceCosts)
        {
            var cost = resourceCost.Value;
            if (cost < lowestCost) lowestCost = cost;
        }

        return lowestCost;
    }

    private int CheapestRequiredResourceItem()
    {
        int lowestCost = 99999999;

        var fundRaisingEventService = new FundRaisingEventService();
        var resourceCosts = fundRaisingEventService.ResourceCosts();
        
        var opsRemaining = OperationsManager.instance.RemainingOperations;
        foreach (var op in opsRemaining)
        {
            var collected = op.CollectedResources;
            foreach (var requiredResource in op.RequiredResources)
            {
                if (!collected.Contains(requiredResource))
                {
                    var cost = resourceCosts[(Resource) requiredResource];
                    if (cost < lowestCost) lowestCost = cost;
                }
            }
        }
        
        return lowestCost;
    }

    private void Resets()
    {
        if (resetLeaderBoard) scoreService.ResetLeaderBoard();
        if (resetAllPlayerPrefs) PlayerPrefs.DeleteAll();
    }

    private void GetHighScore()
    {
        HighscoreEntry highScoreFromLeaderBoard = scoreService.GetTopHighScore();
        highScore = highScoreFromLeaderBoard != null ? highScoreFromLeaderBoard.score : 0;
        Debug.Log($"High score from leaderboard: {highScore}");
    }

    // TODO Make into a coroutine
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
                FundRaisingEventManager.instance.NextDisplayedEvent();
            }

            updatingFundRaisingEvent = true;
        }
        else if (Math.Floor(seconds) % regularity > 0)
        {
            updatingFundRaisingEvent = false;
        }

    }

    private void InitialiseFundingEventLives()
    {
        fundingEventLives = new List<string>();
        for (int i = 1; i <= FundRaisingEventManager.instance.NumberOfEventsAllowed; i++)
        {
            fundingEventLives.Add($"Event{i}");
        }
    }

    //private void IndicateBudget(float percentRemaining)
    //{
    //    for (var i = 1; i <= 5; i++)
    //    {
    //        var colour = new Color(0, 0, 0, 0);

    //        // colour indicator light if within budget remaining
    //        if (percentRemaining >= i * 16.7)
    //        {
    //            colour = new Color(240, 255, 0, 255);
    //        }
    //        GameObject.Find($"BudgetLife{i}").GetComponent<Renderer>().material.color = colour;
    //    }
    //}

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
        scoreText.text = score.ToString();
    }

    public void UpdateHighscoreDisplay()
    {
        highScoreText.text = highScore.ToString();
    }

    public void ResetLeaderBoard()
    {
        scoreService.ResetLeaderBoard();

        // TODO this is not clearing the table (just the PlayerPrefs)

        //highscoresTable.ResetScoreboard();
        highscoresTable.FillHighscoresTable();
        
        GameManager.instance.UpdateHighscoreDisplay();
    }

    //private IEnumerator WaitForGameStart(int secondsDelay)
    //{
    //    //PlayAudio("backgroundNoise1");

    //    yield return new WaitForSeconds(secondsDelay);

    //    //PlayAudio("missionStatementPart1");
    //    //AnimationManager.instance.FadeFireCurtain(true);
    //    //AnimationManager.instance.LowerStartButton(true);
    //}

    #endregion

}
