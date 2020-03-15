using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class BudgetManager : MonoBehaviour
    {
        public static BudgetManager instance;

        private int currentBudget;

        public int CurrentBudget
        {
            get => currentBudget;
            set => currentBudget = value;
        }

        public void IncreaseBudget(int value)
        {
            currentBudget += value;
        } 
        
        public void ReduceBudget(int value)
        {

            Debug.Log($"Reduce budget by: {value}");

            currentBudget -= value;
        }
    }
}