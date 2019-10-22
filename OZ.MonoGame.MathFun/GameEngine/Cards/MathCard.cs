using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    public class MathCard : Card, ICard, IMathCard
    {
        public Operations Operation { get; private set; }

        public int Num1 { get; }
        public int Num2 { get; }

        public override float Value => Calculate();

        public MathCard(Operations operation, int num1, int num2)
        {
            Operation = operation;
            this.Num1 = num1;
            this.Num2 = num2;
        }

         public float Calculate()
        {
            float result;
            switch (Operation)
            {
                case Operations.Multiply:
                    result = Num1 * Num2;
                    break;
                case Operations.Divide:
                    result = Num1 / Num2;
                    break;
                case Operations.Add:
                    result = Num1 + Num2;
                    break;
                case Operations.Sub:
                    result = Num1 - Num2;
                    break;
                default:
                    throw new NotSupportedException("Math operation unsupported.");
            }

            return result;
        }

        public override string ToString()
        {
            return Num1.ToString() + GetOperationTav(Operation) + Num2.ToString();
        }

        private char GetOperationTav(Operations operation)
        {
            switch (operation)
            {
                case Operations.Multiply:
                    return 'x';
                case Operations.Add:
                    return '+';
                case Operations.Sub:
                    return '-';
                case Operations.Divide:
                    return '/';
                default:
                    throw new NotSupportedException("Math operation unsupported.");
            }
        }
    }
}
