using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ItemConduits.Contents.Upgrades
{
    public class StackUpgrade : ModItem
    {
        public override void SetStaticDefaults() =>
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 4;

        public override void SetDefaults()
        {
            Item.maxStack = 8;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) =>
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.EverythingElse;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Multiplier", Language.GetTextValue("Mods.ItemConduits.UI.Multiplier", Math.Pow(2, Item.stack))));
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Actuator, 10)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
