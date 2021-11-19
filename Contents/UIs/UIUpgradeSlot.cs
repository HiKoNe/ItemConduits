using ConduitLib;
using ConduitLib.UIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;

namespace ItemConduits.Contents.UIs
{
    public class UIUpgradeSlot : UIElement, IUIDescription
    {
        public Func<Item> ItemGet { get; set; }
        public Action<Item> ItemSet { get; set; }
        public Func<string> Description { get; set; }

        public UIUpgradeSlot(Func<Item> itemGet, Action<Item> itemSet)
        {
            ItemGet = itemGet;
            ItemSet = itemSet;
            Width.Set(26f, 0);
            Height.Set(26f, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var item = ItemGet();

            if (IsMouseHovering)
            {
                ItemSlot.MouseHover(ref item);
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    ItemSlot.LeftClick(ref item);
                    ItemSet(item);
                }
            }

            var oldScale = Main.inventoryScale;
            Main.inventoryScale = 0.5f;
            ItemSlot.Draw(spriteBatch, ref item, ItemSlot.Context.ChestItem, GetDimensions().Position(), Color.White);
            Main.inventoryScale = oldScale;
        }
    }
}
