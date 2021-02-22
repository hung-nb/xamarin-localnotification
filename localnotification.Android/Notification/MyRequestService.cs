using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using static Android.OS.PowerManager;

namespace localnotification.Droid.Notification
{
    [Service(Enabled = true)]
    public class MyRequestService : Service
    {
        private Handler handler;
        private Action runnable;
        private bool isStarted;
        private WakeLock wakeLock;
        private long DELAY_BETWEEN_LOG_MESSAGES = 15000;
        private int NOTIFICATION_SERVICE_ID = 999999;
        private int NOTIFICATION_SERVICE_ALARM_ID = 9999990;
        private string NOTIFICATION_CHANNEL_ID = "888888";
        private string NOTIFICATION_CHANNEL_NAME = "channel_name_123";

        public override void OnCreate()
        {
            base.OnCreate();

            handler = new Handler();

            runnable = new Action(() =>
            {
                DispatchNotificationThatAlarmIsGenerated("I'm running");
                handler.PostDelayed(runnable, DELAY_BETWEEN_LOG_MESSAGES);
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (isStarted)
            {
                // service is already started
            }
            else
            {
                CreateNotificationChannel();
                DispatchNotificationThatServiceIsRunning();

                handler.PostDelayed(runnable, DELAY_BETWEEN_LOG_MESSAGES);
                isStarted = true;

                PowerManager powerManager = (PowerManager)this.GetSystemService(Context.PowerService);
                WakeLock wakeLock = powerManager.NewWakeLock(WakeLockFlags.Full, "Client Lock");
                wakeLock.Acquire();
            }
            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            //base.OnTaskRemoved(rootIntent);
        }

        public override IBinder OnBind(Intent intent)
        {
            // Return null because this is a pure started service. A hybrid service would return a binder that would
            // allow access to the GetFormattedStamp() method.
            return null;
        }

        public override void OnDestroy()
        {
            // Stop the handler.
            handler.RemoveCallbacks(runnable);

            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(NOTIFICATION_SERVICE_ID);

            isStarted = false;
            wakeLock.Release();
            base.OnDestroy();
        }

        private void CreateNotificationChannel()
        {
            //Notification Channel
            NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME, NotificationImportance.Max);
            notificationChannel.EnableLights(true);
            notificationChannel.LightColor = Color.Red;
            notificationChannel.EnableVibration(true);
            notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });


            NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(notificationChannel);
        }

        private void DispatchNotificationThatServiceIsRunning()
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                   .SetDefaults((int)NotificationDefaults.All)
                   .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
                   .SetVibrate(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 })
                   .SetSound(null)
                   .SetChannelId(NOTIFICATION_CHANNEL_ID)
                   .SetPriority(NotificationCompat.PriorityDefault)
                   .SetAutoCancel(false)
                   .SetContentTitle("Mobile")
                   .SetContentText("My service started")
                   .SetOngoing(true);

            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(this);

            //notificationManager.Notify(NOTIFICATION_SERVICE_ID, builder.Build());
            StartForeground(NOTIFICATION_SERVICE_ID, builder.Build());
        }

        private void DispatchNotificationThatAlarmIsGenerated(string message)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
                .SetContentTitle("Alarm")
                .SetContentText(message)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(NOTIFICATION_SERVICE_ALARM_ID, notificationBuilder.Build());
        }
    }
}
