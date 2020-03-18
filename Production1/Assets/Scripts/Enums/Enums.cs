namespace Com.MachineApps.PrepareAndDeploy.Enums  
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
        Donation = 5
    }

    public enum CheckListItem
    {
        None = 0,
        Passport = 1,
        Visa = 2,
        Vaccinations = 3,
        FirstAidKit = 4,
        MobilePhone = 5,
        BatteryCharger = 6,
        SunBlock = 7
    }

    public enum DeploymentStatus
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
}
