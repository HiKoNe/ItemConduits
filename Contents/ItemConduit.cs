using ConduitLib;
using ConduitLib.APIs;
using ConduitLib.UIs.Elements;
using ItemContainerLib.APIs;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace ItemConduits.Contents
{
    public class ItemConduit : ModConduit
    {
        protected int cururrentRoundRobin;
        protected int priority;
        protected bool roundRobin;
        protected bool wireMode = true;

        public IItemContainer ItemContainer { get; set; }
        public bool RoundRobin { get => roundRobin; set { roundRobin = value; UpdateConduit(); } }
        public int Priority { get => priority; set { priority = value; UpdateConduit(); } }
        public bool WireMode { get => wireMode; set { wireMode = value; UpdateConduit(); } }
        public int MaxTransfer => 4;
        public override List<ModConduit> Network
        {
            get => base.Network; set
            {
                base.Network = value;

                if (network is not null)
                    network.Sort((c, c2) => ((ItemConduit)c2).Priority.CompareTo(((ItemConduit)c).Priority));
            }
        }

        public override int? UpdateDelay => WireMode ? null : 60;
        public override Asset<Texture2D> Texture => ModAsset.ItemConduit[0];
        public override Asset<Texture2D> WrenchIcon => TextureAssets.Item[ModContent.ItemType<ItemItemConduit>()];

        public override bool ValidForConnector()
        {
            if (ItemContainerUtil.TryGetItemContainer(Position.X, Position.Y, out var itemContainer))
            {
                this.ItemContainer = itemContainer;
                return true;
            }
            this.ItemContainer = null;
            return false;
        }

        public override void OnUpdate()
        {
            if (Output && Network is not null && ItemContainer is not null && !ItemContainer.IsEmpty())
            {
                for (int i = 0; i < ItemContainer.ContainerSize; i++)
                {
                    var item = ItemContainer[i];
                    if (item.stack < 1)
                        continue;

                    int toTransfer = item.stack = Math.Min(item.stack, MaxTransfer);

                    for (int c = 0; c < Network.Count; c++)
                    {
                        var itemConduit = (ItemConduit)Network[c];

                        if (RoundRobin)
                        {
                            if (cururrentRoundRobin > c)
                                continue;

                            cururrentRoundRobin = c + 1;

                            if (cururrentRoundRobin >= Network.Count)
                                cururrentRoundRobin = 0;
                        }

                        item = itemConduit.ItemContainer.AddItem(item);

                        if (item.stack < 1)
                            break;
                    }

                    if (ItemContainer.DecreaseItem(i, toTransfer - item.stack).stack == 0)
                        continue;
                    break;
                }
            }
        }

        public override void OnPlace()
        {
            SoundEngine.PlaySound(SoundID.Dig, Position.ToVector2() * 16);
        }

        public override void OnRemove()
        {
            var pos = Position.ToVector2() * 16;
            SoundEngine.PlaySound(SoundID.Dig, pos);
            Item.NewItem(pos, ModContent.ItemType<ItemItemConduit>(), 1, false, 0, true);
        }

        public override bool OnDraw(in SpriteBatch spriteBatch, ref int frameX, ref int frameY, ref float? alpha)
        {
            if (IsConnector)
            {
                frameX = 72;
                frameY = 0;
            }
            return base.OnDraw(spriteBatch, ref frameX, ref frameY, ref alpha);
        }

        public override void OnInitializeUI(ref UIPanel panel, StyleDimension rightDim, StyleDimension topDim)
        {
            var priority = new UIValue(Priority, Language.GetTextValue("Mods.ItemConduits.UI.Priority"))
            {
                Top = topDim,
                OnChange = (value) => Priority = (int)value,
            };
            panel.Append(priority);

            var roundRobin = new UIToggle(ModAsset.Button[0], ModAsset.Button[1], RoundRobin)
            {
                Outline = ConduitAsset.Button[0],
                Top = topDim,
                Left = rightDim,
                Description = () => RoundRobin ? Language.GetTextValue("Mods.ItemConduits.UI.RoundRobinEnable") : Language.GetTextValue("Mods.ItemConduits.UI.RoundRobinDisabled"),
                OnToggle = (toggle) => RoundRobin = toggle,
            };
            panel.Append(roundRobin);

            var wireMode = new UIToggle(ModAsset.Button[2], ModAsset.Button[3], WireMode)
            {
                Outline = ConduitAsset.Button[0],
                Top = topDim,
                Left = rightDim,
                Description = () => WireMode ? Language.GetTextValue("Mods.ItemConduits.UI.WireModeEnable") : Language.GetTextValue("Mods.ItemConduits.UI.WireModeDisabled"),
                OnToggle = (toggle) => WireMode = toggle,
            };
            wireMode.Left.Pixels += roundRobin.Width.Pixels + panel.PaddingLeft;
            panel.Append(wireMode);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["roundRobin"] = roundRobin;
            tag["priority"] = priority;
            tag["wireMode"] = wireMode;
        }

        public override void LoadData(TagCompound tag)
        {
            roundRobin = tag.GetBool("roundRobin");
            priority = tag.GetInt("priority");
            wireMode = tag.GetBool("wireMode");
        }
    }
}
