using ConduitLib;
using ItemConduits.Contents;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ItemConduits
{
    public class GlobalTiles : GlobalTile
    {
        public override bool PreHitWire(int i, int j, int type)
        {
            bool flag = base.PreHitWire(i, j, type);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return flag;

            if (ConduitUtil.TryGetConduit<ItemConduit>(i, j, out var conduit)
                && conduit.WireMode && conduit.IsConnector && conduit.Output)
                conduit.OnUpdate();

            return flag;
        }
    }
}
