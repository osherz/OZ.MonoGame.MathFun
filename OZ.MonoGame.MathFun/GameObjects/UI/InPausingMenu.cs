using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public class InPausingMenu : GameMenu
    {
        private MenuButton _resumeBtn;
        private MenuButton _mainMenuBtn;
        private MenuButton _exitBtn;
        
        #region Events
        public event EventHandler ResumeBtnClicked { add => _resumeBtn.Clicked += value; remove => _resumeBtn.Clicked -= value; }
        public event EventHandler LevelsBtnClicked { add => _mainMenuBtn.Clicked += value; remove => _mainMenuBtn.Clicked -= value; }
        public event EventHandler ExitBtnClicked { add => _exitBtn.Clicked += value; remove => _exitBtn.Clicked -= value; }
        #endregion Events


        public InPausingMenu(GamePrototype gameParent) : base(gameParent)
        {
            Text = "Pause";
        }

        public override void Initialize()
        {
            InitButtons();
            base.Initialize();
        }

        private void InitButtons()
        {
            _resumeBtn = new MenuButton(GameParent) { Text = "Resume" };
            _mainMenuBtn = new MenuButton(GameParent) { Text = "Levels" };
            _exitBtn = new MenuButton(GameParent) { Text = "Exit" };

            foreach (var btn in new[] { _resumeBtn, _mainMenuBtn, _exitBtn })
            {
                btn.IsEnabled = true;
                btn.ControlApearance = ButtonsApearance;
                Add(btn);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            InnerToMiddleX();
        }
    }
}