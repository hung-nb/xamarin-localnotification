using System;
namespace localnotification.Notification
{
    public interface INotificationManager
    {
        void CreateNotification(String title, String message);
    }
}
