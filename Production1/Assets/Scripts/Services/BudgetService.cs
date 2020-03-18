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
    }

    
}
