using ConduitLib.Contents.Items;
using ItemConduits.ConduitLib.APIs;
using System.IO;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ItemConduits.Contents.Filters
{
    public class CustomItemFilter : ModItem, IFilter
    {
        protected string condition = "";

        public object this[int index] { get => condition; set => condition = (string)value; }

        public int FiltersCount => 1;
        public bool IsWhitelist { get; set; }

        public bool Condition(int index, object obj)
        {
            if (obj is Item item)
            {
                var str = ItemConduits.AllToolTips[item.type];

                foreach (var s in condition.Split('|'))
                {
                    foreach (var s2 in s.Trim().Split('&'))
                    {
                        if (s2.Trim().Length > 0 && !str.Contains(s2.Trim()))
                            goto l;
                    }
                    return true;
                    l:;
                }
            }
            return false;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void SaveData(TagCompound tag)
        {
            base.LoadData(tag);
            tag["condition"] = condition;
            tag["whitelist"] = IsWhitelist;
        }

        public override void LoadData(TagCompound tag)
        {
            base.SaveData(tag);
            if (tag.ContainsKey("condition"))
                condition = tag.GetString("condition");
            if (tag.ContainsKey("whitelist"))
                IsWhitelist = tag.GetBool("whitelist");
        }

        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
            writer.Write(condition);
            writer.Write(IsWhitelist);
        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
            condition = reader.ReadString();
            IsWhitelist = reader.ReadBoolean();
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) =>
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.EverythingElse;

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<AdvancedEmptyConduitFilter>()
                .AddIngredient<ItemItemConduit>()
                .AddIngredient<ItemItemConduit>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
