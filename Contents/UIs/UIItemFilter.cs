using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;

namespace ItemConduits.Contents.UIs
{
    public class UIItemFilter : UIElement
    {
        public Func<Item> ItemGet { get; set; }
        public Action<Item> ItemSet { get; set; }

        public UIItemFilter(Func<Item> itemGet, Action<Item> itemSet)
        {
            ItemGet = itemGet;
            ItemSet = itemSet;
            Width.Set(0, 1f);
            Height.Set(0, 1f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var item = ItemGet();

            if (IsMouseHovering)
            {
                ItemSlot.MouseHover(ref item, ItemSlot.Context.ChestItem);
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    if (Main.mouseItem.IsAir)
                        item = new();
                    else
                        item = new(Main.mouseItem.type);
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
