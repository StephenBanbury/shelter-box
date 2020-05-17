namespace Com.MachineApps.PrepareAndDeploy.Enums  
{
    public enum Resource
    {
        None = 0,
        Food = 1,
        Water = 2,
        //Clothing = 3,
        Tents = 4,
        FirstAid = 5,
        Toys = 6,
        //Blankets = 7,
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

    public enum FundRaisingEventStatus
    {
        Pending = 0,
        Current = 1,
        Completed = 2,
        OnDisplay = 3
    }

    public enum TripleState
    {
        One = 1,
        Two = 2,
        Three = 3
    }

    public enum OperationStatus
    {
        None = 0,
        Success = 1,
        Fail = 2,
        Pending = 3
    }

    public enum ScoreType
    {
        None = 0,
        ResetScore = 1,
        LosePoints = 2,
        GainPoints = 3,
        ItemAssigned = 4,
        OperationSuccessful = 5,
        FundsRaised = 6,
        ItemDropped = 7,
        ItemNotRequired = 8,
        ItemAlreadyAssigned = 9,
        BalanceIntoRed = 10,
        GameSuccessfullyCompleted = 11,
        OperationFailed = 12
    }
}
