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

            returnRecourceDictionary.Add(Resource.Food, 1300);
            returnRecourceDictionary.Add(Resource.Boats, 1800);
            returnRecourceDictionary.Add(Resource.FirstAid, 1300);
            returnRecourceDictionary.Add(Resource.Tents, 2000);
            returnRecourceDictionary.Add(Resource.Toys, 550);
            returnRecourceDictionary.Add(Resource.Water, 1150);
            returnRecourceDictionary.Add(Resource.Clothing, 2000);
            returnRecourceDictionary.Add(Resource.Blankets, 1350);

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
                    EstimatedFundsRaised = 450
                },
                new FundRaisingEvent
                {
                    Id = 2,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Dinner with the Churches",
                    SubTitle = "Evening Dinner in a quaint country cottage",
                    Text = "Mr and Mrs Church will lavish their guests with Mr Churches special pasta bake!",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 850,
                },
                new FundRaisingEvent
                {
                    Id = 3,
                    FundRaisingEventType = FundRaisingEventType.SponsoredEvent,
                    Title = "Shine Event Bedruthan Hotel",
                    SubTitle = "Charity donating event",
                    Text = "Charity donating event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 3500,
                },
                new FundRaisingEvent
                {
                    Id = 4,
                    FundRaisingEventType = FundRaisingEventType.SponsoredEvent,
                    Title = "London Triathlon",
                    SubTitle = "Sponsored competitors take part in London Triathlon",
                    Text = "Sponsored competitors take part in London Triathlon",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 4500,
                },
                new FundRaisingEvent
                {
                    Id = 5,
                    FundRaisingEventType = FundRaisingEventType.Festival,
                    Title = "WOMAD",
                    SubTitle = "Fundraising at WOMAD festival",
                    Text = "Fundraising at WOMAD festival",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 4000,
                },
                new FundRaisingEvent
                {
                    Id = 6,
                    FundRaisingEventType = FundRaisingEventType.Exhibition,
                    Title = "Hope & Strength Exhibition ",
                    SubTitle = "Display at Hope & Strength Exhibition",
                    Text = "Display at Hope & Strength Exhibition",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 1400,
                },
                new FundRaisingEvent
                {
                    Id = 7,
                    FundRaisingEventType = FundRaisingEventType.TrainingEvent,
                    Title = "Corporate Training",
                    SubTitle = "Sponsored corporate training event",
                    Text = "Sponsored corporate training event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 3100,
                },
                new FundRaisingEvent
                {
                    Id = 8,
                    FundRaisingEventType = FundRaisingEventType.Donation,
                    Title = "Major Donors Fundraising Event",
                    SubTitle = "Major donors donate",
                    Text = "Major donors donate",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 5600,
                },
                new FundRaisingEvent
                {
                    Id = 9,
                    FundRaisingEventType = FundRaisingEventType.Donation,
                    Title = "Rotary Convention",
                    SubTitle = "Rotary International convention and fundraising event",
                    Text = "Rotary International convention and fundraising event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 3500,
                },
                new FundRaisingEvent
                {
                    Id = 10,
                    FundRaisingEventType = FundRaisingEventType.Conference,
                    Title = "Affiliates Conference",
                    SubTitle = "Affiliates conference and fundraising event",
                    Text = "Affiliates conference and fundraising event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 2100,
                },
                new FundRaisingEvent
                {
                    Id = 11,
                    FundRaisingEventType = FundRaisingEventType.SponsoredEvent,
                    Title = "Tent for Lent",
                    SubTitle = "Local social fundraising event",
                    Text = "Local social fundraising event",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 1600,
                }

            };

            return returnEvents;

        }
    }
}
