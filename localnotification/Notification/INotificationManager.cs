using System;
namespace localnotification.Notification
{
    public interface INotificationManager
    {
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message);
    }
}
