using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KibsAdminEvents.EnrageEvent
{
    public static class EnrageHandler
    {

        public static CoroutineHandle _096;

        public static void EventStart()
        {
            Player scp = null;
            bool picked = false;
            foreach (var item in Player.List)
            {
                if (item.IsScp)
                {
                    if (picked)
                    {
                        item.Role.Set(PlayerRoles.RoleTypeId.Scientist);
                    }
                    else
                    {
                        item.Role.Set(PlayerRoles.RoleTypeId.Scp096, PlayerRoles.RoleSpawnFlags.None);
                        item.Position = Room.Get(Exiled.API.Enums.RoomType.Surface).Position + new UnityEngine.Vector3(0, 1, 0);
                        item.MaxHealth = 10000;
                        item.Heal(9999999, false);
                        _096 = Timing.RunCoroutine(enrager(item.Role as Scp096Role));
                        picked = true;
                    }
                }
            }
        }
            public static IEnumerator<float> enrager(Exiled.API.Features.Roles.Scp096Role scp096) { 
            while (Globals.CurrentEvent == "096Apollyon")
            {
                foreach (var item in Player.List)
                {
                   if(item.IsAlive && !item.IsScp)
                    {
                        scp096.AddTarget(item);
                    }
                }
                scp096.EnragedTimeLeft = 999999999999;
                yield return Timing.WaitForSeconds(1f);
            }
        }

    }
    }

