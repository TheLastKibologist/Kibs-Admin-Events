using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.Handlers;
using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KibsAdminEvents.Scp173Tag
{
  
    public static class PeanutDodge
    {
        public static void EventStarted()
        {
            bool picked = false;
            foreach (var item in Exiled.API.Features.Player.List)
            {
                if(item.IsScp && !picked) {
                    item.Role.Set(PlayerRoles.RoleTypeId.Scp173);
                    item.EnableEffect(Exiled.API.Enums.EffectType.Slowness, 60, 0, false);
                    picked = true;
                }
                else
                {
                    item.Role.Set(PlayerRoles.RoleTypeId.ClassD);
                }
                item.Position = Room.Get(Exiled.API.Enums.RoomType.EzGateA).Position + new UnityEngine.Vector3(0, 1, 0);

            }
            foreach (var item in Room.Get(Exiled.API.Enums.RoomType.EzGateA).Doors)
            {
                item.Lock(Exiled.API.Enums.DoorLockType.AdminCommand);
            }
        }
    }
}
