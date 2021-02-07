using System;
using Android.Content;

namespace localnotification.Droid.Notification
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra("title");
                string message = intent.GetStringExtra("message");

                AndroidNotificationManager.mInstance.Show(title, message);
            }
        }
    }
}
