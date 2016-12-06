using ICities;

namespace SolarPowerPlantWorksAtNight
{
    public class SolarPowerPlantWorksAtNight : LoadingExtensionBase, IUserMod 
    {
        public string Name
        {
            get
            {
                return "Night Time Solar Power";
            }
        }

        public string Description
        {
            get
            {
                return "Makes solar power plants work at night like prior to After Dark";
            }
        }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            SolarPowerPlantAIDetour.Deploy();
        }

        public override void OnReleased()
        {
            base.OnReleased();
            SolarPowerPlantAIDetour.Revert();
        }
    }
}
