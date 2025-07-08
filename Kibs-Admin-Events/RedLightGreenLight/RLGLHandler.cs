using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KibsAdminEvents.RedLightGreenLight
{
    public static class RLGLHandler
    {
        public static bool RedLight;
        public static bool firstick;
        public static int swaptime;
        public static CoroutineHandle _LightChanger;
        public static void EventTriger()
        {
            Round.IsLocked = true;

            foreach (var item in Room.Get(Exiled.API.Enums.RoomType.Surface).Doors)
            {
                item.Lock(Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            foreach (var item in Player.List)
            {
                item.Role.Set(PlayerRoles.RoleTypeId.NtfPrivate, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
                item.Role.Set(PlayerRoles.RoleTypeId.ClassD,PlayerRoles.RoleSpawnFlags.AssignInventory);
                item.EnableEffect(Exiled.API.Enums.EffectType.Slowness,60,0);
            }
            RedLight = false;
            firstick = true;
            _LightChanger = Timing.RunCoroutine(SpeedBooster(4));
        }

        public static IEnumerator<float> SpeedBooster(int minutes)
        {
            Color color = new Color(0, 0, 0);
            while (Globals.CurrentEvent == "RedLightGreenLight")
            {
                System.Random random = new System.Random();
                if (RedLight)
                {

                    color = Color.red;
                    foreach (var item in Player.List)
                    {
                        if (item.Role.Type == PlayerRoles.RoleTypeId.ClassD)
                        {
                            if (item.Velocity.magnitude >= 0.5)
                            {
                                item.Hurt(10, Exiled.API.Enums.DamageType.ParticleDisruptor);
                            }
                        }
                    }

                }
                else
                {
                    color = Color.green;
                }
                if (firstick)
                {
                    swaptime = random.Next(8, 20) * 5;
                    firstick = false;
                }
                else
                {
                    if (swaptime > 0)
                    {
                        swaptime--;
                    }
                    else
                    {
                        firstick = true;
                        RedLight = !RedLight;
                    }
                }
                TimeSpan time = new TimeSpan(0, minutes, 0).Subtract(Round.ElapsedTime);
                string text = null;
                if (minutes > 0)
                {
                    text = time.Minutes + ":" + time.Seconds.ToString("D2");
                }
                else
                {
                    text = time.Seconds.ToString();
                }


                Room.Get(Exiled.API.Enums.RoomType.Surface).Color = color;
                foreach (var item in Player.List)
                {
                    if (time.TotalSeconds > 0)
                    {
                        item.Broadcast(1, "<color=" + color.ToHex() + ">" + text + "</color>", Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        if (item.Role.Type == PlayerRoles.RoleTypeId.ClassD)
                        {
                            item.Vaporize();
                            Room.Get(Exiled.API.Enums.RoomType.Surface).Color = Color.white;
                            break;
                        }
                    }
                }

                yield return Timing.WaitForSeconds(0.2f);
            }
        }
    }
}
