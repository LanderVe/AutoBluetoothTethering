using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AutoBluetoothTethering
{
  class Notifier
  {
    private const string CHANNEL_ID = "AutoBlueToothChannelId";

    private readonly Context context;
    static int notifycounter = 0;

    public Notifier(Context context)
    {
      this.context = context;

      NotificationChannel channel = new NotificationChannel(CHANNEL_ID, CHANNEL_ID, NotificationImportance.Default);
      NotificationManager.FromContext(context).CreateNotificationChannel(channel);
    }

    public void Notify(string text)
    {
      ++notifycounter;

      var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                    .SetSmallIcon(Android.Resource.Drawable.IcPopupDiskFull)
                    .SetNumber(1)
                    .SetContentTitle(text)
                    .SetContentText($"{text} ({notifycounter})  {DateTime.Now}");

      var notificationManager = NotificationManagerCompat.From(context);
      notificationManager.Notify(notifycounter, builder.Build());
    }

  }
}