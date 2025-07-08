using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KibsAdminEvents
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool IsDisabled { get; set; }
        public bool Debug { get; set; }

        public static int VictoryTimeHidden { get; set; } = 20;

        public static int EndGameTime { get; set; } = 5;
    }
}
