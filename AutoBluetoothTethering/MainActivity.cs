using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace AutoBluetoothTethering
{
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
  public class MainActivity : AppCompatActivity
  {
    
    private EditText deviceNameEditText;
    private Button saveButton;
    private ISharedPreferences prefs;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.activity_main);

      Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
      SetSupportActionBar(toolbar);

      deviceNameEditText = FindViewById<EditText>(Resource.Id.deviceNameEditText);
      saveButton = FindViewById<Button>(Resource.Id.saveButton);
      saveButton.Click += ButtonSave_Click;

      //set initial value 
      prefs = PreferenceManager.GetDefaultSharedPreferences(this);
      var targetDeviceName = prefs.GetString(BluetoothHelper.DEVICE_NAME, "");
      deviceNameEditText.Text = targetDeviceName;
    }

    private void ButtonSave_Click(object sender, EventArgs e)
    {
      using (var editor = prefs.Edit())
      {
        editor.PutString(BluetoothHelper.DEVICE_NAME, deviceNameEditText.Text);
        editor.Commit();
      }
    }

  }
}

