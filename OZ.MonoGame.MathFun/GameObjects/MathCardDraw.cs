using OZ.MonoGame.MathFun.GameEngine.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public class MathCardDraw : CardDraw, IMathCard
    {

        public MathCardDraw(GamePrototype gameParent, MathCard mathCard):base(gameParent)
        {
            Card = mathCard;
        }

        public Operations Operation => ((IMathCard)Card).Operation;

        public int Num1 => ((IMathCard)Card).Num1;

        public int Num2 => ((IMathCard)Card).Num2;

        public float Calculate()
        {
            return ((IMathCard)Card).Calculate();
        }
    }
}
