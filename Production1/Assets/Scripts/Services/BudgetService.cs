using System;
using System.Collections.Generic;
using Com.MachineApps.PrepareAndDeploy.Enums;

namespace Com.MachineApps.PrepareAndDeploy.Services
{
    public class BudgetService
    {
        public Dictionary<Resource, int> ResourceCosts()
        {
            var returnRecourceDictionary = new Dictionary<Resource, int>();

            returnRecourceDictionary.Add(Resource.Food, 50);
            returnRecourceDictionary.Add(Resource.Boats, 100);
            returnRecourceDictionary.Add(Resource.FirstAidKits, 40);
            returnRecourceDictionary.Add(Resource.Tents, 150);
            returnRecourceDictionary.Add(Resource.Toys, 15);
            returnRecourceDictionary.Add(Resource.Water, 45);
            returnRecourceDictionary.Add(Resource.Clothing, 60);
            returnRecourceDictionary.Add(Resource.Blankets, 35);

            return returnRecourceDictionary;
        }
    }

    
}
