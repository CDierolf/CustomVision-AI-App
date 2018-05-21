using LandmarkAIApp.classes;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace LandmarkAIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png; *.jpg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
                imgSelectedImage.Source = new BitmapImage(new Uri(fileName));

                MakePredictionAsync(fileName);
            }
        }

        private async void MakePredictionAsync(string fileName)
        {
            string url = ""; // CustomVision API URL
            string predictionKey = ""; // CustomVision API Prediction Key
            string contentType = ""; // CustomVision API Content Type
            var file = File.ReadAllBytes(fileName);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                using (var content = new ByteArrayContent(file))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    var response = await client.PostAsync(url, content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    List<Prediction> predictions = (JsonConvert.DeserializeObject<CustomVision>(responseString)).Predictions;
                    lstViewData.ItemsSource = predictions;
                }

            }
        }
    }
}
