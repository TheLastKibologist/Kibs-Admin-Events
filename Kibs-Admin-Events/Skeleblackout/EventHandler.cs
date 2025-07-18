﻿using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KibsAdminEvents.Skeleblackout
{
    public static class SkinwalkerEventHandler
    {
        public static void EventStarted()
        {
            foreach (var item in Player.List)
            {
                if (item.IsScp)
                {
                    item.Role.Set(RoleTypeId.Scp3114, spawnFlags: RoleSpawnFlags.AssignInventory);
                    item.Position = Room.Get(Exiled.API.Enums.RoomType.Hcz127).Position + new UnityEngine.Vector3(0, 1, 0);
                }
                else
                {
                    item.AddItem(ItemType.Lantern);
                }
            }
            Map.TurnOffAllLights(4294967295);
        }
        public static void NewRole(ChangingRoleEventArgs ev)
        {
            ev.Items.Add(ItemType.Lantern);
        }
    }
}
