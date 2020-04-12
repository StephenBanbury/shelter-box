using System;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;

namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class FundRaisingEventService
    {
        public List<FundRaisingEvent> GetFundRaisingEvents()
        {
            var returnEvents = new List<FundRaisingEvent>();

            // Mock fund raising events for testing
            returnEvents.AddRange(mockFundRaisingEvents());

            return returnEvents;
        }

        public Dictionary<Resource, int> ResourceCosts()
        {
            var returnRecourceDictionary = new Dictionary<Resource, int>();

            returnRecourceDictionary.Add(Resource.Food, 150);
            returnRecourceDictionary.Add(Resource.Boats, 200);
            returnRecourceDictionary.Add(Resource.FirstAidKits, 140);
            returnRecourceDictionary.Add(Resource.Tents, 250);
            returnRecourceDictionary.Add(Resource.Toys, 95);
            returnRecourceDictionary.Add(Resource.Water, 135);
            returnRecourceDictionary.Add(Resource.Clothing, 360);
            returnRecourceDictionary.Add(Resource.Blankets, 135);

            return returnRecourceDictionary;
        }

        private List<FundRaisingEvent> mockFundRaisingEvents()
        {
            var returnEvents = new List<FundRaisingEvent>
            {
                new FundRaisingEvent
                {
                    Id = 1,
                    FundRaisingEventType = FundRaisingEventType.CoffeeMorning,
                    Title = "Coffee Morning with the Simpsons",
                    SubTitle = "Mr and Mrs Simpson invite friends around for morning chat over a cup of coffee",
                    Text = "There will be games and newspapers",
                    EventDate = DateTime.Parse("01/04/2020"),
                    EstimatedFundsRaised = 100
                },
                new FundRaisingEvent
                {
                    Id = 2,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Dinner with the Churches",
                    SubTitle = "Evening Dinner in a quaint country cottage",
                    Text = "Mr and Mrs Church will lavish their guests with Mr Churches special pasta bake!",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 150,
                },
                new FundRaisingEvent
                {
                    Id = 3,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Shine Event Bedruthan Hotel",
                    SubTitle = "Charity donating event",
                    Text = "Charity donating event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 250,
                },
                new FundRaisingEvent
                {
                    Id = 4,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "London Triathlon",
                    SubTitle = "Sponsored competitors take part in London Triathlon",
                    Text = "Sponsored competitors take part in London Triathlon",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 200,
                },
                new FundRaisingEvent
                {
                    Id = 5,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "WOMAD",
                    SubTitle = "Fundraising at WOMAD festival",
                    Text = "Fundraising at WOMAD festival",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 200,
                },
                new FundRaisingEvent
                {
                    Id = 6,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Hope & Strength Exhibition ",
                    SubTitle = "Display at Hope & Strength Exhibition",
                    Text = "Display at Hope & Strength Exhibition",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 230,
                },
                new FundRaisingEvent
                {
                    Id = 7,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Corporate Training",
                    SubTitle = "Sponsored corporate training event",
                    Text = "Sponsored corporate training event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 210,
                },
                new FundRaisingEvent
                {
                    Id = 8,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Major Donors Fundraising Event",
                    SubTitle = "Major donors donate",
                    Text = "Major donors donate",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 300,
                },
                new FundRaisingEvent
                {
                    Id = 9,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Rotary Convention",
                    SubTitle = "Rotary International convention and fundraising event",
                    Text = "Rotary International convention and fundraising event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 250,
                },
                new FundRaisingEvent
                {
                    Id = 10,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Affiliates Conference",
                    SubTitle = "Affiliates conference and fundraising event",
                    Text = "Affiliates conference and fundraising event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 210,
                },
                new FundRaisingEvent
                {
                    Id = 11,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Tent for Lent",
                    SubTitle = "Local social fundraising event",
                    Text = "Local social fundraising event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 90,
                }

            };

            return returnEvents;

        }
    }
}
