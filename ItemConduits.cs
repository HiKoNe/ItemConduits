using Terraria.ModLoader;

namespace ItemConduits
{
	public class ItemConduits : Mod
	{
        public override void Load()
        {
            ModAsset.Load(this);
        }

        public override void Unload()
        {
            ModAsset.Unload();
        }
    }
}