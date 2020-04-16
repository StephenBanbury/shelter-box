﻿namespace Com.MachineApps.PrepareAndDeploy.Enums  
{
    public enum Resource
    {
        None = 0,
        Food = 1,
        Water = 2,
        Clothing = 3,
        Tents = 4,
        FirstAidKits = 5,
        Toys = 6,
        Blankets = 7,
        Boats = 8
    }

    public enum FundRaisingEventType
    {
        Other = 0,
        Exhibition = 1,
        CoffeeMorning = 2,
        EveningDinner = 3,
        SponsoredEvent = 4,
        Donation = 5,
        Festival = 6,
        TrainingEvent = 7,
        Conference = 8
    }

    public enum FundLevel
    {
        Red = 0,
        Amber = 1, 
        Green = 2
    }

    public enum TripleState
    {
        One = 1,
        Two = 2,
        Three = 3
    }

    public enum ScoreType
    {
        Unassigned = 0,
        ResetScore = 1,
        LosePoints = 2,
        GainPoints = 3,
        ItemAssigned = 4,
        DeploymentCompleted = 5,
        FundsRaised = 6,
        ItemDropped = 7,
        IncorrectItemAssigned = 8,
        ItemAlreadyAssigned = 9,
        BalanceIntoRed = 10
    }
}
