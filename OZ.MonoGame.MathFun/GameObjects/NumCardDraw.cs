using OZ.MonoGame.MathFun.GameEngine.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public class NumCardDraw : CardDraw
    {

        public NumCardDraw(GamePrototype gameParent, float result) : base(gameParent)
        {
            Card = new NumCard(result);
        }
    }
}
