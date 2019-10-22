using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public static class UIApearance
    {
        public static Texture2D CardReg { get; set; }
        public static Texture2D HoverCard { get; set; }
        public static Texture2D CardPressed { get; set; }
        public static SpriteFont CardTextFont { get; set; }

        public static SoundEffect CardHoverEffect { get; set; }
        public static SoundEffect CardPressedEffect { get; set; }

        public static Texture2D ScoreBar { get; set; }
        public static Texture2D GlowScoreBar { get; set; }
        public static SpriteFont ScoreBarFont { get; set; }

        public static Texture2D BtnReg
        {
            get;
            set;
        }
        public static Texture2D BtnHovered { get; set; }
        public static Texture2D BtnPressed { get; set; }
        public static Texture2D BtnEnable { get; set; }

        public static Texture2D TextBoxReg { get; set; }
        public static Texture2D TextBoxHovered { get; set; }
        public static Texture2D TextBoxPressed { get; set; }

        public static Texture2D MenuBkg { get; set; }
        public static SpriteFont Font { get; set; }
    }
}
