using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using OZ.MonoGame.MathFun.GameEngine;
using OZ.MonoGame.MathFun.GameEngine.Cards;
using OZ.MonoGame.MathFun.GameObjects;
using OZ.MonoGame.MathFun.GameObjects.Effects;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.GameObjects.UI;

namespace OZ.MonoGame.MathFun.Games
{
    public class MemoryGameControl : IGameObject
    {
        public Game Parent { get; private set; }

        MathCardBuilder<CardDraw, MathCardDraw> _mathCardBuilder;
        CardsBuilder _cardsBuilder;


        Player _player1, _player2;

        DrawScoreBars _drawScoreBars;
        Engine<CardDraw> _gameEngine;
        CardsCollection _cardsCollection;
        WinnerMessage _winnerMessage;

        CardsMatchEffect _star1, _star2;

        public bool IsGameStarted => _gameEngine.IsGameStarted;
        public bool IsGameFinished => _gameEngine.IsGameFinished;
        public bool IsPause { get; private set; }

        float margin = 10;
        const float BOARD_PART_OF_WINDOW = 4f / 5f;

        const double DELAY_BEFORE_CHECK_MATHCING = 1000;
        const double DELAY_IN_MATHCING = 600;


        Vector2 boardSize;
        Vector2 boardLocation;
        Rectangle BoardRectangle => new Rectangle(boardLocation.ToPoint(), boardSize.ToPoint());

        public MemoryGameControl(Game parent, GraphicsDeviceManager graphics)
        {
            Parent = parent;

            _cardsCollection = new CardsCollection(Parent);

            _mathCardBuilder = new MathCardBuilder<CardDraw, MathCardDraw>(1, 10,
                                                                    (op, num1, num2) =>
                                                                    {
                                                                        if (op == Operations.Sub)
                                                                        {
                                                                            int max = Math.Max(num1, num2);
                                                                            int min = Math.Min(num1, num2);
                                                                            num1 = max;
                                                                            num2 = min;
                                                                        }
                                                                        return new MathCardDraw(Parent,new MathCard(op, num1, num2))                                                                       { GameParent = Parent, Parent=_cardsCollection };
                                                                    },
                                                                    (result) => new NumCardDraw(Parent, result) {Parent=_cardsCollection });


            _cardsBuilder = new CardsBuilder();
            _gameEngine = new Engine<CardDraw>(_mathCardBuilder, _cardsBuilder);
            _cardsBuilder.Engine = _gameEngine;
            _cardsBuilder.CardsCollection = _cardsCollection;


            _drawScoreBars = new DrawScoreBars(Parent)
            {
                Location = Vector2.Zero,
                Size = new Vector2()
                {
                    X = graphics.PreferredBackBufferWidth,
                    Y = graphics.PreferredBackBufferHeight * (1 - BOARD_PART_OF_WINDOW) - margin * 2
                }
            };

            _winnerMessage = new WinnerMessage()
            {
                Rect = new Rectangle()
                {
                    Size = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight * 1 / 3).ToPoint(),
                    Location = Vector2.Zero.ToPoint()
                },
                Size = new Vector2(graphics.PreferredBackBufferWidth - 100)
            };

            _player1 = new Player() { Name = "Player1" };
            _player2 = new Player() { Name = "Player2" };
        }

        public void StartNewGame(int level = 1)
        {
            int rows = 0, columns = 0, minNumber = 1, maxNumber = 0;

            switch (level)
            {
                case 1:
                    rows = 3;
                    columns = 4;
                    maxNumber = 4;
                    break;
                case 2:
                    rows = 3;
                    columns = 6;
                    maxNumber = 7;
                    break;
                case 3:
                    rows = 4;
                    columns = 7;
                    maxNumber = 10;
                    break;
                default:
                    break;
            }

            _mathCardBuilder.MinNumber = minNumber;
            _mathCardBuilder.MaxNumber = maxNumber;

            _gameEngine.InitBoard(rows, columns);

            CalculateBoardSize();

            _winnerMessage.Initialize();
            _gameEngine.StartNewGame();
        }

        public void AssignPlayersName(string player1, string player2)
        {
            _player1.Name = player1;
            _player2.Name = player2;
        }

        public void Pause()
        {
            if (IsGameStarted)
            {
                IsPause = true;
            }
        }

        public void Continue()
        {
            if (IsGameStarted)
            {
                IsPause = false;
            }
        }

        public void Initialize()
        {
            _drawScoreBars.Initialize();
            _drawScoreBars.InitPlayers(_player1, _player2);
            _gameEngine.Player1 = _player1;
            _gameEngine.Player2 = _player2;

            _star1 = new CardsMatchEffect();
            _star2 = new CardsMatchEffect();
            Parent.Animations.AddRange(new[] { _star1, _star2 });

            _cardsCollection.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            _cardsCollection.LoadContent(Parent.Content);

            _winnerMessage.Font = ScoreBar.Font = content.Load<SpriteFont>("fonts/playerFont");

            _drawScoreBars.LoadContent(content);

            _winnerMessage.LoadContent(content);

            CardsMatchEffect.Star = content.Load<Texture2D>("spritesEffect/star");
        }

