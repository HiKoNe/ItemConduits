using ConduitLib.APIs;
using ItemConduits.Contents;

namespace ItemConduits.Contents
{
    public class ItemItemConduit : AbstractItemConduit<ItemConduit>
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Item Conduit");
            Tooltip.SetDefault("For transfer Items");

            base.SetStaticDefaults();
        }
    }
}
