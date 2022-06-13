using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Respawning;
using Respawning.NamingRules;
using System.Linq;
using Exiled.API;
using Exiled.Events.Commands;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace MtfPlayersLeft
{
    public class EventHandler
    {
        public int Spawns = 0;
        private MainPlugin Plugin;
        public EventHandler(MainPlugin plugin)
        {
            Plugin = plugin;
        }
        public void OnRoundStart()
        {
            Spawns = 0;
            var unit = GetUnitName(0);
            MEC.Timing.CallDelayed(0.5f, () => { ChangeUnitNameOnAdvancedTeam(0, unit, $"({Player.List.Where(e => e.Role.Type == RoleType.FacilityGuard).Count()}){unit}"); });
        }

        public string GetUnitName(int index)
        {
            return RespawnManager.Singleton.NamingManager.AllUnitNames[index].UnitName;
        }

        public int GetUnitIndex(string indexer)
        {
            return RespawnManager.Singleton.NamingManager.AllUnitNames.IndexOf(RespawnManager.Singleton.NamingManager.AllUnitNames.First(e => e.UnitName == indexer));
        }

        public void ChangeUnitNameOnAdvancedTeam(int index, string OgUnitName, string UnitName)
        {
            foreach (Player p in Player.List)
            {
                if (p.UnitName == OgUnitName)
                {
                    p.UnitName = UnitName;
                }
            }
            RespawnManager.Singleton.NamingManager.AllUnitNames.Remove(RespawnManager.Singleton.NamingManager.AllUnitNames[index]);
            RespawnManager.Singleton.NamingManager.AllUnitNames.Insert(index, new SyncUnit() { UnitName = UnitName, SpawnableTeam = 2 });
        }

        public void OnNtfRespawn(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
            {
                Spawns++;
                var unit = GetUnitName(Spawns);
                MEC.Timing.CallDelayed(0.1f, () => { ChangeUnitNameOnAdvancedTeam(Spawns, unit, $"({Player.List.Where(e => e.UnitName == GetUnitName(Spawns)).Count()}){unit}"); });
            }
        }

        public void RoleChange(ChangingRoleEventArgs ev)
        {
            var ind = 0;
            if (ev.Player.UnitName.Contains("(DEAD)") || ev.Player.UnitName.Contains("(0)"))
            {
                ChangeUnitNameOnAdvancedTeam(GetUnitIndex(ev.Player.UnitName), GetUnitName(GetUnitIndex(ev.Player.UnitName)), $"({Player.List.Where(e => e.UnitName == GetUnitName(Spawns)).Count()}){GetUnitName(GetUnitIndex(ev.Player.UnitName))}");
            }
            foreach (SyncUnit su in RespawnManager.Singleton.NamingManager.AllUnitNames)
            {
                var pl = Player.List.Where(e => e.UnitName == su.UnitName);
                if (!pl.Any())
                {
                    if (Plugin.Config.ShowDeadInsteadOfZero)
                        ChangeUnitNameOnAdvancedTeam(ind, su.UnitName, $"<color=red>(DEAD){su.UnitName.Remove(0, 3)}</color>");
                    else
                        ChangeUnitNameOnAdvancedTeam(ind, su.UnitName, $"<color=red>(0){su.UnitName.Remove(0, 3)}</color>");
                }
                ind++;
            }
        }
    }
}
