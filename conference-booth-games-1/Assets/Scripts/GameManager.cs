using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // static instance of the GM can be accessed from anywhere
    public static GameManager instance;
    public Text doorMessage;

    public Text timerDisplay;
    public static float timer = (5 * 60);
    public static bool timeStarted = true;

    public static DeploymentStatus deploymentStatus;
    public Text deploymentStatusText;

    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;

    private Scene scene;

    private GameObject collectionPoints;
    private GameObject checkListText;

    private static bool timesUp;

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        if (scene.name != "Disaster")
        {
            doorMessage.text = scene.name == "HomeTown" ? "Enter" : "Exit";

            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0];
            audioSource2 = audioSources[1];
            audioSource3 = audioSources[2];

            collectionPoints = GameObject.Find("CollectionPoints");
            checkListText = GameObject.Find("CheckListText");
        }
        else
        {
            timeStarted = false;
        }
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

    void Update()
    {
        if (timeStarted)
        {
            timer -= Time.deltaTime;

            float minutes = Mathf.Floor(timer / 60);
            float seconds = timer % 60;

            timerDisplay.text = $"{minutes:0}:{seconds:00}";

            if (timer <= 0 && !timesUp)
            {
                timerDisplay.color = Color.black;
                audioSource3.Play();
                timesUp = true;
            }
        }
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

    public void UpdateDeploymentStatus(int alterStatusBy)
    {
        deploymentStatus = deploymentStatus + alterStatusBy;
        //deploymentStatusText.text = $"Deployment status: {Regex.Replace((deploymentStatus).ToString(), "(\\B[A-Z])", " $1")}";

        var redLight = GameObject.Find("TrafficLightRed");
        var amberLight = GameObject.Find("TrafficLightAmber");
        var greenLight = GameObject.Find("TrafficLightGreen");

        switch (deploymentStatus)
        {
            case DeploymentStatus.Red:

                redLight.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 255);

                deploymentStatusText.text = $"{deploymentStatus.ToString()}: Go to Shelter Box building and collect deployment resources.";

                if (collectionPoints != null)
                {
                    collectionPoints.SetActive(false);
                }

                if (checkListText != null)
                {
                    checkListText.SetActive(false);
                }

                break;

            case DeploymentStatus.Amber:

                redLight.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 255);
                amberLight.GetComponent<Renderer>().material.color = new Color(248, 128, 0, 255);

                audioSource2.Play();

                deploymentStatusText.text = $"{deploymentStatus.ToString()}: Now collect your personal checklist items.";

                if (collectionPoints != null)
                {
                    collectionPoints.SetActive(true);
                }

                if (checkListText != null)
                {
                    checkListText.SetActive(true);
                }

                break;

            case DeploymentStatus.Green:

                redLight.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 255);
                amberLight.GetComponent<Renderer>().material.color = new Color(248, 128, 0, 255);
                greenLight.GetComponent<Renderer>().material.color = new Color(0, 110, 10, 255);

                audioSource1.Play();

                deploymentStatusText.text = $"{deploymentStatus.ToString()}: Go to airport!";

                break;
        }
    }
}
