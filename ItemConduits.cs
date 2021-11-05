using ItemConduits.Contents;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ItemConduits
{
	public class ItemConduits : Mod
	{
        public override void Load()
        {
            ModAsset.Load(this);
        }

        public override void Unload()
        {
            ModAsset.Unload();
        }

        public override void AddRecipes()
        {
            base.AddRecipes();

            CreateRecipe(ItemType<ItemItemConduit>())
                .AddIngredient(ItemID.Actuator)
                .AddIngredient(ItemType<ConduitShell>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}