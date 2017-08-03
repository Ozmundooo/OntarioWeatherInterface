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
using System.Net;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace WeatherInterface
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

        private void click(object sender, RoutedEventArgs e)
        {
            Button getWeather = (Button)sender;
            
            var city = cityInput.Text;
            JObject yahooJSON = getJSON(city);
            int temp = tempGet(yahooJSON);
            String condition = condGet(yahooJSON);
            output(temp, condition);
            outputImage(yahooJSON);
        }

        private void outputImage(JObject yahooJSON)
        {
            //ImageSource imageSource = new BitmapImage(new Uri("Properties/Resources/0.png"));
            //string uriSource = @"WeatherInterface;Resources/0.png";
            //image.Source = new ImageSourceConverter().ConvertFromString(uriSource) as ImageSource;
            //image.Source = this.FindResource("zero.png") as ImageSource;
            JObject condition = (JObject)yahooJSON["query"]["results"]["channel"]["item"]["condition"];
            int code = Convert.ToInt32(condition["code"]);
            image.Source = new BitmapImage(new Uri(@"F:\Xamarin Projects\WeatherInterface\WeatherInterface\Resources\"+ code +".png"));
        }

        private string condGet(JObject yahooJSON)
        {
            JObject condition = (JObject)yahooJSON["query"]["results"]["channel"]["item"]["condition"];
            String cond = Convert.ToString(condition["text"]);
            return cond;
        }

        private void output(int temp, String cond)
        {
            tempTV.Content = temp + "°C" ;
            condTV.Content = cond;
        }

        private int tempGet(JObject yahooJSON)
        {
            JObject condition = (JObject)yahooJSON["query"]["results"]["channel"]["item"]["condition"];
            int temp = Convert.ToInt32(condition["temp"]);
            temp = (temp - 32) * 5 / 9;
            return temp;
        }

        private static JObject getJSON(string city)
        {
            var json = new WebClient().DownloadString("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" +
                city
                + "%2C%20on%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            JObject data = JObject.Parse(json);
            return data;
        }
    }
}
