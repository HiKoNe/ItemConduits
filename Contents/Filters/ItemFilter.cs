﻿using ConduitLib.Contents.Items;
using Terraria;
using Terraria.ID;

namespace ItemConduits.Contents.Filters
{
    public class ItemFilter : AbstractItemFilter
    {
        public override int FiltersCount => 3;
        public override bool Condition(int index, object obj) => obj is Item item && !conditions[index].IsAir && conditions[index].type == item.type;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 20, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<EmptyConduitFilter>()
                .AddIngredient<ItemItemConduit>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
