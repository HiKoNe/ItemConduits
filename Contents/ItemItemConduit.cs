using ConduitLib.APIs;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace ItemConduits.Contents
{
    public class ItemItemConduit : AbstractItemConduit<ItemConduit>
    {
        public override void SetStaticDefaults() =>
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) =>
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Material;
    }
}
