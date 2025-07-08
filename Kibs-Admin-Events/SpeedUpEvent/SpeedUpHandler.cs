using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KibsAdminEvents.SpeedUpEvent
{
    public static class SpeedUpHandler
    {
        public static byte boost;

        public static CoroutineHandle _SpeedBooster;
        public static void SpeedUp() {
            boost = 0;
            _SpeedBooster = Timing.RunCoroutine(SpeedBooster());
        }

        public static IEnumerator<float> SpeedBooster()
        {
            while (Globals.CurrentEvent == "SpeedUp")
            {
                if (boost <= 180)
                {
                    boost++;
                }
                foreach (var item in Player.List)
                {
                    if (item.IsAlive)
                    {
                        item.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost,boost , 0, true);
                    }
                }
                yield return Timing.WaitForSeconds(6f);
            }
        }
    }
}
