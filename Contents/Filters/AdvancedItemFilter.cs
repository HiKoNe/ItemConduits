using ConduitLib.Contents.Items;
using Terraria;
using Terraria.ID;

namespace ItemConduits.Contents.Filters
{
    public class AdvancedItemFilter : AbstractItemFilter
    {
        public override int FiltersCount => 6;
        public override bool Condition(int index, object obj) => obj is Item item && !conditions[index].IsAir && conditions[index].type == item.type;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<AdvancedEmptyConduitFilter>()
                .AddIngredient<ItemFilter>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
