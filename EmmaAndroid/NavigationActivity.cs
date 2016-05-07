using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;
using EmmaAndroid.Data;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Navigation")]
    public class NavigationActivity : Activity, TextToSpeech.IOnInitListener
    {
        private readonly int VOICE;
        public TextToSpeech SpeechText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Navigation);
            SpeechText = new TextToSpeech(this, this);

            var buttonHome = FindViewById<ImageButton>(Resource.Id.imageButton7);
            buttonHome.Click += (object sender2, EventArgs e2) =>
            {
                StartActivity(typeof(MainActivity));
            };

            var buttonNavigateHome = FindViewById<ImageButton>(Resource.Id.imageButtonNavigateHome);
            buttonNavigateHome.Click += (object sender2, EventArgs e2) =>
            {
                Intent intent = new Intent();
                SpeechText.Speak("Starte Navigation nach Hause.", QueueMode.Flush, null);
                StartActivity(Controller.GetHomeNavigation());
            };

            //Speech IN
            var buttonSpeechIn = FindViewById<ImageButton>(Resource.Id.imageButtonVoice);
            buttonSpeechIn.Click += (object sender3, EventArgs e3) =>
            {
                if (Controller.GetTTSStatus())
                {
                    SpeechText.Speak("Wohin möchten Sie?", QueueMode.Flush, null);
                }
                GetVoice("Title");
            };

            var buttonList = FindViewById<ImageButton>(Resource.Id.imageButtonList);
            buttonList.Click += (object sender, EventArgs e) =>
            {
                var listActivity = new Intent(this, typeof(NavigationListActivity));
                StartActivity(listActivity);
            };

            var buttonLatest = FindViewById<ImageButton>(Resource.Id.imageButtonLatest);
            buttonLatest.Click += (object sender, EventArgs e) =>
            {
                var listActivity = new Intent(this, typeof(NavigationListActivity));
                listActivity.PutExtra("type", "latest");

                StartActivity(listActivity);
            };

            var buttonSearch = FindViewById<ImageButton>(Resource.Id.imageButtonSearch);

            buttonSearch.Click += (object sender, EventArgs e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                AutoCompleteTextView editText = new AutoCompleteTextView(this);

                var autoCompleteOptions = new List<string>();
                autoCompleteOptions.AddRange(AppData.NavigationData.GetAllAddresses());

                ArrayAdapter autoCompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, autoCompleteOptions);
                editText.Adapter = autoCompleteAdapter;

                alert.SetTitle("Zielsuche");
                alert.SetCancelable(true);
                alert.SetMessage("Ziel eingeben:");
                alert.SetView(editText);

                alert.SetPositiveButton("Los!", (senderAlert, args) => {
                    AppData.NavigationData.AddAddress(new AddressData("latest", editText.Text, AddressType.latest));
                    StartActivity(Controller.GetNavigation(editText.Text));
                });
              
                AlertDialog alertDialog = alert.Create();
                alertDialog.Show();
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

            var buttonFlashlight = FindViewById<ImageButton>(Resource.Id.imageButtonFlashlight);
            buttonFlashlight.Click += (object sender, EventArgs e) =>
            {
                Controller.CameraSwitch();
            };
        }

        private void GetVoice(string v)
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                var alert = new AlertDialog.Builder(this);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    return;
                });
                alert.Show();
            }

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Sprach-Zieleingabe");
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 3);
            voiceIntent.PutExtra(RecognizerIntent.ExtraResults, 3);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, VOICE);

        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            
            if (requestCode == VOICE)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0];

                        //Auswahl anzeigen
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        var layout = new LinearLayout(this);
                        layout.Orientation = Orientation.Vertical;
                        SpeechText.Speak("Meinten Sie:", QueueMode.Add, null);
                        foreach (string match in matches)
                        {
                            var button = new Button(this);
                            button.Text = match;
                            SpeechText.Speak(match, QueueMode.Add, null);
                            button.Click += (senderAlert, args) =>
                            {
                                AppData.NavigationData.AddAddress(new AddressData("latest", match, AddressType.latest));
                                StartActivity(Controller.GetNavigation(match));
                            };
                            button.SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
                            layout.AddView(button);
                        }
                        alert.SetView(layout);
                        alert.SetTitle("Meinten Sie: ");
                        alert.SetNegativeButton("Cancel", (senderAlert, args) => {});

                        AlertDialog alertDialog = alert.Create();
                        alertDialog.Show();
                    }
                }
                base.OnActivityResult(requestCode, resultVal, data);
            }
        }

        public void OnInit(OperationResult status)
        {

        }

    } 
}