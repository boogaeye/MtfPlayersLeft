using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;

namespace MtfPlayersLeft
{
    public class MainConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool ShowDeadInsteadOfZero { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}
