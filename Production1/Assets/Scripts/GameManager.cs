
using System;
using System.Globalization;
using Com.MachineApps.PrepareAndDeploy;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Services;
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

    [Tooltip("Initial countdown setting for resource objects (seconds)")]
    public float initialResourceObjectCountdown;

    [SerializeField] private GameObject entrance;

    public int BudgetRemaining = 1000;
    public Text BudgetMeter;
    private static float countdown;
    public static DeploymentStatus deploymentStatus;
    public static bool countdownStarted;

    public string playerName = "Stephen";

    private static float hudDisplayTime;
    private bool updatingResourceCountdown;
    private bool updatingFundRaisingEvent;

    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource1 = audioSources[0];
        audioSource2 = audioSources[1];
        audioSource3 = audioSources[2];

        HudOnOff(false);

        AnimationManager.instance.ActivateMonitor("monitor1", "close");
        AnimationManager.instance.ActivateMonitor("monitor2", "close");
        AnimationManager.instance.ActivateMonitor("monitor3", "close");
        AnimationManager.instance.ActivateMonitor("monitor4", "close");

        StartCountdown();

        UpdateBudgetDisplay();
    }

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

    public DeploymentStatus GetDeploymentStatus()
    {
        return deploymentStatus;
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

    //public void UpdateDeploymentStatus(int alterStatusBy)
    //{
    //    deploymentStatus = deploymentStatus + alterStatusBy;
    //    //deploymentStatusText.text = $"Deployment status: {Regex.Replace((deploymentStatus).ToString(), "(\\B[A-Z])", " $1")}";

    //    var redLight = GameObject.Find("TrafficLightRed");
    //    var amberLight = GameObject.Find("TrafficLightAmber");
    //    var greenLight = GameObject.Find("TrafficLightGreen");

    //    switch (deploymentStatus)
    //    {
    //        case DeploymentStatus.Red:

    //            redLight.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 255);

    //            deploymentStatusText.text = $"Status {deploymentStatus.ToString()}: Go to the Shelter Box building and assign deployment resources.";
    //            HudMessage($"Status {deploymentStatus.ToString()}: Go to Shelter Box building and assign deployment resources.", 10);

    //            break;

    //        case DeploymentStatus.Amber:

    //            redLight.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 255);
    //            amberLight.GetComponent<Renderer>().material.color = new Color(248, 128, 0, 255);

    //            audioSource2.Play();

    //            deploymentStatusText.text = $"Status {deploymentStatus.ToString()}: Now collect your personal checklist items.";
    //            HudMessage($"Status {deploymentStatus.ToString()}: Now collect your personal checklist items.", 10);

    //            break;

    //        case DeploymentStatus.Green:

    //            redLight.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 255);
    //            amberLight.GetComponent<Renderer>().material.color = new Color(248, 128, 0, 255);
    //            greenLight.GetComponent<Renderer>().material.color = new Color(0, 110, 10, 255);

    //            audioSource1.Play();

    //            deploymentStatusText.text = $"Status {deploymentStatus.ToString()}: Go to airport!";
    //            HudMessage($"{deploymentStatus.ToString()}: Go to airport!", 10);

    //            break;
    //    }
    //}

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
        BudgetMeter.text = $"{BudgetRemaining.ToString("C", CultureInfo.CurrentCulture)}";
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

    public int GetResourceCost(Resource resource)
    {
        var fundRaisingEventService = new FundRaisingEventService();
        var resourceCost = fundRaisingEventService.ResourceCosts();
        var cost = resourceCost[resource];

        return cost;
    }

    #endregion

    #region Private methods


    #endregion

}
