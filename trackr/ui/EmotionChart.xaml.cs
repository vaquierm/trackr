using LiveCharts;
using LiveCharts.Wpf;
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

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for EmotionChart.xaml
    /// </summary>
    public partial class EmotionChart : UserControl
    {
        public EmotionChart()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {20, 30, 35, 45, 65, 85},
                    Title = "Anger"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {10, 12, 18, 20, 38, 40},
                    Title = "Contempt"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {5, 8, 12, 15, 22, 25},
                    Title = "Disgust"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {10, 12, 18, 20, 38, 40},
                    Title = "Fear"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Happiness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Neutral"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Sadness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Surprise"
                }
            };

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListBox, (DependencyObject)e.OriginalSource) as ListBoxItem;
            if (item == null) return;

            var series = (LineSeries)item.Content;
            series.Visibility = series.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        public void UpdateSeries(string s)
        {
            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Anger"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Contempt"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Disgust"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Fear"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Happiness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Neutral"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Sadness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {13, 12, 25, 20, 50, 10},
                    Title = "Surprise"
                }
            };
        }
    }
}
