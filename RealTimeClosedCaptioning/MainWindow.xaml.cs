using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealTimeClosedCaptioning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string subscriptionKey = "<< your key >>";
        string region = "<< your region >>";
        
        SpeechFactory factory = null;
        SpeechRecognizer recognizer = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            factory = SpeechFactory.FromSubscription(subscriptionKey, region);
            recognizer = factory.CreateSpeechRecognizer();

            recognizer.IntermediateResultReceived += Recognizer_IntermediateResultReceived;
            recognizer.FinalResultReceived += Recognizer_FinalResultReceived;

            await recognizer.StartContinuousRecognitionAsync();
            LastMessageBlock.Text = "Ready!";
        }

        private void Recognizer_FinalResultReceived(object sender, SpeechRecognitionResultEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(e.Result.RecognizedText))
                {
                    LastMessageBlock.Text = e.Result.RecognizedText.ToUpper();
                    CurrentMessageBlock.Text = string.Empty;
                }
            });
        }

        private void Recognizer_IntermediateResultReceived(object sender, SpeechRecognitionResultEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                CurrentMessageBlock.Text = e.Result.RecognizedText.ToUpper();
            });
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (recognizer != null)
                await recognizer.StopContinuousRecognitionAsync();

        }
    }
}
