using Rocket.API;

namespace VehicleStructureLimit
{
    public class VehicleLimitConfig : IRocketPluginConfiguration
    {
        public int MaxBarricadesOnVehicle;
        public string BypassPermission;
        public bool ShowMessageToPlayer;

        public void LoadDefaults()
        {
            MaxBarricadesOnVehicle = 10; // Araç başına maksimum 10 yapı
            BypassPermission = "vehiclelimit.bypass"; // Bu izne sahip olan takılmaz
            ShowMessageToPlayer = true;
        }
    }
}