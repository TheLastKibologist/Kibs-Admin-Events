using CustomPlayerEffects;
using CustomRendering;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Permissions.Commands;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.EventArgs;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.CustomRoles.Events;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngineInternal;
using UnityEngine;

namespace EventCommands
{
    using System;

    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using Exiled.Permissions.Extensions;
    using InventorySystem.Items.Keycards.Snake;
    using KibsAdminEvents;
    using Utf8Json.Resolvers.Internal;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventCommands : ICommand
    {
        public string Command { get; } = "TriggerEvent";


        public string[] Aliases { get; } = new[] { "TrigEv" ,"TRV" };


        public string Description { get; } = "Triggers a given event to happen, will go with next round if round already in play";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string evs = null;

            foreach (var item in Globals.events)
            {
                evs += item + " ";
            }

            Player player = Player.Get(sender);

            if (player.CheckPermission("KibsAE.TriggerEvents")) {
                if (arguments.Count > 0) {
                    if (Globals.events.Contains(arguments.First())) {
                        if(Round.InProgress)
                        {
                            response = arguments.First() + " event will happen next round";
                        }
                        else
                        {
                            response = arguments.First() + " event will happen this round";
                        }
                        Globals.IncomingEvent = arguments.First();

                    }
                    else
                    {
                        response = "Inavlid event name, event names are: " + evs;
                    }
                   
                }
                else
                {
                    response = "You need to specify an event, events are: " + evs;
                }
                    }

            else { response = "You dont have the permission"; }

                return true;
        }

    }
    [CommandHandler(typeof(ClientCommandHandler))]
    public class EventCommand2 : ICommand
    {
        public string Command { get; } = "EventInfo";


        public string[] Aliases { get; } = new[] { "EventHelp" };


        public string Description { get; } = "Provides info about a K.A.E event if its in progress";

        public string role = null;
        public string roledata = null;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Globals.CurrentEvent == "TheHide") {
                Player player = Player.Get(sender);

                if (player.IsScp)
                {
                    role = "The Hidden";
                    roledata = "You are invisible to everyone except those directly next to you. You can see people through walls even when their not moving. You win if you kill all the mtf or survive for 20 minutes. Beware, the mtf will try to turn on generators and if all of them activate then you will lose 5 minutes after";
                }
                else
                {
                    if (player.IsNTF)
                    {
                        role = "Mtf";
                        roledata = "Your job is to kill all the hidden before the round ends in 20 minutes. You can also turn on the generators in order to lure the Hidden out. If all the generators activate, the hidden become visible and will die 5 minutes after.";
                    }
                    else
                    {
                        role = "Spectator";
                        roledata = "You are dead, wait to respawn and use again";
                    }
                    }

                    response = "The current event is The Hide By kibs" + Environment.NewLine +
                         "Your current role is " + role + Environment.NewLine +
                         roledata;
            }
            else {
                response = "There is no event this round";
                    }
            
                return true;
        }

    }

}