using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace PonderApp
{
    // the ConfigurationChanges flags set here keep the EGL context
    // from being destroyed whenever the device is rotated or the
    // keyboard is shown (highly recommended for all GL apps)
    [Activity(Label = "PonderApp",
                    ConfigurationChanges = ConfigChanges.KeyboardHidden,
                    ScreenOrientation = ScreenOrientation.SensorLandscape,
                    MainLauncher = true,
                    Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        GLView1 view;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create our OpenGL view, and display it
            view = new GLView1(this);
            SetContentView(view);
        }

        protected override void OnPause()
        {
            // never forget to do this!
            base.OnPause();
            view.Pause();
        }

        protected override void OnResume()
        {
            // never forget to do this!
            base.OnResume();
            view.Resume();
        }
    }
}