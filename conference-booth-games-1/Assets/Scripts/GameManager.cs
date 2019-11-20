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

    private Scene scene; 

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        doorMessage.text = scene.name == "HomeTown" ? "Enter" : "Exit";
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

    public void UpdateDeploymentStatus(int alterStatusBy)
    {
        deploymentStatus = deploymentStatus + alterStatusBy;
        var statusText = Regex.Replace((deploymentStatus).ToString(), "(\\B[A-Z])", " $1");
        deploymentStatusText.text = $"Deployment status: {statusText}";
    }
}
