using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    public class MathCardBuilder<TCard,TMathCard> : ICardBuilder<TCard>
        where TCard : class, ICard
        where TMathCard : TCard, IMathCard
        
    {
        public delegate TMathCard CreateMathCardDelegate(Operations operation, int num1, int num2);
        public delegate TCard CreateResultCardDelegate(float result);


        public CreateMathCardDelegate CreateMathCard { get; set; }
        public CreateResultCardDelegate CreateResultCard { get; set; }

        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }

        public MathCardBuilder(int min, int max, CreateMathCardDelegate createMathCard, CreateResultCardDelegate createResultCard)
        {
            MinNumber = min;
            MaxNumber = max;
            CreateMathCard = createMathCard;
            CreateResultCard = createResultCard;
        }

        private Random rand = new Random();
        public TCard[] GeneratePairCard()
        {           
            Operations operation = (Operations)rand.Next(0, 3);

            TMathCard mathCard = CreateMathCard(operation,
                                            rand.Next(MinNumber, MaxNumber+1),
                                            rand.Next(MinNumber, MaxNumber+1));
            TCard numCard =CreateResultCard(mathCard.Calculate());

            return new TCard[2] { mathCard, numCard };
        }
    }
}
