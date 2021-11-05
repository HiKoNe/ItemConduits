using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace ItemConduits
{
    public static class ModAsset
    {
        static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.SetProperty;

        public static Asset<Texture2D>[] ItemConduit { get; set; }

        internal static void Load(Mod mod)
        {
            foreach (var memberInfo in typeof(ModAsset).GetMembers(flags))
            {
                if (memberInfo is PropertyInfo propertyInfo)
                {
                    int assetID = 0;
                    var assetList = new List<Asset<Texture2D>>();
                    while (mod.RequestAssetIfExists<Texture2D>($"Assets/{memberInfo.Name}_{assetID++}", out var asset))
                        assetList.Add(asset);
                    propertyInfo.SetValue(null, assetList.ToArray());
                }
            }
        }

        internal static void Unload()
        {
            ItemConduit = null;
        }
    }
}
