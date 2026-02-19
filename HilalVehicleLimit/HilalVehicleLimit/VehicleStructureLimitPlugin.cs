using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using System; // ConsoleColor için gerekli
using Logger = Rocket.Core.Logging.Logger;

namespace VehicleStructureLimit
{
    public class VehicleStructureLimitPlugin : RocketPlugin<VehicleLimitConfig>
    {
        protected override void Load()
        {
            // RocketMod Logger genellikle tek parametre (string) alır. 
            // Renklendirme için sistem konsolunu veya RM'nin Log yöntemini kullanabiliriz.

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("###################################################");
            Console.WriteLine("  _    _   _        _____    _____  ");
            Console.WriteLine(" | |  | | | |      |  __ \\  |  __ \\ ");
            Console.WriteLine(" | |__| | | |      | |__) | | |__) |");
            Console.WriteLine(" |  __  | | |      |  _  /  |  ___/ ");
            Console.WriteLine(" | |  | | | |____  | | \\ \\  | |     ");
            Console.WriteLine(" |_|  |_| |______| |_|  \\_\\ |_|     ");
            Console.WriteLine("                                    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($">> Plugin: {Name}");
            Console.WriteLine(">> Yapımcı: Feral");
            Console.WriteLine(">> Şirket: HilalSunucuları");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> Durum: Aktif!");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("###################################################");
            Console.ResetColor();

            BarricadeManager.onDeployBarricadeRequested += OnDeployBarricadeRequested;
        }

        protected override void Unload()
        {
            Logger.Log("VehicleStructureLimit Devre Dışı Bırakıldı - HilalSunucuları");
            BarricadeManager.onDeployBarricadeRequested -= OnDeployBarricadeRequested;
        }

        private void OnDeployBarricadeRequested(Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if (hit == null) return;

            InteractableVehicle vehicle = hit.GetComponentInParent<InteractableVehicle>();
            if (vehicle == null) return;

            UnturnedPlayer player = UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(owner));

            if (player.IsAdmin || player.HasPermission(Configuration.Instance.BypassPermission))
            {
                return;
            }

            byte x, y;
            ushort plant;
            BarricadeRegion region;

            if (BarricadeManager.tryGetPlant(vehicle.transform, out x, out y, out plant, out region))
            {
                if (region.drops.Count >= Configuration.Instance.MaxBarricadesOnVehicle)
                {
                    shouldAllow = false;

                    if (Configuration.Instance.ShowMessageToPlayer)
                    {
                        UnturnedChat.Say(player, $"[HilalRP] Bu araca daha fazla yapı koyamazsın! (Limit: {Configuration.Instance.MaxBarricadesOnVehicle})", Color.red);
                    }
                }
            }
        }
    }
}