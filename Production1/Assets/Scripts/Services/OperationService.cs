using System;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Enums;
using Com.MachineApps.PrepareAndDeploy.Models;

namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class OperationService
    {
        public List<Operation> GetOperations()
        {
            var returnOperations = new List<Operation>();

            // Mock ops for testing
            returnOperations.AddRange(mockOperations());

            return returnOperations;
        }

        private List<Operation> mockOperations()
        {
            var returnOperations = new List<Operation>
            {
                new Operation
                {
                    Id = 1,
                    Title = "Bahamas: Hurricane",
                    SubTitle = "Hurricain Dorian - many homes destroyed",
                    Text = "",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new List<int>()
                    {
                        //(int) Resource.Clothing, 
                        (int) Resource.Food, 
                        (int) Resource.FirstAid, 
                        (int) Resource.Tents, 
                        (int) Resource.Toys, 
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>(),
                    OperationStatus = OperationStatus.None,
                    MovieTitle = "Bahamas_HurricainDorian_2019.mp4"
                },
                new Operation
                {
                    Id = 2,
                    Title = "Drought",
                    SubTitle = "Water supplies depleted and crops failing",
                    Text = "UN declares it a Humanitarian Crisis",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new List<int>()
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAid,
                        //(int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water
                    },
                    CollectedResources = new List<int>(),
                    OperationStatus = OperationStatus.None,
                    MovieTitle = ""
                },
                new Operation
                {
                    Id = 3,
                    Title = "BurkinaFaso: Conflict",
                    SubTitle = "Displacement of people",
                    Text = "",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new List<int>()
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAid,
                        (int) Resource.Tents,
                        //(int) Resource.Toys,
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>(),
                    OperationStatus = OperationStatus.None,
                    MovieTitle = "BurkinaFaso_Conflict_2020.mp4"
                },
                new Operation
                {
                    Id = 4,
                    Title = "Bangladesh: Flood",
                    SubTitle = "Widespread displacement",
                    Text = "",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new List<int>()
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAid,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        //(int) Resource.Boats,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>(),
                    OperationStatus = OperationStatus.None,
                    MovieTitle = "Bangladesh_Flood_2019.mp4"
                },
                new Operation
                {
                    Id = 5,
                    Title = "Tsunami",
                    SubTitle = "As many as 2000 people affected",
                    Text = "Rescue is underway",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new List<int>()
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAid,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        //(int) Resource.Water,
                        (int) Resource.Boats,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>(),
                    OperationStatus = OperationStatus.None,
                    MovieTitle = ""
                },
                new Operation
                {
                    Id = 6,
                    Title = "Peru: Earthquake",
                    SubTitle = "Five villages evacuated",
                    Text = "Hundreds of people unaccounted for",
                    ReportDate = DateTime.UtcNow,
                    Archived = false,
                    RequiredResources = new List<int>()
                    {
                        //(int) Resource.Clothing,
                        (int) Resource.Food,
                        (int) Resource.FirstAid,
                        (int) Resource.Tents,
                        (int) Resource.Toys,
                        (int) Resource.Water,
                        //(int) Resource.Blankets
                    },
                    CollectedResources = new List<int>(),
                    OperationStatus = OperationStatus.None,
                    MovieTitle = ""
                },
                //new Operation
                //{
                //    Id = 7,
                //    Title = "Auckland: Volcano",
                //    SubTitle = "Assessment team has arrived",
                //    Text = "",
                //    ReportDate = DateTime.UtcNow,
                //    Archived = false,
                //    RequiredResources = new List<int>()
                //    {
                //        //(int) Resource.Clothing,
                //        //(int) Resource.Food,
                //        (int) Resource.FirstAid,
                //        (int) Resource.Tents,
                //        (int) Resource.Toys,
                //        (int) Resource.Water,
                //        //(int) Resource.Blankets
                //    },
                //    CollectedResources = new List<int>(),
                //    OperationStatus = OperationStatus.None,
                //    MovieTitle = ""
                //},
                //new Operation
                //{
                //    Id = 8,
                //    Title = "Malawi: Flooding",
                //    SubTitle = "Assessment team has arrived",
                //    Text = "",
                //    ReportDate = DateTime.UtcNow,
                //    Archived = false,
                //    RequiredResources = new List<int>()
                //    {
                //        //(int) Resource.Clothing,
                //        (int) Resource.Food,
                //        (int) Resource.FirstAid,
                //        (int) Resource.Tents,
                //        //(int) Resource.Toys,
                //        (int) Resource.Water,
                //        //(int) Resource.Blankets
                //    },
                //    CollectedResources = new List<int>(),
                //    OperationStatus = OperationStatus.None,
                //    MovieTitle = ""
                //},
                //new Operation
                //{
                //    Id = 9,
                //    Title = "Syria: Conflict",
                //    SubTitle = "Shelter, aid and winter clothing provided near Idlib",
                //    Text = "",
                //    ReportDate = DateTime.UtcNow,
                //    Archived = false,
                //    RequiredResources = new List<int>()
                //    {
                //        //(int) Resource.Clothing,
                //        (int) Resource.Food,
                //        (int) Resource.FirstAid,
                //        (int) Resource.Tents,
                //        (int) Resource.Toys,
                //        (int) Resource.Water,
                //        //(int) Resource.Blankets
                //    },
                //    CollectedResources = new List<int>(),
                //    OperationStatus = OperationStatus.None,
                //    MovieTitle = ""
                //},
                //new Operation
                //{
                //    Id = 10,
                //    Title = "Somaliland: Drought",
                //    SubTitle = "Over 4,468 households helped to date",
                //    Text = "",
                //    ReportDate = DateTime.UtcNow,
                //    Archived = false,
                //    RequiredResources = new List<int>()
                //    {
                //        //(int) Resource.Clothing,
                //        (int) Resource.Food,
                //        //(int) Resource.FirstAid,
                //        //(int) Resource.Tents,
                //        (int) Resource.Toys,
                //        (int) Resource.Water,
                //        //(int) Resource.Blankets
                //    },
                //    CollectedResources = new List<int>(),
                //    OperationStatus = OperationStatus.None,
                //    MovieTitle = ""
                //},
                //new Operation
                //{
                //    Id = 11,
                //    Title = "Lake Chad Basin: Nigeria, Niger, Chad & Cameroon",
                //    SubTitle = "5th year of assistance through partners, ACTED & IEDA",
                //    Text = "",
                //    ReportDate = DateTime.UtcNow,
                //    Archived = false,
                //    RequiredResources = new List<int>()
                //    {
                //        //(int) Resource.Clothing,
                //        (int) Resource.Food,
                //        (int) Resource.FirstAid,
                //        (int) Resource.Tents,
                //        (int) Resource.Toys,
                //        (int) Resource.Water,
                //        //(int) Resource.Blankets
                //    },
                //    CollectedResources = new List<int>(),
                //    OperationStatus = OperationStatus.None,
                //    MovieTitle = ""
                //}
            };

            return returnOperations;
        }
    }

    
}
