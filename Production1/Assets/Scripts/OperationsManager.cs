﻿using System;
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

        [SerializeField] private GameObject monitor1;
        [SerializeField] private GameObject monitor2;
        [SerializeField] private GameObject monitor3;
        [SerializeField] private GameObject monitor4;

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
        [SerializeField] private bool rotateOperations = true;

        public static List<Operation> operations;

        public int operationId0 = 0;
        public int operationId1 = 1;
        public int operationId2 = 2;
        public int operationId3 = 3;

        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private OperationService operationService = new OperationService();
        //private List<Operation> usedOperations = new List<Operation>();
        private List<int> usedIndexes = new List<int>();
        private bool updatingMonitorReplacement;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                operations = operationService.GetOperations();
                ShowOperationsStatus();
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

        public void AssignOperationsToMonitors()
        {
            Debug.Log($"operationId0: {operationId0}");
            Debug.Log($"operationId1: {operationId1}");
            Debug.Log($"operationId2: {operationId2}");
            Debug.Log($"operationId3: {operationId3}");

            // Heading

            //monitor1aText.text = operations[operationId0].Title;
            //monitor2aText.text = operations[operationId1].Title;
            //monitor3aText.text = operations[operationId2].Title;
            //monitor4aText.text = operations[operationId3].Title;

            monitor1aText.text = operations.FirstOrDefault(o => o.Id == operationId0).Title;
            monitor2aText.text = operations.FirstOrDefault(o => o.Id == operationId1).Title;
            monitor3aText.text = operations.FirstOrDefault(o => o.Id == operationId2).Title;
            monitor4aText.text = operations.FirstOrDefault(o => o.Id == operationId3).Title;

            // Subheading

            //monitor1bText.text = operations[operationId0].SubTitle;
            //monitor2bText.text = operations[operationId1].SubTitle;
            //monitor3bText.text = operations[operationId2].SubTitle;
            //monitor4bText.text = operations[operationId3].SubTitle;

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
            string debugList = "";
            foreach (var operation in operations.OrderBy(r => r.Title)) //.Where(r => usedIndexes.Contains(r.Id)))
            {
                debugList += $"{operation.Title.Replace("!", "")} ({operation.OperationStatus.ToString()}) \n";
            }
            GameManager.instance.DebugText(debugList);
        }

        public void DisasterScenarioDeployed(int operationId)
        {
            var thisOperation = operations.FirstOrDefault(r => r.Id == operationId);
            thisOperation.OperationStatus = OperationStatus.Success;

            StartCoroutine(DeployedRoutine(operationId));
        }

        public void CollectResource(int operationId, int resourceId)
        {
            operations[operationId].CollectedResources.Add(resourceId);
        }

        public int[] RequiredResources(int operationId)
        {
            return operations[operationId].RequiredResources;
        }

        public int[] CollectedResources(int operationId)
        {
            var operation = operations[operationId];
            return operation.CollectedResources.ToArray();
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
            var newIndex = RandomOperationIndex();
            var randomMonitor = (int) (4 * Random.value);

            Operation operation;

            switch (randomMonitor)
            {
                case 1:
                    operation = operations.FirstOrDefault(r => r.Id == operationId0);
                    operation.OperationStatus = OperationStatus.Fail;
                    operationId0 = newIndex;
                    break;
                case 2:
                    operation = operations.FirstOrDefault(r => r.Id == operationId1);
                    operation.OperationStatus = OperationStatus.Fail;
                    operationId1 = newIndex;
                    break;
                case 3:
                    operation = operations.FirstOrDefault(r => r.Id == operationId2);
                    operation.OperationStatus = OperationStatus.Fail;
                    operationId2 = newIndex;
                    break;
                    operation = operations.FirstOrDefault(r => r.Id == operationId3);
                    operation.OperationStatus = OperationStatus.Fail;
                    operationId3 = newIndex;
                    break;
            }

            operation = operations.FirstOrDefault(r => r.Id == newIndex);
            operation.OperationStatus = OperationStatus.Pending;

            Debug.Log($"Monitor {randomMonitor} - operation replaced with {newIndex}");
            //usedIndexes.Add(newIndex);
        }

        private IEnumerator DeployedRoutine(int operationId)
        {
            Debug.Log($"StartCounter - operationId: {operationId}");

            string monitor = "";

            if (operationId == operationId0)
            {
                monitor = "monitor1";
                video1.gameObject.SetActive(true);
                video1.Play();
            }
            else if (operationId == operationId1)
            {
                monitor = "monitor2";
                video2.gameObject.SetActive(true);
                video2.Play();
            }
            else if (operationId == operationId2)
            {
                monitor = "monitor3";
                video3.gameObject.SetActive(true);
                video3.Play();
            }
            else if (operationId == operationId3)
            {
                monitor = "monitor4";
                video4.gameObject.SetActive(true);
                video4.Play();
            }

            yield return new WaitForSeconds(5);

            video1.Stop();
            video2.Stop();
            video3.Stop();
            video4.Stop();

            Debug.Log($"Monitor: {monitor}");

            // TODO either...
            // Replace existing operation with a new unused one.
            // ReplaceOperation(operationId);

            // TODO or...
            AnimationManager.instance.ActivateMonitor(monitor, false);

            video1.gameObject.SetActive(false);
            video2.gameObject.SetActive(false);
            video3.gameObject.SetActive(false);
            video4.gameObject.SetActive(false);
        }

        private string ResourceListText(int operationId)
        {
            //var requiredResources = operations[operationId].RequiredResources;
            //var collectedResources = operations[operationId].CollectedResources;

            var requiredResources = operations.FirstOrDefault(o => o.Id == operationId).RequiredResources;
            var collectedResources = operations.FirstOrDefault(o => o.Id == operationId).CollectedResources;

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