        private void CalculateBoardSize()
        {
            int width = Parent.Graphics.PreferredBackBufferWidth;
            int height = (int)(Parent.Graphics.PreferredBackBufferHeight * BOARD_PART_OF_WINDOW);

            _cardsCollection.Size = new Vector2()
            {
                X = width,
                Y = height
            };

            _cardsCollection.Location = new Vector2(10, Parent.Graphics.PreferredBackBufferHeight - height);


            _cardsCollection.RectangleOfContentDrawing = new Rectangle()
            {
                Location = new Point(100),
                Size = new Point(943 - 100, 600 - 100)
            };
        }


        public void UnloadContent(ContentManager content)
        {
            _cardsCollection.UnloadContent(content);
            _drawScoreBars.UnloadContent(content);
            _winnerMessage.UnloadContent(content);
        }


        bool _checkingMatchProccess = false;
        double _delay;
        bool _isMouseAlreadyPressed = false;
        public void Update(GameTime gameTime)
        {
            if (_gameEngine.IsGameStarted && !IsPause)
            {
                if (_gameEngine.ThereIs2VisibleCards)
                {
                    // Start process of checking matching
                    if (_delay == 0)
                    {
                        _checkingMatchProccess = true;
                        if (_gameEngine.CheckMatch(true))
                        {
                            StartMatchEffect(gameTime);
                            _delay = DELAY_IN_MATHCING;
                        }
                        else
                        {
                            _delay = DELAY_BEFORE_CHECK_MATHCING;
                        }
                    }

                    // Still in delay
                    if (_delay > 0)
                    {
                        _delay -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    else // Stop delay and check matching
                    {
                        _delay = 0;
                        _gameEngine.CheckMatch();
                        _checkingMatchProccess = false;
                    }
                }
                _cardsCollection.Update(gameTime);
                UpdatePlayers(gameTime);
            }

            GetInputPosition(out Vector2? dummy, out bool isPressing);
            _isMouseAlreadyPressed = isPressing;
        }

        private void StartMatchEffect(GameTime gameTime)
        {
            var board = _gameEngine.Board;
            var visibleCards = _gameEngine.VisibleCards;
            Vector2 source1 = board[visibleCards[0]].Middle;
            Vector2 source2 = board[visibleCards[1]].Middle;
            Vector2 destination = _drawScoreBars.TurnOf.MiddleOfScore;

            _star1.StartAnimate(source1, destination, DELAY_IN_MATHCING);
            _star2.StartAnimate(source2, destination, DELAY_IN_MATHCING);
        }
        private void CardshoverChecking(GameTime gameTime)
        {
            Vector2? positionInput = Vector2.One * -1;
            GetInputPosition(out positionInput, out bool isPressing);
            if (positionInput.HasValue)
            {
                if (!_checkingMatchProccess)
                {
                    foreach (var card in _cardsCollection)
                    {
                        card.IsHover = card.IsContains(positionInput.Value.ToPoint());
                        card.Update(gameTime);
                    }
                }
            }
        }

        bool _isTouched = false;
        Vector2 _oldTouchPosition;
        private void GetInputPosition(out Vector2? position, out bool isPressing)
        {
#if ANDROID
            var touch = TouchPanel.GetState();
            position = null;
            isPressing = false;
            if (touch.IsConnected)
            {
                if (touch.Count >= 1)
                {
                    var item = touch[0];
                    position = _oldTouchPosition = item.Position;
                    isPressing = item.State == TouchLocationState.Moved || item.State == TouchLocationState.Pressed;
                    _isTouched = true;
                }
                else if (_isTouched)
                {
                    position = _oldTouchPosition;
                    isPressing = false;
                    _isTouched = false;
                }

            }

#elif WINDOWS
            position = Mouse.GetState().Position.ToVector2();
            isPressing = Mouse.GetState().LeftButton == ButtonState.Pressed;
#endif

        }
        private void UpdatePlayers(GameTime gameTime)
        {
            if (!_gameEngine.IsGameFinished)
            {
                _drawScoreBars.Player2ScoreBar.IsMyTurn = !(_drawScoreBars.Player1ScoreBar.IsMyTurn = _gameEngine.TurnOf == _drawScoreBars.Player1ScoreBar.Player);
            }
            else
            {
                _winnerMessage.WinnerName = _gameEngine.Winner != null ? _gameEngine.Winner.Name : "no one";
                _winnerMessage.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsGameStarted)
            {
                _cardsCollection.Draw(gameTime, spriteBatch);
                _drawScoreBars.Draw(gameTime, spriteBatch);

                if (_gameEngine.IsGameFinished)
                {
                    _winnerMessage.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
