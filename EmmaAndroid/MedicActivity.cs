using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using EmmaAndroid.Data;
using Android.Speech;
using Android.Content.PM;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Medikamente")]
    public class MedicActivity : Activity, TextToSpeech.IOnInitListener
    {
        private readonly int VOICE = 1;
        public TextToSpeech SpeechText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Medic);

            var buttonList = FindViewById<ImageButton>(Resource.Id.imageButtonList);
            buttonList.Click += (object sender2, EventArgs e2) =>
            {
                Toast.MakeText(this, "Nicht implementiert!", ToastLength.Long).Show();
            };
            
            var buttonEmergencyMedic = FindViewById<ImageButton>(Resource.Id.imageButtonEmergencyMedic);
            buttonEmergencyMedic.Click += (object sender2, EventArgs e2) =>
            {
         
                Toast.MakeText(this, "Nicht implementiert!", ToastLength.Long).Show();
            };

            var buttonHome = FindViewById<ImageButton>(Resource.Id.imageButton7);
            buttonHome.Click += (object sender2, EventArgs e2) =>
            {
                StartActivity(typeof(MainActivity));
            };

            var buttonPicture = FindViewById<ImageButton>(Resource.Id.imageButtonPhotoSearchMedic);
            buttonPicture.Click += async (object sender, EventArgs e) =>
            {
                AppData.camera.Release();
                var scanner = new ZXing.Mobile.MobileBarcodeScanner(); 
                var result = await scanner.Scan();

                if (result != null)
                { 
                    string search = result.Text;

                    if (search.StartsWith("-"))
                        search = search.TrimStart('-');

                    StartMedicSearch(search);
                }
                
            };

            var buttonTextPicture = FindViewById<ImageButton>(Resource.Id.imageButtonPhotoTextSearchMedic);
            buttonTextPicture.Click += (object sender, EventArgs e) =>
            {
                Toast.MakeText(this, "Nicht implementiert!", ToastLength.Long).Show();
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


            var buttonSpeechIn = FindViewById<ImageButton>(Resource.Id.imageButtonVoice);
            buttonSpeechIn.Click += (object sender3, EventArgs e3) =>
            {
                GetVoice("Title");
            };

            var buttonSearch = FindViewById<ImageButton>(Resource.Id.imageButtonSeachMedic);
            buttonSearch.Click += (object sender, EventArgs e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                AutoCompleteTextView editText = new AutoCompleteTextView(this);

                var autoCompleteOptions = new List<string>();
                autoCompleteOptions.AddRange(AppData.NavigationData.GetAllAddresses());

                ArrayAdapter autoCompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, autoCompleteOptions);
                editText.Adapter = autoCompleteAdapter;

                alert.SetTitle("Medikamentensuche");
                alert.SetCancelable(true);
                alert.SetMessage("Medikament, Wirkstoff oder PNZ eingeben:");
                alert.SetView(editText);

                alert.SetPositiveButton("Los!", (senderAlert, args) => {
                    StartMedicSearch(editText.Text);
                });

                AlertDialog alertDialog = alert.Create();
                alertDialog.Show();
            };

            var buttonFlashlight = FindViewById<ImageButton>(Resource.Id.imageButtonFlashlight);
            buttonFlashlight.Click += (object sender, EventArgs e) =>
            {
                Controller.CameraSwitch();
            };

        }

        private void StartMedicSearch(string search)
        {
            var uri = Android.Net.Uri.Parse("https://www.medipreis.de/suchen?q=" + search);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void GetVoice(string v)
        {
            string rec = PackageManager.FeatureMicrophone;
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
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Medikamentensuche");
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 3);
            voiceIntent.PutExtra(RecognizerIntent.ExtraResults, 3);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, VOICE);

        }


        protected override async void OnActivityResult(int requestCode, Result resultVal, Intent data)
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
                        layout.Orientation = Android.Widget.Orientation.Vertical;

                        foreach (string match in matches)
                        {
                            var button = new Button(this);
                            button.Text = match;
                            button.SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
                            button.Click += (senderAlert, args) =>
                            {
                                StartMedicSearch(match);
                            };
                            layout.AddView(button);
                        }
                        alert.SetView(layout);
                        alert.SetTitle("Meinten Sie: ");
                        alert.SetNegativeButton("Cancel", (senderAlert, args) => { });

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