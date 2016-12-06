using System.Reflection;
using UnityEngine;

namespace SolarPowerPlantWorksAtNight
{
    public class SolarPowerPlantAIDetour : PowerPlantAI
    {

        private static bool deployed = false;

        private static string[] methods = new[]
        {"GetElectricityRate", "GetConstructionInfo", "BuildingLoaded", "ProduceGoods", "GetElectricityProduction"};

        private static RedirectCallsState[]  states = null;


        public static void Deploy()
        {
            if (!deployed)
            {
                var i = 0;
                foreach (var method in methods)
                {
                    var original = typeof(SolarPowerPlantAI).GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var detour = typeof(SolarPowerPlantAIDetour).GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var state = RedirectionHelper.RedirectCalls(original, detour);
                    if (states == null)
                    {
                        states = new RedirectCallsState[methods.Length];
                    }
                    states[i++] = state;
                }
                deployed = true;
            }
        }

        public static void Revert()
        {
            if (deployed)
            {
                var i = 0;
                foreach (var method in methods)
                {
                    var original = typeof(SolarPowerPlantAI).GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    RedirectionHelper.RevertRedirect(original, states[i++]);
                }
                states = null;
                deployed = false;
            }
        }

        public override int GetElectricityRate(ushort buildingID, ref Building data)
        {
            return base.GetElectricityRate(buildingID, ref data);
        }

        public override string GetConstructionInfo(int productionRate)
        {
            return base.GetConstructionInfo(productionRate);
        }

        public override void BuildingLoaded(ushort buildingID, ref Building data, uint version)
        {
            base.BuildingLoaded(buildingID, ref data, version);
        }

        protected override void ProduceGoods(ushort buildingID, ref Building buildingData, ref Building.Frame frameData, int productionRate, ref Citizen.BehaviourData behaviour, int aliveWorkerCount, int totalWorkerCount, int workPlaceCount, int aliveVisitorCount, int totalVisitorCount, int visitPlaceCount){
            base.ProduceGoods(buildingID, ref buildingData, ref frameData, productionRate, ref behaviour, aliveWorkerCount, totalWorkerCount, workPlaceCount, aliveVisitorCount, totalVisitorCount, visitPlaceCount);
        }

        public override void GetElectricityProduction(out int min, out int max)
        {
            base.GetElectricityProduction(out min, out max);
        }

        
    }
}