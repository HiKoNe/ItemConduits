using ItemConduits.ConduitLib.APIs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ItemConduits.Contents.Filters
{
    public class ItemFilter : ModItem, IFilter
    {
        protected IList<int> conditions = new List<int>();

        public bool IsWhitelist { get; set; }

        public int FiltersCount => 5;

        public bool Conditions(int index, object obj)
        {
            var item = (Item)obj;
            if (index < conditions.Count)
            {
                return conditions[index] == item.type;
            }
            return conditions.Count == 0;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.maxStack = 1;
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            tag["conditions"] = conditions;
            tag["whitelist"] = IsWhitelist;
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            if (tag.ContainsKey("conditions"))
                conditions = tag.GetList<int>("conditions");
            if (tag.ContainsKey("whitelist"))
                IsWhitelist = tag.GetBool("whitelist");
        }

        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
            writer.Write((byte)conditions.Count);
            foreach (var condition in conditions)
                writer.Write(condition);
            writer.Write(IsWhitelist);
        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
            conditions = new List<int>();
            for (int i = 0; i < reader.ReadByte(); i++)
                conditions.Add(reader.ReadInt32());
            IsWhitelist = reader.ReadBoolean();
        }
    }
}
