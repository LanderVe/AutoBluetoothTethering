using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Bluetooth;
using Android.Support.V4.App;
using Android.Preferences;

namespace AutoBluetoothTethering
{
  [BroadcastReceiver(Enabled = true)]
  [IntentFilter(new[] { BluetoothDevice.ActionAclConnected, BluetoothDevice.ActionAclDisconnected })]
  public class SystemReceiver : BroadcastReceiver
  {
    private static Notifier notifier;

    public override void OnReceive(Context context, Intent intent)
    {
      //check device name
      ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
      string targetDeviceName = prefs.GetString(BluetoothHelper.DEVICE_NAME, "random WronG default value");
      BluetoothDevice connectedDevice = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

      if (targetDeviceName != connectedDevice.Name) return;

      var btHelper = new BluetoothHelper(context);

      if (BluetoothDevice.ActionAclConnected == intent.Action)
      {
        btHelper.SetBluetoothTetherState(isEnabled: true);
        //Nofity(context, "Received ActionAclConnected intent!");
      }
      else if (BluetoothDevice.ActionAclDisconnected == intent.Action)
      {
        btHelper.SetBluetoothTetherState(isEnabled: false);
        //Nofity(context, "Received ActionAclDisconnected intent!");
      }
    }

    private void Nofity(Context context, string text)
    {
      if (notifier == null)
      {
        notifier = new Notifier(context);
      }
      notifier.Notify(text);
    }
  }


}