﻿using SC2APIProtocol;

namespace Sharky
{
    public class TargetingData
    {
        public Point2D AttackPoint { get; set; }
        public Point2D ForwardDefensePoint { get; set; }
        public Point2D MainDefensePoint { get; set; }
        public Point2D SelfMainBasePoint { get; set; }
        public Point2D EnemyMainBasePoint { get; set; }
    }
}