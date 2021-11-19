using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ItemConduits
{
	public class ItemConduits : Mod
	{
        public static Dictionary<int, string> AllToolTips { get; private set; }

        public override void Load()
        {
            ModAsset.Load(this);
        }

        public override void Unload()
        {
            ModAsset.Unload();
            AllToolTips.Clear();
        }

        public override void PostSetupContent()
        {
            AllToolTips = new();
            for (int i = 1; i < ItemLoader.ItemCount; i++)
            {
                var item = new Item();
                item.SetDefaults(i);
                if (item.type == ItemID.None)
                    continue;

                int yoyoLogo = 0;
                int researchLine = 0;
                int numLines = 1;
                int num2 = 30;
                string[] toolTipLine = new string[num2];
                bool[] preFixLine = new bool[num2];
                bool[] badPreFixLine = new bool[num2];
                string[] tooltipNames = new string[num2];
                Main.MouseText_DrawItemTooltip_GetLinesInfo(item, ref yoyoLogo, ref researchLine, 0, ref numLines, toolTipLine, preFixLine, badPreFixLine, tooltipNames);
                ItemLoader.ModifyTooltips(item, ref numLines, tooltipNames, ref toolTipLine, ref preFixLine, ref badPreFixLine, ref yoyoLogo, out _);
                AllToolTips[item.type] = string.Join("\n", toolTipLine).ToLower();
            }
        }
    }
}