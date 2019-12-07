using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // static instance of the GM can be accessed from anywhere
    public static GameManager instance;

    public Text doorMessage;
    public Text deploymentStatusText;
    //public Text timerDisplay;
    public Text hudCountdownDisplay;
    public Text hudText;

    public static float countdown = (5 * 60);
    public static bool countdownStarted = true;
    private static bool timesUp;

    public static DeploymentStatus deploymentStatus;

    private static float hudDisplayTime;

    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;

    private Scene scene;

    private GameObject collectionPoints;
    private GameObject checkListText;

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
            //timeStarted = false;
            //hudText.text = "Make your way to the shop and find the button to exit this disaster area!";
            HudMessage("Make your way to the shop and find the button to exit this disaster area!", 10);
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
        if (countdownStarted)
        {
            countdown -= Time.deltaTime;

            float minutes = Mathf.Floor(countdown / 60);
            float seconds = countdown % 60;

            //timerDisplay.text = $"{minutes:0}:{seconds:00}";
            hudCountdownDisplay.text = $"{minutes:0}:{seconds:00}";

            if (countdown <= 0 && !timesUp)
            {
                //timerDisplay.color = Color.black;
                hudCountdownDisplay.color = Color.black;
                audioSource3.Play();
                timesUp = true;
            }

            if (countdown <= hudDisplayTime && hudDisplayTime != 0)
            {
                hudText.text = "";
                hudDisplayTime = 0;
            }
        }
    }

    private void HudMessage(string messageText, int displayTimeSeconds)
    {
        hudDisplayTime = countdown - displayTimeSeconds;
        hudText.text = messageText;
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

                deploymentStatusText.text = $"Status {deploymentStatus.ToString()}: Go to the Shelter Box building and assign deployment resources.";
                //hudText.text = $"{deploymentStatus.ToString()}: Go to Shelter Box building and assign deployment resources.";
                HudMessage($"Status {deploymentStatus.ToString()}: Go to Shelter Box building and assign deployment resources.", 10);

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

                deploymentStatusText.text = $"Status {deploymentStatus.ToString()}: Now collect your personal checklist items.";
                //hudText.text = $"{deploymentStatus.ToString()}: Now collect your personal checklist items.";
                HudMessage($"Status {deploymentStatus.ToString()}: Now collect your personal checklist items.", 10);

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

                deploymentStatusText.text = $"Status {deploymentStatus.ToString()}: Go to airport!";
                //hudText.text = $"{deploymentStatus.ToString()}: Go to airport!";
                HudMessage($"{deploymentStatus.ToString()}: Go to airport!", 10);

                break;
        }
    }
}
