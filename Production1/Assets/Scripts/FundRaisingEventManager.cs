using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class FundRaisingEventManager : MonoBehaviour
    {
        public static FundRaisingEventManager instance;
        public static List<FundRaisingEvent> fundRaisingEvents;

        //[SerializeField]
        public Text ComputerText;
        private int currentIndex = -1;

        private FundRaisingEventService fundRaisingEventService = new FundRaisingEventService();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                fundRaisingEvents = fundRaisingEventService.GetFundRaisingEvents();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Debug.Log("FundRaisingEventManager Start");
            Debug.Log($"FundRaisingEvents.Count: {fundRaisingEvents.Count}");
            
            NextEvent();
        }

        public void NextEvent()
        {
            // TODO check the new index hasn't been used recently

            Random random = new Random();
            int randomIndex = random.Next(0, fundRaisingEvents.Count);

            if (currentIndex != randomIndex)
            {
                currentIndex = randomIndex;
            }

            AssignEventToScreen();
        }

        public void AssignEventToScreen()
        {
            // Heading
            ComputerText.text = fundRaisingEvents[currentIndex].Title + " " + fundRaisingEvents[currentIndex].SubTitle;

            //monitor2aText.text = reports[reportId1].Title;
            //monitor3aText.text = reports[reportId2].Title;
            //monitor4aText.text = reports[reportId3].Title;

            // Subheading
            //monitor1bText.text = reports[reportId0].SubTitle;
            //monitor2bText.text = reports[reportId1].SubTitle;
            //monitor3bText.text = reports[reportId2].SubTitle;
            //monitor4bText.text = reports[reportId3].SubTitle;

            // Checklist
            //monitor1cText.text = ResourceListText(reportId0);
            //monitor2cText.text = ResourceListText(reportId1);
            //monitor3cText.text = ResourceListText(reportId2);
            //monitor4cText.text = ResourceListText(reportId3);
        }
    }
}