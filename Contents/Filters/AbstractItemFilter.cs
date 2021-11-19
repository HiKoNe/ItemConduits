using ItemConduits.ConduitLib.APIs;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using System;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace ItemConduits.Contents.Filters
{
    public abstract class AbstractItemFilter : ModItem, IFilter
    {
        protected Item[] conditions;

        public bool IsWhitelist { get; set; } = true;
        public object this[int index] { get => conditions[index]; set => conditions[index] = (Item)value; }

        public abstract int FiltersCount { get; }
        public abstract bool Condition(int index, object obj);

        public override void SetDefaults()
        {
            base.SetDefaults();
            conditions = new Item[FiltersCount];
            Array.Fill(conditions, new());

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.maxStack = 1;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) =>
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.EverythingElse;

        public override void SaveData(TagCompound tag)
        {
            base.LoadData(tag);
            tag["conditions"] = (from condition in conditions select ItemIO.Save(condition)).ToList();
            tag["whitelist"] = IsWhitelist;
        }

        public override void LoadData(TagCompound tag)
        {
            base.SaveData(tag);
            if (tag.ContainsKey("conditions"))
                conditions = (from condition in tag.GetList<TagCompound>("conditions") select ItemIO.Load(condition)).ToArray();
            if (tag.ContainsKey("whitelist"))
                IsWhitelist = tag.GetBool("whitelist");
        }

        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
            foreach (var condition in conditions)
                ItemIO.Send(condition, writer);
            writer.Write(IsWhitelist);
        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
            for (int i = 0; i < FiltersCount; i++)
                conditions[i] = ItemIO.Receive(reader);
            IsWhitelist = reader.ReadBoolean();
        }
    }
}
