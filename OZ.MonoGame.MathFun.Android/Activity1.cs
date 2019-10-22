#define ANDROID

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Graphics;
using Android.Views;

namespace OZ.MonoGame.MathFun.Android
{
    [Activity(Label = "MathFun"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.UserLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        private readonly bool _setImmersive = false;
        private View _view;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            GetDisplaySize(out int width, out int height);

            var g = new Game(width, height);
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }

        private void GetDisplaySize(out int width, out int height)
        {
            var realSize = new Point();
            WindowManager.DefaultDisplay.GetRealSize(realSize);

            System.Console.WriteLine("Default Real Size: " + realSize);

            var rectSize = new Rect();
            WindowManager.DefaultDisplay.GetRectSize(rectSize);

            System.Console.WriteLine("Default Rect Size: " + rectSize);

            if (_setImmersive) { width = realSize.X; height = realSize.Y; }
            else { width = rectSize.Width(); height = rectSize.Height(); }
        }
    }
}

