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

        [SerializeField] private int numberOfOperations;

        [Tooltip("Seconds delay between failing operations")]
        [SerializeField] private int failedOpsInterval = 45;

        [SerializeField] private bool allowOpsToFail;
        [SerializeField] private bool replaceFailedOps;
        [SerializeField] private bool replaceSuccessfulOps;

        private int operationId0 = 0;
        private int operationId1 = 1;
        private int operationId2 = 2;
        private int operationId3 = 3;

        private static List<Operation> operations;
        private DateTime startDateTime = DateTime.UtcNow;
        private DateTime reviewDateTime;
        private readonly OperationService operationService = new OperationService();
        private readonly List<int> usedIndexes = new List<int>();
        private bool updatingFailedOperation;
        private bool updatingSuccessfulOperation;
        private bool rotateOperations;

        public List<Operation> RemainingOperations
        {
            get
            {
                return operations.Where(o =>
                    o.OperationStatus == OperationStatus.Pending || o.OperationStatus == OperationStatus.None).ToList();
            }
        }

        public int OperationId(int monitorId)
        {
            int operationId = 0;

            switch (monitorId)
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

                if (numberOfOperations < operations.Count)
                {
                    operations = operations.Take(numberOfOperations).ToList();
                }

                // Dubugging
                //DebugHelper.instance.EnumerateOperations(operations, "OperationsManager");
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            //DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Debug.Log("OperationManager Start");

            reviewDateTime = startDateTime.AddSeconds(failedOpsInterval);

            // Randomize Operations at start
            while (usedIndexes.Count < 4)
            {
                int randomIndex = RandomOperationIndex();

                if (!usedIndexes.Contains(randomIndex))
                {
                    usedIndexes.Add(randomIndex);
                    var op = operations.FirstOrDefault(r => r.Id == randomIndex);
                    op.OperationStatus = OperationStatus.Pending;
                    //Debug.Log($"operation: {op.Title} - {op.OperationStatus}");
                }
            }

            // TODO stop using usedIndexes - it's no longer necessary now we have OperationStatus
            Debug.Log($"usedIndexes.Count = {usedIndexes.Count}");

            operationId0 = usedIndexes[0];
            operationId1 = usedIndexes[1];
            operationId2 = usedIndexes[2];
            operationId3 = usedIndexes[3];
            
            UpdateCurrentOperationsChart();

            AssignOperationsToMonitors();
        }

        void FixedUpdate()
        {
            if (allowOpsToFail) StartCoroutine(CheckForFailedOperation());
        }

        private IEnumerator CheckForFailedOperation()
        {
            if (DateTime.UtcNow >= reviewDateTime
                && rotateOperations
                && !updatingSuccessfulOperation
                && !updatingFailedOperation)
            {
                // Select random operation and replace it with a new operation
                updatingFailedOperation = true;

                StartCoroutine(WaitAndAssignNewOperation(4));

                reviewDateTime = DateTime.UtcNow.AddSeconds(failedOpsInterval);
            }
            else
            {
                updatingFailedOperation = false;
            }

            yield return  new WaitForSeconds(0.1f);
        }

        public void SetRotateOperations(bool rotate)
        {
            reviewDateTime = DateTime.UtcNow.AddSeconds(failedOpsInterval);
            rotateOperations = rotate;
        }

        public void AssignOperationsToMonitors()
        {
            // Debugging
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId0), "AssignOperationsToMonitors");
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId1), "AssignOperationsToMonitors");
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId2), "AssignOperationsToMonitors");
            //DebugHelper.instance.EnumerateOperations(operations.FirstOrDefault(o => o.Id == operationId3), "AssignOperationsToMonitors");

            // Heading

            monitor1aText.text = operations.FirstOrDefault(o => o.Id == operationId0)?.Title;
            monitor2aText.text = operations.FirstOrDefault(o => o.Id == operationId1)?.Title;
            monitor3aText.text = operations.FirstOrDefault(o => o.Id == operationId2)?.Title;
            monitor4aText.text = operations.FirstOrDefault(o => o.Id == operationId3)?.Title;

            // Subheading

            monitor1bText.text = operations.FirstOrDefault(o => o.Id == operationId0)?.SubTitle;
            monitor2bText.text = operations.FirstOrDefault(o => o.Id == operationId1)?.SubTitle;
            monitor3bText.text = operations.FirstOrDefault(o => o.Id == operationId2)?.SubTitle;
            monitor4bText.text = operations.FirstOrDefault(o => o.Id == operationId3)?.SubTitle;

            // Checklist
            monitor1cText.text = ResourceListText(operationId0);
            monitor2cText.text = ResourceListText(operationId1);
            monitor3cText.text = ResourceListText(operationId2);
            monitor4cText.text = ResourceListText(operationId3);
        }

        public void UpdateCurrentOperationsChart()
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

        public void OperationSuccessfullyDeployed(int operationId)
        {
            var op = operations.FirstOrDefault(o => o.Id == operationId);
            op.OperationStatus = OperationStatus.Success;

            StartCoroutine(SuccessfullyDeployedRoutine(operationId));
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

        private IEnumerator WaitAndAssignNewOperation(int waitForSeconds)
        {
            GameManager.instance.PlayAudio("operationFailure");

            var scoreService = new ScoreService();
            var score = scoreService.GetScoreValue(ScoreType.OperationFailed);
            GameManager.instance.UpdateScore(score);
            GameManager.instance.UpdateScoresRegister(ScoreType.OperationFailed);

            var monitorNum = FailAndReplaceWithRandomOperation();

            if (monitorNum > 0)
            {
                // There are unused ops
                AnimationManager.instance.ActivateMonitor($"Monitor{monitorNum}", false);

                yield return new WaitForSeconds(waitForSeconds);

                if (replaceFailedOps)
                {
                    AssignOperationsToMonitors();
                    AnimationManager.instance.ActivateMonitor($"Monitor{monitorNum}", true);
                }

                UpdateCurrentOperationsChart();
            }
            else
            {
                // No unused ops left

                // Select an operation - from operationId0 - operationId3 where value > 0
                // We will know what the respective monitor is so we can close it down and fail the op
                // set operationIdx to -1

                var remainingMonitors = RemainingMonitors();

                var rand = (int) (remainingMonitors.Count * Random.value);
                var randomMonitor = remainingMonitors[rand];

                Debug.Log($"Selected monitor: {randomMonitor}");

                AnimationManager.instance.ActivateMonitor($"Monitor{randomMonitor.Key}", false);

                yield return new WaitForSeconds(waitForSeconds);

                operations.First(o => o.Id == randomMonitor.Value).OperationStatus = OperationStatus.Fail;

                AssignOperationsToMonitors();
                UpdateCurrentOperationsChart();

                switch (randomMonitor.Key)
                {
                    case 1:
                        operationId0 = -1;
                        break;
                    case 2:
                        operationId1 = -1;
                        break;
                    case 3:
                        operationId2 = -1;
                        break;
                    case 4:
                        operationId3 = -1;
                        break;
                }

                // No more monitors left = GAME OVER!
                if (remainingMonitors.Count - 1 == 0)
                {
                    GameManager.instance.GameOver("Last operation failed");
                }
            }
        }

        private List<KeyValuePair<int, int>> RemainingMonitors()
        {
            var remainingMonitors = new List<KeyValuePair<int, int>>();

            Debug.Log($"operationId0: {operationId0}");
            Debug.Log($"operationId1: {operationId1}");
            Debug.Log($"operationId2: {operationId2}");
            Debug.Log($"operationId3: {operationId3}");

            if (operationId0 > 0) remainingMonitors.Add(new KeyValuePair<int, int>(1, operationId0));
            if (operationId1 > 0) remainingMonitors.Add(new KeyValuePair<int, int>(2, operationId1));
            if (operationId2 > 0) remainingMonitors.Add(new KeyValuePair<int, int>(3, operationId2));
            if (operationId3 > 0) remainingMonitors.Add(new KeyValuePair<int, int>(4, operationId3));

            Debug.Log($"Remaining monitors: {remainingMonitors.Count}");

            return remainingMonitors;
        }

        private int FailAndReplaceWithRandomOperation()
        {
            Debug.Log("ReplaceRandomOperation");

            var newOpIndex = RandomOperationIndex();
            
            if (newOpIndex > 0)
            {
                Operation op;

                var randomMonitor = (int)(4 * Random.value) + 1;

                switch (randomMonitor)
                {
                    case 1:
                        op = operations.FirstOrDefault(r => r.Id == operationId0);
                        op.OperationStatus = OperationStatus.Fail;
                        if(replaceFailedOps) operationId0 = newOpIndex;
                        break;
                    case 2:
                        op = operations.FirstOrDefault(r => r.Id == operationId1);
                        op.OperationStatus = OperationStatus.Fail;
                        if (replaceFailedOps) operationId1 = newOpIndex;
                        break;
                    case 3:
                        op = operations.FirstOrDefault(r => r.Id == operationId2);
                        op.OperationStatus = OperationStatus.Fail;
                        if (replaceFailedOps) operationId2 = newOpIndex;
                        break;
                    case 4:
                        op = operations.FirstOrDefault(r => r.Id == operationId3);
                        op.OperationStatus = OperationStatus.Fail;
                        if (replaceFailedOps) operationId3 = newOpIndex;
                        break;
                }

                if (replaceFailedOps)
                {
                    op = operations.FirstOrDefault(r => r.Id == newOpIndex);
                    op.OperationStatus = OperationStatus.Pending;
                    usedIndexes.Add(newOpIndex); // TODO remove once sure
                }

                Debug.Log($"Monitor {randomMonitor} - operation replaced with {newOpIndex}");

                DebugHelper.instance.EnumerateOperationIndexes(usedIndexes, operations, "ReplaceOperation");

                return randomMonitor;
            }

            return 0;
        }

        private int RandomOperationIndex()
        {
            var unusedOps = operations.Where(o => o.OperationStatus == OperationStatus.None).ToList();
            if (unusedOps.Any())
            {
                var randomPosition = (int) (unusedOps.Count * Random.value);
                var randomIndex = unusedOps[randomPosition].Id;
                return randomIndex;
            }
            return -1;
        }

        private IEnumerator SuccessfullyDeployedRoutine(int operationId)
        {
            // We don't want to start the routine if an operation is being failed and
            // a monitor is being refreshed, so wait until such a situation has completed

            yield return new WaitUntil(() => updatingFailedOperation == false);

            
            // Flag used to prevent an operation being failed at the same time
            updatingSuccessfulOperation = true;

            //string spotLight = "";
            string monitor = "";

            var newOpIndex = RandomOperationIndex();

            if (operationId == operationId0)
            {
                monitor = "Monitor1";
                //spotLight = "BoxLight1";
                video1.gameObject.SetActive(true);
                video1.Play();
                if (replaceSuccessfulOps) operationId0 = newOpIndex;
            }
            else if (operationId == operationId1)
            {
                monitor = "Monitor2";
                //spotLight = "BoxLight2";
                video2.gameObject.SetActive(true);
                video2.Play();
                if (replaceSuccessfulOps) operationId1 = newOpIndex;
            }
            else if (operationId == operationId2)
            {
                monitor = "Monitor3";
                //spotLight = "BoxLight3";
                video3.gameObject.SetActive(true);
                video3.Play();
                if (replaceSuccessfulOps) operationId2 = newOpIndex;
            }
            else if (operationId == operationId3)
            {
                monitor = "Monitor4";
                //spotLight = "BoxLight4";
                video4.gameObject.SetActive(true);
                video4.Play();
                if (replaceSuccessfulOps) operationId3 = newOpIndex;
            }

            yield return new WaitForSeconds(5);

            video1.Stop();
            video2.Stop();
            video3.Stop();
            video4.Stop();

            AnimationManager.instance.ActivateMonitor(monitor, false);

            //GameObject.Find(spotLight).SetActive(false);

            video1.gameObject.SetActive(false);
            video2.gameObject.SetActive(false);
            video3.gameObject.SetActive(false);
            video4.gameObject.SetActive(false);

            var scoreService = new ScoreService();
            var score = scoreService.GetScoreValue(ScoreType.OperationSuccessful);
            GameManager.instance.UpdateScore(score);
            GameManager.instance.UpdateScoresRegister(ScoreType.OperationSuccessful);
            

            var remainingMonitors = RemainingMonitors();

            // All operations used = Game Over
            if (remainingMonitors.Count == 0)
            {
                score = scoreService.GetScoreValue(ScoreType.GameSuccessfullyCompleted);
                GameManager.instance.UpdateScore(score);
                GameManager.instance.UpdateScoresRegister(ScoreType.GameSuccessfullyCompleted);

                yield return new WaitForSeconds(3);

                GameManager.instance.GameOver("Last operation successfully deployed");
            } 
            else if (replaceSuccessfulOps)
            {
                yield return new WaitForSeconds(3);

                // If we have a new operation assign it to pending and the display on monitor
                if (newOpIndex > 0)
                {
                    var op = operations.First(o => o.Id == newOpIndex);
                    op.OperationStatus = OperationStatus.Pending;
                    usedIndexes.Add(newOpIndex); // TODO remove once sure usedIndexes not being used
                    AssignOperationsToMonitors();
                    AnimationManager.instance.ActivateMonitor(monitor, true);
                }

                OperationsManager.instance.UpdateCurrentOperationsChart();
            }

            updatingSuccessfulOperation = false;
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