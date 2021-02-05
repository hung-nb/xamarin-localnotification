using System;
using localnotification.Notification;
using Xamarin.Forms;

namespace localnotification
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void SendNotification(object sender, EventArgs e)
        {
            DependencyService.Get<INotificationManager>().SendNotification("SPTutorials", message.Text, null);
        }
    }
}
