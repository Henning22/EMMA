using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using EmmaAndroid.Data;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Navigation")]
    public class NavigationListActivity : Activity, TextToSpeech.IOnInitListener
    {
        public TextToSpeech SpeechText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ListLayout);
            SpeechText = new TextToSpeech(this, this);

            var listLayout = GetListLayout();

            var buttonHome = FindViewById<ImageButton>(Resource.Id.imageButton7);
            buttonHome.Click += (object sender2, EventArgs e2) =>
            {
                StartActivity(typeof(MainActivity));
            };

            var testSpeechButton = FindViewById<ImageButton>(Resource.Id.imageButton10);
            testSpeechButton.SetImageResource(Resource.Drawable.audio_out_deactivated);
            testSpeechButton.Click += delegate
            {
                Controller.SwitchTTSStatus();
                if (Controller.GetTTSStatus())
                {
                    testSpeechButton.SetImageResource(Resource.Drawable.audio_out_deactivated);
                    SpeechText.Speak("Automatische Sprachausgabe aktiviert!", QueueMode.Flush, null);
                }
                else
                {
                    testSpeechButton.SetImageResource(Resource.Drawable.audio_out);
                }
            };

            var buttonAdd = FindViewById<ImageButton>(Resource.Id.imageButtonAdd);
            if (Intent.GetStringExtra("type") != "latest")
            { 
                buttonAdd.Click += (object sender, EventArgs e) =>
                {
                    Intent intent = new Intent(this, typeof(NavigationAddActivity));
                    intent.PutExtra("type", Intent.GetStringExtra("type"));
                    StartActivity(intent);
                };
            }
            else
                buttonAdd.Visibility = ViewStates.Invisible;

            var buttonFlashlight = FindViewById<ImageButton>(Resource.Id.imageButtonFlashlight);
            buttonFlashlight.Click += (object sender, EventArgs e) =>
            {
                Controller.CameraSwitch();
            };
        }

        private LinearLayout GetListLayout()
        {
            var listLayout = (LinearLayout)FindViewById(Resource.Id.linearLayout6);

            List<AddressData> listOfAddressData = new List<AddressData>();

            if (Intent.GetStringExtra("type") == "latest")
            { 
                listOfAddressData.AddRange(AppData.NavigationData.GetAddressesOfType(AddressType.latest));

                foreach (AddressData data in listOfAddressData)
                {
                    listLayout.AddView(TextLatest(data));
                    listLayout.AddView(Divider());
                }
            }
            else
            { 
                listOfAddressData.AddRange(AppData.NavigationData.GetAddressesOfType(AddressType.standard));

                foreach (AddressData data in listOfAddressData)
                {
                    listLayout.AddView(Text(data));
                    listLayout.AddView(Divider());
                }
            }
            
            return listLayout;
        }

        private View Text(AddressData data)
        {
            var tw = new TextView(this);
            tw.SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
            tw.Clickable = true;
            tw.Focusable = true;
            tw.SetTextColor(Android.Graphics.Color.Black);
            tw.Text = data.Title;
            tw.Click += (object sender, EventArgs o) =>
            {
                StartActivity(Controller.GetNavigation(data.Address));
            };

            return tw;
        }

        private View TextLatest(AddressData data)
        {
            var tw = new TextView(this);
            tw.SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
            tw.Clickable = true;
            tw.Focusable = true;
            tw.SetTextColor(Android.Graphics.Color.Black);
            tw.Text = data.Address;
            tw.Click += (object sender, EventArgs o) =>
            {
                StartActivity(Controller.GetNavigation(data.Address));
            };

            return tw;
        }

        private View Divider()
        {
            var div = new View(this);
            div.SetMinimumWidth(100);
            div.SetBackgroundColor(Android.Graphics.Color.DarkGray);
            div.SetMinimumHeight(2);

            return div;
        }
        public void OnInit(OperationResult status)
        {

        }
    }
}