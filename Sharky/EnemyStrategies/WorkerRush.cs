﻿using Sharky.Managers;
using System.Linq;
using System.Numerics;

namespace Sharky.EnemyStrategies
{
    public class WorkerRush : EnemyStrategy
    {
        ITargetingManager TargetingManager;

        public WorkerRush(EnemyStrategyHistory enemyStrategyHistory, IChatManager chatManager, IUnitManager unitManager, SharkyOptions sharkyOptions, ITargetingManager targetingManager)
        {
            EnemyStrategyHistory = enemyStrategyHistory;
            ChatManager = chatManager;
            UnitManager = unitManager;
            SharkyOptions = sharkyOptions;
            TargetingManager = targetingManager;
        }

        protected override bool Detect(int frame)
        {
            if (frame < SharkyOptions.FramesPerSecond * 60)
            {
                if (UnitManager.EnemyUnits.Values.Count(u => u.UnitClassifications.Contains(UnitClassification.Worker) && Vector2.DistanceSquared(new Vector2(TargetingManager.EnemyMainBasePoint.X, TargetingManager.EnemyMainBasePoint.Y), new Vector2(u.Unit.Pos.X, u.Unit.Pos.Y)) > (40 * 40)) >= 5)
                {
                    return true;
                }
            }

            return false;
        }
    }
}