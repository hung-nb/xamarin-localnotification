using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using localnotification.Notification;
using Xamarin.Forms;

[assembly: Dependency(typeof(localnotification.Droid.Notification.AndroidNotificationManager))]
namespace localnotification.Droid.Notification
{
    public class AndroidNotificationManager : INotificationManager
    {
        bool mChannelInitialized = false;
        NotificationManager mManager;

        const string mChannelId = "default";
        const string mChannelName = "Default";
        const string mChannelDescription = "The default channel for notifications.";
        int mPendingIntentId = 0;
        int mMessageId = 0;

        private Context mContext;
        public static String NOTIFICATION_CHANNEL_ID = "10023";

        public AndroidNotificationManager()
        {
            mContext = Android.App.Application.Context;
            mManager = (NotificationManager)mContext.GetSystemService(Android.App.Application.NotificationService);
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!mChannelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                //Intent intent = new Intent(mContext, typeof(AlarmHandler));
                //intent.PutExtra(TitleKey, title);
                //intent.PutExtra(MessageKey, message);

                //PendingIntent pendingIntent = PendingIntent.GetBroadcast(mContext, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                //long triggerTime = GetNotifyTime(notifyTime.Value);
                //AlarmManager alarmManager = mContext.GetSystemService(Context.AlarmService) as AlarmManager;
                //alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Show(title, message);
            }
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(mChannelName);
                var channel = new NotificationChannel(mChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = mChannelDescription
                };
                mManager.CreateNotificationChannel(channel);
            }

            mChannelInitialized = true;
        }

        private void Show(string title, string message)
        {
            try
            {
                Intent intent = new Intent(mContext, typeof(MainActivity));
                intent.PutExtra("title", title);
                intent.PutExtra("message", message);

                PendingIntent pendingIntent = PendingIntent.GetActivity(mContext, mPendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(mContext, NOTIFICATION_CHANNEL_ID)
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
                    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

                var notification = builder.Build();
                mManager.Notify(mMessageId++, notification);
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
