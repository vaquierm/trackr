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
using trackr.core;
using trackr.ImageProcessing;

namespace trackr.ui
{
    /// <summary>
    /// Interaction logic for EmotionChart.xaml
    /// </summary>
    public partial class EmotionChart : UserControl
    {
        public bool UsePatientBasis { get; set; }

        public EmotionChart()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Anger"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Contempt"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Disgust"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Fear"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Happiness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Neutral"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
                    Title = "Sadness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { },
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

        public void UpdateSeriesPatientBasis(TherapyPatient patient)
        {
            if (patient?.GetSessions() == null)
            {
                return;
            }

            foreach (var series in Series)
            {
                series.Values.Clear();
            }

            foreach (var session in patient.GetSessions())
            {
                var emotionData = session.GetEmotionDataList();

                if (emotionData.Count == 0)
                    continue;

                double avg = 0;

                // Anger
                foreach (var data in emotionData)
                {
                    avg += data.Anger;
                }
                avg /= emotionData.Count;
                Series.ElementAt(0).Values.Add(avg);
                avg = 0;

                // Contempt
                foreach (var data in emotionData)
                {
                    avg += data.Contempt;
                }
                avg /= emotionData.Count;
                Series.ElementAt(1).Values.Add(avg);
                avg = 0;

                // Disgust
                foreach (var data in emotionData)
                {
                    avg += data.Disgust;
                }
                avg /= emotionData.Count;
                Series.ElementAt(2).Values.Add(avg);
                avg = 0;

                // Fear
                foreach (var data in emotionData)
                {
                    avg += data.Fear;
                }
                avg /= emotionData.Count;
                Series.ElementAt(3).Values.Add(avg);
                avg = 0;

                // Happiness
                foreach (var data in emotionData)
                {
                    avg += data.Happiness;
                }
                avg /= emotionData.Count;
                Series.ElementAt(4).Values.Add(avg);
                avg = 0;

                // Neutral
                foreach (var data in emotionData)
                {
                    avg += data.Neutral;
                }
                avg /= emotionData.Count;
                Series.ElementAt(5).Values.Add(avg);
                avg = 0;

                // Sadness
                foreach (var data in emotionData)
                {
                    avg += data.Sadness;
                }
                avg /= emotionData.Count;
                Series.ElementAt(6).Values.Add(avg);
                avg = 0;

                // Surprise
                foreach (var data in emotionData)
                {
                    avg += data.Surprise;
                }
                avg /= emotionData.Count;
                Series.ElementAt(7).Values.Add(avg);
            }
        }

        public void UpdateSeriesSessionBasis(TherapySession session)
        {
            if (session == null)
            {
                return;
            }

            foreach (var series in Series)
            {
                series.Values.Clear();
            }

            var allData = session.GetEmotionDataList();

            if (allData.Count == 0)
                return;

            int chunkSize = allData.Count < 50 ? 1 : allData.Count / 50;
            double sumAnger = 0;
            double sumContempt = 0;
            double sumDisgust = 0;
            double sumFear = 0;
            double sumHappiness = 0;
            double sumNeutral = 0;
            double sumSadness = 0;
            double sumSurprise = 0;
            int counter = 0;

            for (int i = 0; i < allData.Count; i ++)
            {
                if (counter >= chunkSize)
                {
                    Series.ElementAt(0).Values.Add(sumAnger / chunkSize);
                    Series.ElementAt(1).Values.Add(sumContempt / chunkSize);
                    Series.ElementAt(2).Values.Add(sumDisgust / chunkSize);
                    Series.ElementAt(3).Values.Add(sumFear / chunkSize);
                    Series.ElementAt(4).Values.Add(sumHappiness / chunkSize);
                    Series.ElementAt(5).Values.Add(sumNeutral / chunkSize);
                    Series.ElementAt(6).Values.Add(sumSadness / chunkSize);
                    Series.ElementAt(7).Values.Add(sumSurprise / chunkSize);

                    sumAnger = 0;
                    sumContempt = 0;
                    sumDisgust = 0;
                    sumFear = 0;
                    sumHappiness = 0;
                    sumNeutral = 0;
                    sumSadness = 0;
                    sumSurprise = 0;
                    counter = 0;
                }

                sumAnger += allData.ElementAt(i).Anger;
                sumContempt += allData.ElementAt(i).Contempt;
                sumDisgust += allData.ElementAt(i).Disgust;
                sumFear += allData.ElementAt(i).Fear;
                sumHappiness += allData.ElementAt(i).Happiness;
                sumNeutral += allData.ElementAt(i).Neutral;
                sumSadness += allData.ElementAt(i).Sadness;
                sumSurprise += allData.ElementAt(i).Surprise;

                counter++;
            }

            if (counter > 0)
            {
                Series.ElementAt(0).Values.Add(sumAnger / chunkSize);
                Series.ElementAt(1).Values.Add(sumContempt / chunkSize);
                Series.ElementAt(2).Values.Add(sumDisgust / chunkSize);
                Series.ElementAt(3).Values.Add(sumFear / chunkSize);
                Series.ElementAt(4).Values.Add(sumHappiness / chunkSize);
                Series.ElementAt(5).Values.Add(sumNeutral / chunkSize);
                Series.ElementAt(6).Values.Add(sumSadness / chunkSize);
                Series.ElementAt(7).Values.Add(sumSurprise / chunkSize);
            }
        }

        private void ResetSessionBasis()
        {
            foreach (var series in Series)
            {
                series.Values.Clear();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (UsePatientBasis)
            {
                UpdateSeriesPatientBasis(Workspace.Instance.ActivePatient);
            }
            else
            {
                if (Workspace.Instance.ActivePatient.GetSessions().Count == 0)
                    ResetSessionBasis();
                else
                    UpdateSeriesSessionBasis(Workspace.Instance.ActivePatient.GetSessions().ElementAt(Workspace.Instance.ActivePatient.GetSessions().Count - 1));
            }
        }


    }
}
