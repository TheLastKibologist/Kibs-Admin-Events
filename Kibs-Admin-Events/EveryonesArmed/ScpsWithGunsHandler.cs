using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KibsAdminEvents.EveryonesArmed
{
    public static class ScpsWithGunsHandler
    {
        public static void EventStarted()
        {
            foreach (var item in Player.List)
            {
                if (item.IsScp)
                {
                    Firearm gun = item.AddItem(ItemType.GunCOM18) as Firearm;
                    gun.AmmoDrain = 0;
                    gun.Penetration = 100;
                    item.CurrentItem = gun;
                }
                else
                {
                 Jailbird mace = item.AddItem(ItemType.Jailbird) as Jailbird;
                    mace.ConcussionDuration /= 2;
                    mace.FlashDuration /= 2;
                }
            }
        }
        public static void NewRole(ChangingRoleEventArgs ev)
        {
            ev.Items.Add(ItemType.Jailbird);
        }
    }
}
