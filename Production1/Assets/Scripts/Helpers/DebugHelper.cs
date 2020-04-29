using System.Collections.Generic;
using System.Linq;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class DebugHelper : MonoBehaviour
    {
        public static DebugHelper instance;

        void Awake()
        {
            if (instance && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        public void EnumerateOperations(List<Operation> operations, string caller)
        {
            Debug.Log($"Caller: {caller}");
            foreach (var operation in operations)
            {
                EnumerateOperations(operation, "");
            }
        }

        public void EnumerateOperations(Operation operation, string caller)
        {
            if (caller != "") Debug.Log($"Caller: {caller}");
            Debug.Log($"{operation.Title} ({operation.Id}):");
            foreach (var resource in operation.RequiredResources)
            {
                Debug.Log($"{(Resource) resource}");
            }

            Debug.Log("---------------------");
        }

        public void EnumerateOperationIndexes(List<int> indexes, List<Operation> operations, string caller)
        {
            Debug.Log($"Caller: {caller}");
            foreach (var index in indexes)
            {
                EnumerateOperationIndexes(index, operations, "");
            }
            Debug.Log("---------------------");
        }

        public void EnumerateOperationIndexes(int index, List<Operation> operations, string caller)
        {
            if (caller != "") Debug.Log($"Caller: {caller}");
            var op = operations.FirstOrDefault(o => o.Id == index);
            Debug.Log(op != null 
                ? $"Operation Id {index}: {op.Title}" 
                : $"Operation Id {index} does not exist!");
        }
    }
}