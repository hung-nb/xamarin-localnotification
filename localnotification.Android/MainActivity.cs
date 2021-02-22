using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using localnotification.Droid.Notification;

namespace localnotification.Droid
{
    [Activity(Label = "localnotification", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }




        public void StartMyRequestService()
        {
            var serviceToStart = new Intent(this, typeof(MyRequestService));
            StartService(serviceToStart);
        }

        public void StopMyRequestService()
        {
            var serviceToStart = new Intent(this, typeof(MyRequestService));
            StopService(serviceToStart);
        }



        protected override void OnPause()
        {
            base.OnPause();
            StartMyRequestService();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StartMyRequestService();
        }

        protected override void OnResume()
        {
            base.OnResume();
            StopMyRequestService();
        }
    }
}