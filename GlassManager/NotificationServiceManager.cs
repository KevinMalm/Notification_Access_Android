using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Service.Notification;

namespace GlassManager
{
    [Service(Label = "NotificationListenServiceSystem", Permission = "android.permission.BIND_NOTIFICATION_LISTENER_SERVICE")]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    public class NotificationService: NotificationListenerService
    {
        public static Notification_logger DATABASE = null;
        public static NotificationService service_OBJ = null;
        public NotificationService()
        {
            if (service_OBJ == null)
                service_OBJ = new NotificationService();
            if (DATABASE == null)
                DATABASE = new Notification_logger();
            return;  
        }

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {
            base.OnNotificationPosted(sbn);
            string category = sbn.Notification.Category;
            int priority = sbn.Notification.Priority;

            DATABASE.save_notification(sbn);
        }

        public override void OnNotificationRemoved(StatusBarNotification sbn)
        {
            base.OnNotificationRemoved(sbn);
        }

    }
}
