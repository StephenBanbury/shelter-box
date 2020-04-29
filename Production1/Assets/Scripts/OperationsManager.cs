using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using Com.MachineApps.PrepareAndDeploy.Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class OperationsManager : MonoBehaviour
    {
        public static OperationsManager instance;

        //[SerializeField] private GameObject monitor1;
        //[SerializeField] private GameObject monitor2;
        //[SerializeField] private GameObject monitor3;
        //[SerializeField] private GameObject monitor4;

        [SerializeField] private Text monitor1aText;
        [SerializeField] private Text monitor2aText;
        [SerializeField] private Text monitor3aText;
        [SerializeField] private Text monitor4aText;

        [SerializeField] private Text monitor1bText;
        [SerializeField] private Text monitor2bText;
        [SerializeField] private Text monitor3bText;
        [SerializeField] private Text monitor4bText;

        [SerializeField] private Text monitor1cText;
        [SerializeField] private Text monitor2cText;
        [SerializeField] private Text monitor3cText;
        [SerializeField] private Text monitor4cText;

        [SerializeField] private VideoPlayer video1;
        [SerializeField] private VideoPlayer video2;
        [SerializeField] private VideoPlayer video3;
        [SerializeField] private VideoPlayer video4;

        [SerializeField] private int updateInterval = 60;

        private int operationId0 = 0;
        private int operationId1 = 1;
        private int operationId2 = 2;
        private int operationId3 = 3;

        private static List<Operation> operations;

        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private OperationService operationService = new OperationService();
        //private List<Operation> usedOperations = new List<Operation>();
        private List<int> usedIndexes = new List<int>();
        private bool updatingMonitorReplacement;
        private bool rotateOperations;

        public int OperationId(int id)
        {
            int operationId = 0;

            switch (id)
            {
                case 0:
                    operationId = operationId0;
                    break;
                case 1:
                    operationId = operationId1;
                    break;
                case 2:
                    operationId = operationId2;
                    break;
                case 3:
                    operationId = operationId3;
                    break;
            }

            return operationId;
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                operations = operationService.GetOperations();

                // Dubugging
                //DebugHelper.instance.EnumerateOperations(operations, "OperationsManager");

            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Debug.Log("OperationManager Start");

            reviewDateTime = startDateTime.AddSeconds(updateInterval);

            // Randomize Operations at start
            while (usedIndexes.Count < 4)
            {
                int randomIndex = RandomOperationIndex();

                if (!usedIndexes.Contains(randomIndex))
                {
                    usedIndexes.Add(randomIndex);
                    var op = operations.FirstOrDefault(r => r.Id == randomIndex);
                    op.OperationStatus = OperationStatus.Pending;
                    Debug.Log($"operation: {op.Title} - {op.OperationStatus}");
                }
            }

            Debug.Log($"usedIndexes.Count = {usedIndexes.Count}");

            operationId0 = usedIndexes[0];
            operationId1 = usedIndexes[1];
            operationId2 = usedIndexes[2];
            operationId3 = usedIndexes[3];

            rotateOperations = false;

            ShowOperationsStatus();

            AssignOperationsToMonitors();
        }

        void FixedUpdate()
        {
            // Only rotate Operations if rotateOperations = true
            //if (DateTime.UtcNow >= reviewDateTime && rotateOperations)
            //{
            //    operationId0 = operationId0 < operations.Count - 1 ? operationId0 + 1 : 0;
            //    operationId1 = operationId1 < operations.Count - 1 ? operationId1 + 1 : 0;
            //    operationId2 = operationId2 < operations.Count - 1 ? operationId2 + 1 : 0;
            //    operationId3 = operationId3 < operations.Count - 1 ? operationId3 + 1 : 0;

            //    AssignOperationsToMonitors();

            //    reviewDateTime = DateTime.UtcNow.AddSeconds(updateInterval);
            //}

            // Select random operation and replace it with a new operation
            if (DateTime.UtcNow >= reviewDateTime && rotateOperations && !updatingMonitorReplacement)
            {
                Debug.Log("Updating Monitor Replacement");

                updatingMonitorReplacement = true;
                ReplaceOperation();
                AssignOperationsToMonitors();
                ShowOperationsStatus();
                reviewDateTime = DateTime.UtcNow.AddSeconds(updateInterval);
            }
            else //if(DateTime.UtcNow < reviewDateTime)
            {
                updatingMonitorReplacement = false;
            }

        }

        public void SetRotateOperations(bool rotate)
        {
            rotateOperations = rotate;
        }

        public void AssignOperationsToMonitors()
        {
            //Debug.Log($"operationId0: {operationId0}");
            //Debug.Log($"operationId1: {operationId1}");
            //Debug.Log($"operationId2: {operationId2}");
            //Debug.Log($"operationId3: {operationId3}");

            // Debugging
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId0), "AssignOperationsToMonitors");
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId1), "AssignOperationsToMonitors");
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId2), "AssignOperationsToMonitors");
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId3), "AssignOperationsToMonitors");

            // Heading

            monitor1aText.text = operations.FirstOrDefault(o => o.Id == operationId0).Title;
            monitor2aText.text = operations.FirstOrDefault(o => o.Id == operationId1).Title;
            monitor3aText.text = operations.FirstOrDefault(o => o.Id == operationId2).Title;
            monitor4aText.text = operations.FirstOrDefault(o => o.Id == operationId3).Title;

            // Subheading

            monitor1bText.text = operations.FirstOrDefault(o => o.Id == operationId0).SubTitle;
            monitor2bText.text = operations.FirstOrDefault(o => o.Id == operationId1).SubTitle;
            monitor3bText.text = operations.FirstOrDefault(o => o.Id == operationId2).SubTitle;
            monitor4bText.text = operations.FirstOrDefault(o => o.Id == operationId3).SubTitle;

            // Checklist
            monitor1cText.text = ResourceListText(operationId0);
            monitor2cText.text = ResourceListText(operationId1);
            monitor3cText.text = ResourceListText(operationId2);
            monitor4cText.text = ResourceListText(operationId3);
        }

        public void ShowOperationsStatus()
        {
            string pendingList = "";
            string successList = "";
            string failedList = "";

            foreach (var operation in operations
                .Where(o => o.OperationStatus == OperationStatus.Pending)
                .OrderBy(r => r.Title))
            {
                pendingList += $"{operation.Title.Replace("!", "")}\n";
            }

            foreach (var operation in operations
                .Where(o => o.OperationStatus == OperationStatus.Success)
                .OrderBy(r => r.Title))
            {
                successList += $"{operation.Title.Replace("!", "")}\n";
            }

            foreach (var operation in operations
                .Where(o => o.OperationStatus == OperationStatus.Fail)
                .OrderBy(r => r.Title))
            {
                failedList += $"{operation.Title.Replace("!", "")}\n";
            }

            GameManager.instance.OpsStatusText(pendingList, successList, failedList);
        }

        public void DisasterScenarioDeployed(int operationId)
        {
            var op = operations.FirstOrDefault(o => o.Id == operationId);
            op.OperationStatus = OperationStatus.Success;

            StartCoroutine(DeployedRoutine(operationId));
        }

        public void CollectResource(int operationId, int resourceId)
        {
            var op = operations.FirstOrDefault(o => o.Id == operationId);
            op?.CollectedResources.Add(resourceId);
        }

        public int[] RequiredResources(int operationId)
        {
            var op = operations.FirstOrDefault(o => o.Id == operationId);
            return op?.RequiredResources.ToArray();
        }

        public int[] CollectedResources(int operationId)
        {
            var op = operations.FirstOrDefault(o => o.Id == operationId);
            return op?.CollectedResources.ToArray();
        }

        public TripleState SelectedResourceIsRequired(int operationId, int resourceId)
        {
            var response = TripleState.One; // Not required or collected

            var resourcesRequiredForDisaster = RequiredResources(operationId);
            var resourcesCollected = CollectedResources(operationId);

            if (resourcesCollected.Contains(resourceId))
            {
                response = TripleState.Two; // Already collected
            }
            else if (resourcesRequiredForDisaster.Contains(resourceId))
            {
                response = TripleState.Three; // Is required
            }

            return response;
        }

        private int RandomOperationIndex()
        {
            var randomIndex = (int) (operations.Count * Random.value) + 1;

            // use random index if not used before, otherwise recursively generate a new one
            var returnValue = !usedIndexes.Contains(randomIndex)
                ? randomIndex
                : RandomOperationIndex();

            Debug.Log($"randomIndex: {returnValue}");
            return returnValue;
        }

        private void ReplaceOperation()
        {
            Debug.Log("ReplaceOperation");

            var newIndex = RandomOperationIndex();
            var randomMonitor = (int) (4 * Random.value);

            Operation op;

            switch (randomMonitor)
            {
                case 1:
                    op = operations.FirstOrDefault(r => r.Id == operationId0);
                    op.OperationStatus = OperationStatus.Fail;
                    operationId0 = newIndex;
                    break;
                case 2:
                    op = operations.FirstOrDefault(r => r.Id == operationId1);
                    op.OperationStatus = OperationStatus.Fail;
                    operationId1 = newIndex;
                    break;
                case 3:
                    op = operations.FirstOrDefault(r => r.Id == operationId2);
                    op.OperationStatus = OperationStatus.Fail;
                    operationId2 = newIndex;
                    break;
                    op = operations.FirstOrDefault(r => r.Id == operationId3);
                    op.OperationStatus = OperationStatus.Fail;
                    operationId3 = newIndex;
                    break;
            }

            op = operations.FirstOrDefault(r => r.Id == newIndex);
            op.OperationStatus = OperationStatus.Pending;

            Debug.Log($"Monitor {randomMonitor} - operation replaced with {newIndex}");
            //usedIndexes.Add(newIndex);
        }

        private IEnumerator DeployedRoutine(int operationId)
        {
            string spotLight = "";
            string monitor = "";

            if (operationId == operationId0)
            {
                monitor = "Monitor1";
                spotLight = "SpotLight1";
                video1.gameObject.SetActive(true);
                video1.Play();
            }
            else if (operationId == operationId1)
            {
                monitor = "Monitor2";
                spotLight = "SpotLight2";
                video2.gameObject.SetActive(true);
                video2.Play();
            }
            else if (operationId == operationId2)
            {
                monitor = "Monitor3";
                spotLight = "SpotLight3";
                video3.gameObject.SetActive(true);
                video3.Play();
            }
            else if (operationId == operationId3)
            {
                monitor = "Monitor4";
                spotLight = "SpotLight4";
                video4.gameObject.SetActive(true);
                video4.Play();
            }

            yield return new WaitForSeconds(5);

            video1.Stop();
            video2.Stop();
            video3.Stop();
            video4.Stop();

            AnimationManager.instance.ActivateMonitor(monitor, false);

            GameObject.Find(spotLight).SetActive(false);

            video1.gameObject.SetActive(false);
            video2.gameObject.SetActive(false);
            video3.gameObject.SetActive(false);
            video4.gameObject.SetActive(false);
        }

        private string ResourceListText(int operationId)
        {
            List<int> requiredResources = new List<int>();
            List<int> collectedResources = new List<int>();

            var op = operations.FirstOrDefault(o => o.Id == operationId);

            if (op != null)
            {

                requiredResources = op.RequiredResources;
                collectedResources = op.CollectedResources;
            }


            var resourceText = "";
            foreach (var requiredResource in requiredResources)
            {
                if (!collectedResources.Contains(requiredResource))
                {
                    resourceText += Regex.Replace(((Resource)requiredResource).ToString(), "(\\B[A-Z])", " $1");
                    resourceText += "\r\n";
                }
            }

            return resourceText;
        }
    }
}