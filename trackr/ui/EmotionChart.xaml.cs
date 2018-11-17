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
        public EmotionChart()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Anger"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Contempt"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Disgust"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Fear"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Happiness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Neutral"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
                    Title = "Sadness"
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {},
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
            foreach (LineSeries series in Series)
            {
                series.Values.Clear();
            }

            foreach (TherapySession session in patient.GetSessions())
            {
                List<EmotionData> emotionData = session.GetEmotionDataList();

                float avg = 0;

                // Anger
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Anger;
                }
                avg /= emotionData.Count;
                Series.ElementAt(0).Values.Add(avg);
                avg = 0;

                // Contempt
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Contempt;
                }
                avg /= emotionData.Count;
                Series.ElementAt(1).Values.Add(avg);
                avg = 0;

                // Disgust
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Disgust;
                }
                avg /= emotionData.Count;
                Series.ElementAt(2).Values.Add(avg);
                avg = 0;

                // Fear
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Fear;
                }
                avg /= emotionData.Count;
                Series.ElementAt(3).Values.Add(avg);
                avg = 0;

                // Happiness
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Happiness;
                }
                avg /= emotionData.Count;
                Series.ElementAt(4).Values.Add(avg);
                avg = 0;

                // Neutral
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Neutral;
                }
                avg /= emotionData.Count;
                Series.ElementAt(5).Values.Add(avg);
                avg = 0;

                // Sadness
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Sadness;
                }
                avg /= emotionData.Count;
                Series.ElementAt(6).Values.Add(avg);
                avg = 0;

                // Surprise
                foreach (EmotionData data in emotionData)
                {
                    avg += data.Surprise;
                }
                avg /= emotionData.Count;
                Series.ElementAt(7).Values.Add(avg);
            }
        }

        public void UpdateSeriesSessionBasis(TherapySession session)
        {
            List<EmotionData> emotionData = session.GetEmotionDataList();

            // Anger
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(0).Values.Add(data.Anger);
            }

            // Contempt
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(1).Values.Add(data.Contempt);
            }

            // Disgust
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(2).Values.Add(data.Disgust);
            }

            // Fear
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(3).Values.Add(data.Fear);
            }

            // Happiness
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(4).Values.Add(data.Happiness);
            }

            // Neutral
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(5).Values.Add(data.Neutral);
            }

            // Sadness
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(6).Values.Add(data.Sadness);
            }

            // Surprise
            foreach (EmotionData data in emotionData)
            {
                Series.ElementAt(7).Values.Add(data.Surprise);
            }
        }
    }
}
