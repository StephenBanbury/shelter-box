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
                    EstimatedFundsRaised = 300
                },
                new FundRaisingEvent
                {
                    Id = 2,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Dinner with the Churches",
                    SubTitle = "Evening Dinner in a quaint country cottage",
                    Text = "Mr and Mrs Church will lavish their guests with Mr Churches special pasta bake!",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 450,
                },
                new FundRaisingEvent
                {
                    Id = 3,
                    FundRaisingEventType = FundRaisingEventType.EveningDinner,
                    Title = "Dinner with the Churches",
                    SubTitle = "Evening Dinner in a quaint country cottage",
                    Text = "Mr and Mrs Church will lavish their guests with Mr Churches special pasta bake!",
                    EventDate = DateTime.Parse("23/05/2020"),
                    EstimatedFundsRaised = 450,
                }

            };

            return returnEvents;

        }
    }
}
