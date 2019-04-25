using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;

namespace AutoBluetoothTethering
{
  class BluetoothHelper
  {
    public const string DEVICE_NAME = "device_name";

    private readonly Context context;

    public BluetoothHelper(Context context)
    {
      this.context = context;
    }

    public void SetBluetoothTetherState(bool isEnabled)
    {
      try
      {
        var bluetoothPanClass = Class.ForName("android.bluetooth.BluetoothPan");
        var btPanCtor = bluetoothPanClass.GetDeclaredConstructor(Class.FromType(typeof(Context)), Class.FromType(typeof(IBluetoothProfileServiceListener)));
        btPanCtor.Accessible = true;

        var setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethods().Single(m => m.Name == "setBluetoothTethering");
        setBluetoothTetheringMethod.Accessible = true;

        var btServiceListener = new BTPanServiceListener();
        var btSrvInstance = btPanCtor.NewInstance(context, btServiceListener);

        btServiceListener.ServiceConnected += (s, e) =>
          setBluetoothTetheringMethod.Invoke(btSrvInstance, isEnabled);


        //var methods = classBluetoothPan.GetDeclaredMethods()
        //var method = methods[17];
        //var parTypes = method.GetParameterTypes();
        //Method setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethod("setBluetoothTethering", parTypes);

        //Method setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethod("setBluetoothTethering", new Class[] { Class.FromType(typeof(Java.Lang.Boolean)) });
        //Method setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethod("setBluetoothTethering", new Class[] { Class.ForName("boolean") });
        //Method setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethod("setBluetoothTethering", new Class[] { Class.FromType(typeof(bool)) });
        //Method setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethod("setBluetoothTethering", new Class[] { Class.FromType(typeof(Java.Lang.Boolean)) });
        //Method setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethod("setBluetoothTethering", new Class[] { Class.FromType(typeof(Java.Lang.Z)) });

        //var setBluetoothTetheringMethod = bluetoothPanClass.GetDeclaredMethods().Single(m => m.Name == "setBluetoothTethering");
        //setBluetoothTetheringMethod.Accessible = true;
        //setBluetoothTetheringMethod.Invoke(BTSrvInstance, true);
      }
      catch (ClassNotFoundException e)
      {
        e.PrintStackTrace();
      }
      catch (Java.Lang.Exception e)
      {
        e.PrintStackTrace();
      }
    }

    class BTPanServiceListener : Java.Lang.Object, IBluetoothProfileServiceListener
    {
      public event EventHandler ServiceConnected;

      public void OnServiceConnected([GeneratedEnum] ProfileType profile, IBluetoothProfile proxy) 
        => ServiceConnected?.Invoke(this, EventArgs.Empty);

      public void OnServiceDisconnected([GeneratedEnum] ProfileType profile) { }
    }
  }
}