using System;
using RimWorld;
using TwitchToolkit.Store;
using Verse;
using IncidentWorker_RaidEnemy = TwitchToolkit.Incidents.IncidentWorker_RaidEnemy;

namespace ToolkitAddon_VanillaEventGranularity
{
    public class IncidentAddon_DelayedRaid : IncidentHelper
    {
        private IncidentParms parms;
        private IncidentWorker worker;
        private bool timeToFire;
        private int ticksDelay;
        private const int TICKS_PER_HOUR = 2500;

        public override bool IsPossible()
        {
            worker = new IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;
            parms = new IncidentParms();
            parms.points = DefDatabase<ConfigDef>.GetNamed(storeIncident.defName).value;
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
            parms.target = Current.Game.RandomPlayerHomeMap;
            ticksDelay = Rand.RangeInclusive(DefDatabase<ConfigDef>.GetNamed("RaidMinimumTicks").value,
                DefDatabase<ConfigDef>.GetNamed("RaidMinimumTicks").value);
            return (worker.CanFireNow(parms));
        }

        public override void TryExecute()
        {
            // buy in the store queues up the incident, second trigger fires the raid
            if (timeToFire)
            {
                worker.TryExecute(parms);
            }
            else
            {
                timeToFire = true;
                string letterContent = "LetterDelayedRaid".Translate(Math.Ceiling((double) ticksDelay / TICKS_PER_HOUR));
                Find.LetterStack.ReceiveLetter("LetterLabelDelayedRaid".Translate(), letterContent, LetterDefOf.ThreatSmall);
                Find.Storyteller.incidentQueue.Add(worker.def, Find.TickManager.TicksGame + ticksDelay, parms, TICKS_PER_HOUR);
            }
        }
    }
}