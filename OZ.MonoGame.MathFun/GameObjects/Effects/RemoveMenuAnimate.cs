using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OZ.MonoGame.GameObjects.UI;
using OZ.MonoGame.GameObjects.Effects;

namespace OZ.MonoGame.MathFun.GameObjects.Effects
{
    public class RemoveMenuAnimate : MoveToAnimation
    {
        public RemoveMenuAnimate(Panel panel)
        {
            Obj = panel;
            Destination = new Vector2()
            {
                X = -1*(panel.Size.X*1.5f),
                Y = Obj.Location.Y
            };
        }
    }
}
