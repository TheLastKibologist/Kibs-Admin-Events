using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KibsAdminEvents
{
    [CustomItem(ItemType.SurfaceAccessPass)]
    public class HiddenTracker : CustomItem
    {
        public static CoroutineHandle _Tracker;
        public override uint Id { get; set; } = 71;
        public override string Name { get; set; } = "Hidden Tracker";
        public override string Description { get; set; } = "Beeps when the Hidden gets closer";
        public override float Weight { get; set; } = 2;
        public override SpawnProperties SpawnProperties { get; set; }

        protected override void OnChanging(ChangingItemEventArgs ev)
        {
            base.OnChanging(ev);
            if(Check(ev.Item)) {
                _Tracker = Timing.RunCoroutine(HiddenTrack(ev.Item));
            }
        }
        public static IEnumerator<float> HiddenTrack(Exiled.API.Features.Items.Item item)
        {
            float mindist = 0;
            while (true)
            {

                foreach (var hidden in Globals.hidden)
                {
                    if (Vector3.Distance(item.Owner.Position, hidden.Position) <= 25)
                    {
                        if(Vector3.Distance(item.Owner.Position, hidden.Position)< mindist || mindist == 0)
                        {
                            mindist = Vector3.Distance(item.Owner.Position, hidden.Position);
                        }
                    }
                }
                if (mindist != 0)
                {
                    item.Owner.PlayShieldBreakSound();
                    item.Owner.ShowHint("Beep", 0.1f);
                    yield return Timing.WaitForSeconds(mindist / 12.5f);
                }
                else
                {
                    yield return Timing.WaitForSeconds(0.5f);
                }

                
            }
        }
        }
}
