using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace MtfPlayersLeft
{
    public class MainPlugin : Plugin<MainConfig>
    {
        private EventHandler eventHandler;
        public override void OnEnabled()
        {
            eventHandler = new EventHandler(this);
            Exiled.Events.Handlers.Server.RoundStarted += eventHandler.OnRoundStart;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += eventHandler.OnNtfAnnounced;
            Exiled.Events.Handlers.Player.ChangingRole += eventHandler.RoleChange;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= eventHandler.OnRoundStart; 
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= eventHandler.OnNtfAnnounced;
            Exiled.Events.Handlers.Player.ChangingRole -= eventHandler.RoleChange;
            base.OnDisabled();
        }
    }
}
