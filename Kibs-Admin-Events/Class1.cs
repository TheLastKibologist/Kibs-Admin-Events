using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Features.Waves;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using KibsAdminEvents.EveryonesArmed;
using KibsAdminEvents.Scp173Tag;
using KibsAdminEvents.Skeleblackout;
using KibsAdminEvents.SpeedUpEvent;
using KibsAdminEvents.TheHide;
using MapGeneration;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KibsAdminEvents
{
    public static class Globals
    {
        
        public static string CurrentEvent = null;
        public static string IncomingEvent = null;
        public static string[] events = { "SpeedUp" , "096Apollyon", "Skinwalker", "PeanutDodge" , "LeapFrog" , "GunsAndGlowsticks" };
        public static List<Exiled.API.Features.Player> hidden = null;
    }
    public class Class1 : Plugin<Config>
    {


        public TheHideRole theHide = new TheHideRole();
        public override string Name => "Kibs Admin Events";
        public override string Author => "Kibs";
        public override System.Version Version => new System.Version(1,0,0);

        public static Class1 instance;
        public override void OnEnabled()
        {
            instance = this;
            theHide.Register();
            CustomItem.RegisterItems();

            Exiled.Events.Handlers.Player.ActivatingGenerator += Generator.OnActivatingGenerator;
            Exiled.Events.Handlers.Player.ChangingRole += RoundStart.OnRoleChanged;
            Exiled.Events.Handlers.Player.Died += RoundStart.OnDeath;

            Exiled.Events.Handlers.Scp173.BeingObserved += RoundStart.BeingWatched;

            Exiled.Events.Handlers.Server.RespawnedTeam += Respawns.OnRespawnedTeam;
            Exiled.Events.Handlers.Server.RoundStarted += RoundStart.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            instance = null;
            theHide.Unregister();

            CustomItem.UnregisterItems();

            Exiled.Events.Handlers.Player.ActivatingGenerator -= Generator.OnActivatingGenerator;
            Exiled.Events.Handlers.Player.ChangingRole -= RoundStart.OnRoleChanged;
            Exiled.Events.Handlers.Player.Died -= RoundStart.OnDeath;


            Exiled.Events.Handlers.Scp173.BeingObserved -= RoundStart.BeingWatched;

            Exiled.Events.Handlers.Server.RespawnedTeam -= Respawns.OnRespawnedTeam;
            Exiled.Events.Handlers.Server.RoundStarted -= RoundStart.OnRoundStarted;

            base.OnDisabled();
        }
        public override void OnReloaded()
        {
            instance = this;
            base.OnReloaded();
        }



        public static class Generator
        {
            public static void OnActivatingGenerator(ActivatingGeneratorEventArgs ev)
            {
                if (Globals.CurrentEvent == Globals.events[0])
                {
                    foreach (var item in Exiled.API.Features.Player.List)
                    {
                        if (item.IsScp)
                        {
                            item.Broadcast(15, "The generator in " + ev.Generator.Room.name + " is being activated");
                        }
                    }
                }
            }
        }
        public static class Respawns
        {
            public static void OnRespawnedTeam(RespawnedTeamEventArgs ev) { 
           if(Globals.CurrentEvent == Globals.events[0]) {
                    RoomType[] mtfspawns = { RoomType.HczEzCheckpointA, RoomType.HczEzCheckpointB };
                    foreach (var item in ev.Players)
                    {
                        System.Random random = new System.Random();

                        item.Position = Room.Get(mtfspawns[random.Next(0, 2)]).Position + new Vector3(0, 1, 0);
                    }

                } 
            
            }
        }


        public static class RoundStart
        {
            public static void OnDeath(DiedEventArgs ev)
            {
                if(Globals.CurrentEvent == "LeapFrog")
                {
                    LeapFrogManager.DeadPlayer(ev.Player);
                }
            }
            public static void BeingWatched(BeingObservedEventArgs ev)
            {
                if(Globals.CurrentEvent == "PeanutDodge")
                {
                    ev.IsAllowed = false;
                }
            }
            public static void OnRoleChanged(ChangingRoleEventArgs ev)
            {
                if (Globals.CurrentEvent == "Skinwalker")
                {
                    SkinwalkerEventHandler.NewRole(ev);
                }
                if (Globals.CurrentEvent == "GunsAndGlowsticks")
                {
                    ScpsWithGunsHandler.NewRole(ev);
                }
            }
            
            public static void OnRoundStarted()
            {

                if (Globals.IncomingEvent != null) { 
                Globals.CurrentEvent = Globals.IncomingEvent;
                Globals.IncomingEvent = null;
                    switch (Globals.CurrentEvent)
                    {
                        default:

                            break;

                        case "SpeedUp":

                            SpeedUpHandler.SpeedUp();
                            break;

                        case "096Apollyon":
                            EnrageEvent.EnrageHandler.EventStart();
                            break;

                        case "Skinwalker":
                            SkinwalkerEventHandler.EventStarted();
                            break;

                        case "TheHide":
                            TheHideEvents.HideRoundStart();
                            break;
                        case "PeanutDodge":
                            PeanutDodge.EventStarted();
                            break;

                        case "LeapFrog":
                            LeapFrogManager.EventStarted();
                            break;

                        case "GunsAndGlowsticks":
                            ScpsWithGunsHandler.EventStarted();
                            break;
                    }
                }
                else
                {
                    Globals.CurrentEvent = null;
                    Globals.hidden.Clear();
                }





            }

            // todo: scps with guns and everyone else with jailbirds, 939 lunge only but dead ppl also become 939
            }
        }
    }

