using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Speech;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "EMMA", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, TextToSpeech.IOnInitListener
    {
        private readonly int VOICE;
        public TextToSpeech SpeechText;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            SpeechText = new TextToSpeech(this, this);

            


            //Buttons
            var buttonAlgorithm = FindViewById<ImageButton>(Resource.Id.imageButtonAlgorithm);
            buttonAlgorithm.Click += (object sender2, EventArgs e2) =>
            {
                Toast.MakeText(this, "Nicht implementiert!", ToastLength.Long).Show();
            };

            var testSpeechButton = FindViewById<ImageButton>(Resource.Id.imageButton10);
            testSpeechButton.SetImageResource(Resource.Drawable.audio_out_deactivated);
            testSpeechButton.Click += delegate
            {
                Controller.SwitchTTSStatus();
                if(Controller.GetTTSStatus())
                {
                    testSpeechButton.SetImageResource(Resource.Drawable.audio_out_deactivated);
                    SpeechText.Speak("Automatische Sprachausgabe aktiviert!", QueueMode.Flush, null);
                }
                else
                { 
                   testSpeechButton.SetImageResource(Resource.Drawable.audio_out);
                }   
             };

  


            var buttonMedic = FindViewById<ImageButton>(Resource.Id.imageButtonMedic);
            buttonMedic.Click += (object sender, EventArgs e) =>
            {
                StartActivity(new Intent(this, typeof(MedicActivity)));
            };

            var buttonNav = FindViewById<ImageButton>(Resource.Id.imageButtonNavigation);
            buttonNav.Click += (object sender, EventArgs e) =>
            {
                StartActivity(new Intent(this, typeof(NavigationActivity)));
            };

            var buttonNote = FindViewById<ImageButton>(Resource.Id.imageButtonNote);
            buttonNote.Click += (object sender, EventArgs e) =>
            {
                StartActivity(new Intent(this, typeof(NoteActivity)));
            };

            var buttonSpeechIn = FindViewById<ImageButton>(Resource.Id.imageButtonVoice);
            buttonSpeechIn.Click += (object sender3, EventArgs e3) =>
            {
                Intent intent = new Intent();
                if (Controller.GetTTSStatus())
                {
                    SpeechText.Speak("Wählen Sie einen Menüpunkt!", QueueMode.Flush, null);
                }

                GetVoice("Title");

            };

            var buttonFlashlight = FindViewById<ImageButton>(Resource.Id.imageButtonFlashlight);
            buttonFlashlight.Click += (object sender, EventArgs e) =>
            {
                Controller.CameraSwitch();
            };
        }


        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            //TODO: Error Handling
            //TODO: How to get a smaller domain?
            if (requestCode == VOICE)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0].ToLower();
                        switch(textInput)
                        {
                            case "navigation":
                            case "navi":
                                StartActivity(new Intent(this, typeof(NavigationActivity)));
                                break;
                            case "medizin":
                            case "medic":
                            case "medikamente":
                                StartActivity(new Intent(this, typeof(MedicActivity)));
                                break;
                            case "note":
                            case "notiz":
                                StartActivity(new Intent(this, typeof(NoteActivity)));
                                break;
                            default:
                                Toast.MakeText(this, "Bitte wiederholen Sie die Spracheingabe.", ToastLength.Short).Show();
                                break;

                        }
                        if(textInput == "navigation")
                        {
                            
                        }
                    }
                }
                base.OnActivityResult(requestCode, resultVal, data);
            }
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
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Wählen Sie einen Menüpunkt");
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, VOICE);

        }

        public void OnInit(OperationResult status)
        {

        }
    }
}

