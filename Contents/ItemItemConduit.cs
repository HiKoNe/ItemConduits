using ConduitLib.APIs;
using ConduitLib.Contents.Items;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;

namespace ItemConduits.Contents
{
    public class ItemItemConduit : AbstractItemConduit<ItemConduit>
    {
        public override void SetStaticDefaults() =>
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 0, 5, 0);
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) =>
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Material;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Actuator)
                .AddIngredient<ConduitShell>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
