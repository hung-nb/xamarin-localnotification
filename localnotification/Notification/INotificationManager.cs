using System;
namespace localnotification.Notification
{
    public interface INotificationManager
    {
        void SendNotification(String title, String message, DateTime? notifyTime);
    }
}
