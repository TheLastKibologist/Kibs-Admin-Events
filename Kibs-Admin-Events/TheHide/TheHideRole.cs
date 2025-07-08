using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.CustomRoles;
using MEC;
using PlayerRoles;
using Exiled.API.Features.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using PlayerRoles.PlayableScps.Scp939.Ripples;
using CustomPlayerEffects;
using Exiled.Events.Handlers;
using PlayerRoles.PlayableScps.Scp939;
using Exiled.API.Enums;
using Scp939Role = Exiled.API.Features.Roles.Scp939Role;
using DrawableLine;

namespace KibsAdminEvents
{
    public class TheHideRole : CustomRole
    {
        public static CoroutineHandle _MainCoro;

        public static bool AllGensOn = false;
        public static bool Countdown = false;
        public static TimeSpan timed = new TimeSpan(1);
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp939;
        public override uint Id { get; set; } = 32;
        public override int MaxHealth { get; set; } = 2000;
        public override string Name { get; set; } = "The Hidden";
        public override string Description { get; set; } = "You are invisible, hunt down them all";
        public override string CustomInfo { get; set; } = "The Hidden";
        protected override void RoleAdded(Exiled.API.Features.Player player)
        {
            base.RoleAdded(player);

            _MainCoro = Timing.RunCoroutine(MainCoro(player));

        }
        public static IEnumerator<float> MainCoro(Exiled.API.Features.Player player)
        {
            while (player.IsAlive)
            {
                foreach (var item in Exiled.API.Features.Player.List)
                {
                    if (!item.IsScp && !AllGensOn)
                    {
                        if (Vector3.Distance(item.Position, player.Position) <= 25 && Vector3.Distance(item.Position, player.Position) >= 5)
                        {
                            item.EnableEffect(Exiled.API.Enums.EffectType.AmnesiaVision);
                        }
                        else
                        {

                            item.DisableEffect(Exiled.API.Enums.EffectType.AmnesiaVision);

                        }
                    }
                        Scp939Role scp939 = player.Role as Scp939Role;
                        scp939.PlayRippleSound(UsableRippleType.FireArm, item.Position, player);
                        
                    
                    
                   
                }
                if(Round.ElapsedTime.TotalMinutes >= Config.VictoryTimeHidden && Recontainer.EngagedGeneratorCount < 3)
                {
                    Round.EndRound();
                }
                if (Recontainer.EngagedGeneratorCount >= 3 && !AllGensOn)
                {
                    foreach (var item in Exiled.API.Features.Player.List)
                    {
                        if (item.IsScp)
                        {
                            item.Broadcast(10, "You've lost invisibility and will die in "+ Config.EndGameTime + " mins", shouldClearPrevious: true);
                        }
                        else
                        {
                            item.Broadcast(10, "The Hidden are visible now and will die in " + Config.EndGameTime + "Minutes",shouldClearPrevious: true);
                        }
                    }
                    AllGensOn = true;
                    Timing.CallDelayed(10, () => {
                        Countdown = true;
                        timed = Round.ElapsedTime;
                    });
                }
                if (Countdown)
                {
                    int remainingtime = ((int)timed.Add(new TimeSpan(0, Config.EndGameTime, 0)).Subtract(Round.ElapsedTime).TotalSeconds);
                    Color color = new Color(255,(remainingtime/(Config.EndGameTime*60))*255, (remainingtime / (Config.EndGameTime * 60)) * 255);
                    string message = timed.Add(new TimeSpan(0, Config.EndGameTime, 0)).Subtract(Round.ElapsedTime).Minutes + ":" + timed.Add(new TimeSpan(0, Config.EndGameTime, 0)).Subtract(Round.ElapsedTime).Seconds.ToString("00");
                    foreach (var item in Exiled.API.Features.Player.List)
                    {
                        item.Broadcast(1,"<color="+color.ToHex()+">" + message + "</color>", shouldClearPrevious: true);
                    }
                    if (timed.Add(new TimeSpan(0, Config.EndGameTime, 0)).Subtract(Round.ElapsedTime).TotalMilliseconds <= 0) {
                        foreach (var item in Exiled.API.Features.Player.List)
                        {
                            if (item.IsScp)
                            {
                                item.Kill(deathReason: "The gens were turned on and you passed out from the disruptions");
                            }
                        }
                    }
                }
                    yield return Timing.WaitForSeconds(0.5f);
            }
        }
        }
}
