using RimWorld;
using TwitchToolkit.Store;
using Verse;
using IncidentWorker_RaidEnemy = TwitchToolkit.Incidents.IncidentWorker_RaidEnemy;

namespace ToolkitAddon_VanillaEventGranularity
{
    public class IncidentAddon_Raid : IncidentHelper
    {
        private IncidentParms parms;
        private IncidentWorker worker;

        public override bool IsPossible()
        {
            worker = new IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;
            parms = new IncidentParms();
            parms.points = DefDatabase<ConfigDef>.GetNamed(storeIncident.defName).value;
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
            parms.target = Current.Game.RandomPlayerHomeMap;
            return (worker.CanFireNow(parms));
        }

        public override void TryExecute() => worker.TryExecute(parms);
    }
}