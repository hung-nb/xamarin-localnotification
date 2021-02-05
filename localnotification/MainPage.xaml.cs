using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DependencyService.Get<INotificationManager>().CreateNotification("SPTutorials", message.Text);
        }
    }
}
