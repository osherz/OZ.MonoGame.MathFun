using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    public interface IMathCard : ICard
    {
        Operations Operation { get; }

        int Num1 { get; }
        int Num2 { get; }

        float Calculate();

        // return string in format num1(operation)num2
        // for example 2+3
        string ToString();
    }
}
