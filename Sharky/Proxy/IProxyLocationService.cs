﻿using SC2APIProtocol;
using Sharky.Builds;

namespace Sharky.Proxy
{
    public interface IProxyLocationService
    {
        Point2D GetCliffProxyLocation(float offsetDistance = 0);
        Point2D GetGroundProxyLocation(float offsetDistance = 0);
        ProxyData? GetProxyData();
    }
}
