using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace ItemConduits.Contents.UIs
{
    public class UIInputText : UIPanel
    {
        protected int cursorTimer;
        protected int cursorPosition;
        protected bool focus;
        protected int cd;
        
        public Func<string> TextGet { get; set; }
        public Action<string> TextSet { get; set; }
        public bool Big { get; set; }
        public bool Focus
        {
            get => focus;
            set
            {
                focus = value;
                if (focus)
                    BorderColor = Color.Yellow;
                else
                    BorderColor = Color.Black;
            }
        }
        public Asset<DynamicSpriteFont> Font { get; set; }
        public Color TextColor { get; set; }
        public Vector2 MeasureText { get; set; }
        public int CursorPosition { get => cursorPosition; set => cursorPosition = Math.Clamp(value, 0, TextGet().Length); }

        public UIInputText(Func<string> textGet, Action<string> textSet)
        {
            Width.Set(0, 1f);
            Height.Set(0, 1f);
            TextGet = textGet;
            TextSet = textSet;
            Big = false;
            Focus = false;
            Font = FontAssets.MouseText;
            TextColor = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            cursorTimer++;
            cursorTimer %= 60;

            if (!IsMouseHovering && (Main.mouseLeft || Main.mouseRight))
                Focus = false;
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            Focus ^= true;
            CursorPosition = int.MaxValue;
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
            CursorPosition = 0;
            TextSet("");
        }

        bool curCD;
        bool lastCD;
        bool CD()
        {
            curCD = true;
            cursorTimer = 0;
            if (!lastCD)
            {
                cd = 30;
                return true;
            }
            if (--cd <= 0)
            {
                cd = 2;
                return true;
            }
            return false;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            curCD = false;

            var pos = GetDimensions().Position() + new Vector2(8, 3);

            string text = TextGet();

            if (Focus)
            {
                Main.LocalPlayer.mouseInterface = true;
                PlayerInput.WritingText = true;
                Main.instance.HandleIME();

                var newText = Main.GetInputText("");
                text = text.Insert(CursorPosition, newText);
                cursorPosition += newText.Length;

                if (IsKey(Keys.Left) && CD())
                    CursorPosition--;

                if (IsKey(Keys.Right) && CD())
                    CursorPosition++;

                if (IsKey(Keys.Back) && CursorPosition > 0 && CD())
                    text = text.Remove(--CursorPosition, 1);

                if (IsKey(Keys.Delete) && CursorPosition < text.Length && CD())
                    text = text.Remove(--CursorPosition, 1);

                TextSet(text);
            }

            DrawString(spriteBatch, text, pos);

            if (Focus && cursorTimer < 30)
            {
                float cursorX = Font.Value.MeasureString(text.Substring(0, CursorPosition)).X;
                DrawString(spriteBatch, "|", pos + new Vector2(cursorX - 3, 0));
            }

            if (!curCD && lastCD)
                cd = 0;

            lastCD = curCD;
        }

        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 pos)
        {
            if (Big)
                MeasureText = Utils.DrawBorderStringBig(spriteBatch, text, pos, TextColor);
            else
                MeasureText = Utils.DrawBorderString(spriteBatch, text, pos, TextColor);
        }

        public static bool IsKey(Keys key)
        {
            return Main.keyState.IsKeyDown(key); //&& !Main.oldKeyState.IsKeyDown(key);
        }
    }
}
