using System;
using Android.App;
using Android.Content;
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
        public string mTitleKey = "title";
        public string mMessageKey = "message";

        public static AndroidNotificationManager mInstance { get; private set; }

        public AndroidNotificationManager()
        {
            mContext = Android.App.Application.Context;
            mManager = (NotificationManager)mContext.GetSystemService(Android.App.Application.NotificationService);
            mInstance = this;
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!mChannelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                Intent intent = new Intent(mContext, typeof(AlarmHandler));
                intent.PutExtra(mTitleKey, title);
                intent.PutExtra(mMessageKey, message);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(mContext, mPendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = mContext.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Show(title, message);
            }
        }

        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationImportance importance = NotificationImportance.High;

                NotificationChannel notificationChannel = new NotificationChannel(mChannelId, mChannelName, importance)
                {
                    Description = mChannelDescription
                };
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                if (mManager != null)
                {
                    mManager.CreateNotificationChannel(notificationChannel);
                }
            }

            mChannelInitialized = true;
        }

        public void Show(string title, string message)
        {
            try
            {
                Intent intent = new Intent(mContext, typeof(MainActivity));
                intent.PutExtra(mTitleKey, title);
                intent.PutExtra(mMessageKey, message);

                PendingIntent pendingIntent = PendingIntent.GetActivity(mContext, mPendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(mContext, mChannelId)
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
                    .SetVisibility((int)NotificationVisibility.Public)
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
