using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    public class NumCard : Card
    {
        public float Number { get; }
        public override float Value => Number;

        public NumCard(float number) => Number = number;

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
