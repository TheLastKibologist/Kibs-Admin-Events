using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using MapGeneration;
using PlayerRoles;
using System.ComponentModel;
using UnityEngine;

namespace KibsAdminEvents.TheHide
{
    public class TheHideEvents
    {
        public static void HideRoundStart()
        {
            {
                RoomType[] Checkpoints = { RoomType.EzCheckpointHallwayA, RoomType.EzCheckpointHallwayB, RoomType.LczCheckpointA, RoomType.LczCheckpointB };

                foreach (var item in Checkpoints)
                {
                    foreach (var item1 in Room.Get(item).Doors)
                    {
                        item1.Lock(DoorLockType.AdminCommand);
                        Lift.Get(Interactables.Interobjects.ElevatorGroup.LczA01).ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.AdminCommand);
                        Lift.Get(Interactables.Interobjects.ElevatorGroup.LczA02).ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.AdminCommand);

                        Lift.Get(Interactables.Interobjects.ElevatorGroup.LczB01).ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.AdminCommand);
                        Lift.Get(Interactables.Interobjects.ElevatorGroup.LczB02).ChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.AdminCommand);
                    }
                }


                int[] rolekey = { 3, 0, 1, 1, 2, 1, 2, 2, 1, 1, 1, 0, 2, 1, 2, 1, 1, 1, 1, 1, 1, 0, 1, 3, 1, 1, 2, 1, 2, 2, 1, 0, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1 };
                RoomType[] mtfspawns = { RoomType.HczEzCheckpointA, RoomType.HczEzCheckpointB };


                
                    List<Exiled.API.Features.Player> list1 = Exiled.API.Features.Player.List.ToList();
                    List<Exiled.API.Features.Player> list = null;

                    while(list1.Count >= 1)
                    {
                        System.Random random = new System.Random();
                        int indexd = random.Next(0, list1.Count);
                        list.Add(list1[indexd]);
                        list1.RemoveAt(indexd);
                    }

                    foreach (var item in list)
                    {
                        
                        System.Random random = new System.Random();

                        Vector3 spawnpos = Room.Get(mtfspawns[random.Next(0, 2)]).Position + new UnityEngine.Vector3((random.Next(0, 1001) - 500) / 500, 1, (random.Next(0, 1001) - 500) / 500);


                        int index = list.IndexOf(item);


                        switch (rolekey[index])
                        {
                            default:
                                item.Role.Set(RoleTypeId.NtfPrivate, RoleSpawnFlags.AssignInventory);
                                item.Position = spawnpos;
                                break;

                            case 0:
                                CustomRole.Get(32).AddRole(item);
                                Globals.hidden.Add(item);
                                break;

                            case 1:
                                item.Role.Set(RoleTypeId.NtfPrivate, RoleSpawnFlags.AssignInventory);
                                item.Position = spawnpos;
                                break;
                            case 2:
                                item.Role.Set(RoleTypeId.NtfSergeant, RoleSpawnFlags.AssignInventory);
                                item.Position = spawnpos;
                                break;
                            case 3:
                                item.Role.Set(RoleTypeId.NtfCaptain, RoleSpawnFlags.AssignInventory);
                                item.Position = spawnpos;
                                break;
                        }
                        
                            item.Broadcast(10, "For more info about the event, type .EventInfo into console", shouldClearPrevious: true);
                        
                        
                    }
                
            }
            Respawn.ModifyTokens(Faction.FoundationStaff, 20);
            Respawn.ModifyTokens(Faction.FoundationEnemy, -1);
            Respawn.AdvanceTimer(Faction.FoundationEnemy, -10000000000000000f);
        }
    }
}
