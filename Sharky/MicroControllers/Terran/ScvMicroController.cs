﻿using SC2APIProtocol;
using Sharky.DefaultBot;
using Sharky.Pathing;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Sharky.MicroControllers.Terran
{
    public class ScvMicroController : IndividualMicroController
    {
        MacroData MacroData;

        public ScvMicroController(DefaultSharkyBot defaultSharkyBot, IPathFinder sharkyPathFinder, MicroPriority microPriority, bool groupUpEnabled)
            : base(defaultSharkyBot, sharkyPathFinder, microPriority, groupUpEnabled)
        {
            MacroData = defaultSharkyBot.MacroData;
        }

        protected bool Repair(UnitCommander commander, IEnumerable<UnitCommander> supportTargets, int frame, out List<SC2APIProtocol.Action> action)
        {
            action = null;

            if (MacroData.Minerals < 5) { return false; }

            var repairTargets = commander.UnitCalculation.NearbyAllies.Where(a => a.Attributes.Contains(Attribute.Mechanical) && a.Unit.BuildProgress == 1 && a.Unit.Health < a.Unit.HealthMax).OrderByDescending(a => a.Unit.HealthMax - a.Unit.Health);

            var repairTarget = repairTargets.FirstOrDefault(a => Vector2.DistanceSquared(a.Position, commander.UnitCalculation.Position) <= (a.Unit.Radius + commander.UnitCalculation.Unit.Radius + commander.UnitCalculation.Range) * (a.Unit.Radius + commander.UnitCalculation.Unit.Radius + commander.UnitCalculation.Range));
            if (repairTarget == null)
            {
                repairTarget = repairTargets.FirstOrDefault();
            }

            if (repairTarget != null)
            {
                action = commander.Order(frame, Abilities.EFFECT_REPAIR, targetTag: repairTarget.Unit.Tag);
                return true;
            }

            return false;
        }

        protected override bool OffensiveAbility(UnitCommander commander, Point2D target, Point2D defensivePoint, Point2D groupCenter, UnitCalculation bestTarget, int frame, out List<SC2APIProtocol.Action> action)
        {
            action = null;

            if (commander.UnitRole == UnitRole.Support)
            {
                if (Repair(commander, null, frame, out action)) { return true; }
            }

            return false;
        }

        public override List<SC2APIProtocol.Action> Support(UnitCommander commander, IEnumerable<UnitCommander> supportTargets, Point2D target, Point2D defensivePoint, Point2D groupCenter, int frame)
        {
            List<SC2APIProtocol.Action> action = null;

            if (Repair(commander, supportTargets, frame, out action)) { return action; }

            return base.Support(commander, supportTargets, target, defensivePoint, groupCenter, frame);
        }

        public override List<SC2APIProtocol.Action> Idle(UnitCommander commander, Point2D defensivePoint, int frame)
        {
            List<SC2APIProtocol.Action> action = null;
            UpdateState(commander, defensivePoint, defensivePoint, null, null, Formation.Normal, frame);
            if (Repair(commander, null, frame, out action)) { return action; }
            return action;
        }
    }
}