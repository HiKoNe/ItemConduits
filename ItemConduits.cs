using Terraria.ModLoader;

namespace ItemConduits
{
	public class ItemConduits : Mod
	{
        public override void Load()
        {
            ItemContainerLib.APIs.ItemContainerUtil.TryGetItemContainer(0, 0, out _);
            base.Load();
        }
    }
}