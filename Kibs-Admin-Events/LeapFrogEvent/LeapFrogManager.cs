using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
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

    public static class LeapFrogManager
    {
        public static void EventStarted()
        {
            Lift.Get(Interactables.Interobjects.ElevatorGroup.Scp049).ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.AdminCommand);
            Exiled.API.Features.Map.CleanAllItems();
            foreach (var item in Room.Get(Exiled.API.Enums.RoomType.Hcz049).Doors)
            {
                item.Lock(Exiled.API.Enums.DoorLockType.AdminCommand);
                item.IsOpen = true;
            }
            bool picked = false;
            foreach (var item in Exiled.API.Features.Player.List)
            {
                if (item.IsScp && !picked)
                {
                    item.Role.Set(PlayerRoles.RoleTypeId.Scp939);
                    item.EnableEffect(Exiled.API.Enums.EffectType.Ensnared, 1, 0, false);
                    picked = true;
                }
                else
                {
                    item.Role.Set(PlayerRoles.RoleTypeId.ClassD);
                }

                item.Position = Door.Get(Exiled.API.Enums.DoorType.Scp049Armory).Position + new UnityEngine.Vector3(0, 1, 0);

            }
            if (!picked)
            {
                Exiled.API.Features.Player player = Exiled.API.Features.Player.List.GetRandomValue();
                player.Role.Set(PlayerRoles.RoleTypeId.Scp939);
                player.EnableEffect(Exiled.API.Enums.EffectType.Ensnared, 1, 0, false);
                player.Position = Door.Get(Exiled.API.Enums.DoorType.Scp049Armory).Position + new UnityEngine.Vector3(0, 1, 0);

            }


        }
        public static void DeadPlayer(Exiled.API.Features.Player player)
        {
            player.Role.Set(RoleTypeId.Scp939, RoleSpawnFlags.None);
            player.EnableEffect(Exiled.API.Enums.EffectType.Ensnared);
        }
    }
}
