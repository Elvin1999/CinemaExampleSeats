using Cinema.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml.Linq;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string Genre { get; set; }
        public string Time { get; set; }
        public string Rating { get; set; }
        public dynamic Data { get; set; }
        public dynamic SingleData { get; set; }


        HttpClient httpclient = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public int Index { get; set; } = 0;
        public string Genre2 { get; set; }
        public string Time2 { get; set; }
        public int Minute { get; set; }
        public int Hour { get; set; }
        public void GetMovie()
        {
            var name = SearchTxtBox.Text;
            HttpResponseMessage response = new HttpResponseMessage();
            response = httpclient.GetAsync($@"http://www.omdbapi.com/?apikey=4e6e4efc&s={name}&plot=full").Result;
            var str = response.Content.ReadAsStringAsync().Result;
            Data = JsonConvert.DeserializeObject(str);
            response = httpclient.GetAsync($@"http://www.omdbapi.com/?apikey=4e6e4efc&t={Data.Search[Index].Title}&plot=full").Result;

            str = response.Content.ReadAsStringAsync().Result;
            SingleData = JsonConvert.DeserializeObject(str);





            if (Index == 0)
            {
                image2.Source = SingleData.Poster;
                Genre = SingleData.Genre;
                Genre2 = " ";
                foreach (var item in Genre)
                {
                    if (item == ',')
                    {
                        Genre2 += " |";
                    }
                    else
                    {
                        Genre2 += item;
                    }

                }

                GenreTxtBlock.Text = Genre2;
                YearTxtBlock.Text = SingleData.Released;
                Time = SingleData.Runtime;
                Time2 = "";
                foreach (var item in Time)
                {
                    if (item != ' ' && item != 'm' && item != 'i' && item != 'n')
                    {
                        Time2 += item;
                    }

                }
                Minute = int.Parse(Time2);
                Hour = Minute / 60;
                Minute -= Hour * 60;

                timeTxtBlock.Text = Hour.ToString() + "h " + Minute.ToString() + "min";
                RatingTxtBlock.Text = SingleData.imdbRating + "/10";
                TitleTextBlock.Text = SingleData.Title;
                DTxtBlock.Text = FilmTypeCombobox.Text;

                DirectorTxtBlock.Text = SingleData.Director;
                WriterTxtBlock.Text = SingleData.Writer;




            }
            else
            {
                BackroundImage.Source = SingleData.Poster;
                --Index;
            }


        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {



            var border = sender as Border;
            if (border.Background == Brushes.LightGray)
                border.Background = Brushes.Pink;
            else
                border.Background = Brushes.LightGray;

            var txtblock = border.Child as TextBlock;
            //MessageBox.Show(txtblock.Text);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GetMovie();
            ++Index;
            GetMovie();
        }


        public  SolidColorBrush ChangeColor(string color)
        {
         
            // in WPF you can use a BrushConverter
            SolidColorBrush redBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            return redBrush;
        }
        private void CheckHoutBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchTxtBox.Text = SearchTxtBox.Text.ToLower();
            if (File.Exists(SearchTxtBox.Text+".json"))
            {
                var movie = FileHelper.FileHelper.JsonDeserializeMovie(SearchTxtBox.Text);
                List<Seat> seats = new List<Seat>();
                int index = 0;
                foreach (var item in panelA.Children)
                {
                    var border = item as Border;

                    border.Background = ChangeColor(movie.Seats[index].Color);

                  
                    index++;
                }
            }
            else
            {

                // CreatePDF()
                #region WriteJson
                List<Seat> seats = new List<Seat>();

                foreach (var item in panelA.Children)
                {
                    var border = item as Border;
                    var textBlock = border.Child as TextBlock;
                    Seat seat = new Seat(border.Background.ToString(), textBlock.Text, 10);
                    seats.Add(seat);
                }
                foreach (var item in panelB.Children)
                {
                    var border = item as Border;
                    var textBlock = border.Child as TextBlock;
                    Seat seat = new Seat(border.Background.ToString(), textBlock.Text, 10);
                    seats.Add(seat);
                }
                foreach (var item in panelC.Children)
                {
                    var border = item as Border;
                    var textBlock = border.Child as TextBlock;
                    Seat seat = new Seat(border.Background.ToString(), textBlock.Text, 10);
                    seats.Add(seat);
                }

                Movie movie = new Movie
                {
                    MovieName = SearchTxtBox.Text,
                    Seats = seats
                };
                FileHelper.FileHelper.JsonSerializationMovie(movie);
            }

            #endregion
        }
    }

}
