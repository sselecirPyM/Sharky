﻿using Sharky.DefaultBot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharky.Builds.MacroServices
{
    /// <summary>
    /// Class for requesting cancelling building a unit
    /// </summary>
    public class UnitRequestCancellingService
    {
        ActiveUnitData ActiveUnitData;
        MacroData MacroData;
        SharkyUnitData SharkyUnitData;

        private HashSet<UnitTypes> CancelRequests = new();

        public UnitRequestCancellingService(DefaultSharkyBot defaultSharkyBot)
        {
            ActiveUnitData = defaultSharkyBot.ActiveUnitData;
            MacroData = defaultSharkyBot.MacroData;
            SharkyUnitData = defaultSharkyBot.SharkyUnitData;
        }

        /// <summary>
        /// Requests unit cancelling.
        /// </summary>
        /// <param name="unitType">Unit type to cancel.</param>
        public void RequestCancel(UnitTypes unit)
        {
            CancelRequests.Add(unit);
        }

        public List<SC2APIProtocol.Action> CancelUnits()
        {
            var commands = new List<SC2APIProtocol.Action>();

            foreach (var unitType in CancelRequests)
            {
                if (!SharkyUnitData.ResearchedUpgrades.Contains((uint)unitType))
                {
                    var data = SharkyUnitData.UnitData[unitType];

                    var builder = ActiveUnitData.Commanders.Values.FirstOrDefault(c => c.UnitCalculation.Unit.Orders.Any(o => o.AbilityId == (int)data.AbilityId));

                    if (builder != null)
                    {
                        Console.WriteLine($"Cancelling {unitType} unit");
                        commands.AddRange(builder.Order(MacroData.Frame, Abilities.CANCEL_LAST));
                    }
                }
            }

            CancelRequests.Clear();

            return commands;
        }
    }
}
