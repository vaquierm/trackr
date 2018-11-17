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

                float avg = 0;

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

        public void UpdateSeriesPatientBasisMonthly(TherapyPatient patient)
        {
            if (patient?.GetSessions() == null)
            {
                return;
            }

            foreach (var series in Series)
            {
                series.Values.Clear();
            }

            float anger = 0;
            float contempt = 0;
            float disgust = 0;
            float fear = 0;
            float happiness = 0;
            float neutral = 0;
            float sadness = 0;
            float surprise = 0;

            var n = 0;

            var month = 0;
            var year = 0;

            foreach (var session in patient.GetSessions())
            {
                // If first time setting month and year
                if (month == 0 || year == 0)
                {
                    month = session.StartDateTime.Month;
                    year = session.StartDateTime.Year;
                }
                // If in a different month or year
                else if (month != session.StartDateTime.Month && year != session.StartDateTime.Year)
                {
                    if (month == 12)
                    {
                        month = 1;
                        year++;
                    }
                    else
                    {
                        month++;
                    }

                    // Add average emotion for the month
                    Series.ElementAt(0).Values.Add(anger / n);
                    Series.ElementAt(1).Values.Add(contempt / n);
                    Series.ElementAt(2).Values.Add(disgust / n);
                    Series.ElementAt(3).Values.Add(fear / n);
                    Series.ElementAt(4).Values.Add(happiness / n);
                    Series.ElementAt(5).Values.Add(neutral / n);
                    Series.ElementAt(6).Values.Add(sadness / n);
                    Series.ElementAt(7).Values.Add(surprise /n);

                    // Reset counters
                    anger = 0;
                    contempt = 0;
                    disgust = 0;
                    fear = 0;
                    happiness = 0;
                    neutral = 0;
                    sadness = 0;
                    surprise = 0;
                    n = 0;
                }

                n++;

                var emotionData = session.GetEmotionDataList();

                float avg = 0;

                // Anger
                foreach (var data in emotionData)
                {
                    avg += data.Anger;
                }
                avg /= emotionData.Count;
                anger += avg;
                avg = 0;

                // Contempt
                foreach (var data in emotionData)
                {
                    avg += data.Contempt;
                }
                avg /= emotionData.Count;
                contempt += avg;
                avg = 0;

                // Disgust
                foreach (var data in emotionData)
                {
                    avg += data.Disgust;
                }
                avg /= emotionData.Count;
                disgust += avg;
                avg = 0;

                // Fear
                foreach (var data in emotionData)
                {
                    avg += data.Fear;
                }
                avg /= emotionData.Count;
                fear += avg;
                avg = 0;

                // Happiness
                foreach (var data in emotionData)
                {
                    avg += data.Happiness;
                }
                avg /= emotionData.Count;
                happiness += avg;
                avg = 0;

                // Neutral
                foreach (var data in emotionData)
                {
                    avg += data.Neutral;
                }
                avg /= emotionData.Count;
                neutral += avg;
                avg = 0;

                // Sadness
                foreach (var data in emotionData)
                {
                    avg += data.Sadness;
                }
                avg /= emotionData.Count;
                sadness += avg;
                avg = 0;

                // Surprise
                foreach (var data in emotionData)
                {
                    avg += data.Surprise;
                }
                avg /= emotionData.Count;
                surprise += avg;
            }
        }

        public void UpdateSeriesPatientBasisYearly(TherapyPatient patient)
        {
            if (patient?.GetSessions() == null)
            {
                return;
            }

            foreach (var series in Series)
            {
                series.Values.Clear();
            }

            float anger = 0;
            float contempt = 0;
            float disgust = 0;
            float fear = 0;
            float happiness = 0;
            float neutral = 0;
            float sadness = 0;
            float surprise = 0;

            int n = 0;
            
            int year = 0;

            foreach (var session in patient.GetSessions())
            {
                // If first time setting year
                if (year == 0)
                {
                    year = session.StartDateTime.Year;
                }
                // If in a different year
                else if (year != session.StartDateTime.Year)
                {
                    year++;

                    // Add average emotion for the year
                    Series.ElementAt(0).Values.Add(anger / n);
                    Series.ElementAt(1).Values.Add(contempt / n);
                    Series.ElementAt(2).Values.Add(disgust / n);
                    Series.ElementAt(3).Values.Add(fear / n);
                    Series.ElementAt(4).Values.Add(happiness / n);
                    Series.ElementAt(5).Values.Add(neutral / n);
                    Series.ElementAt(6).Values.Add(sadness / n);
                    Series.ElementAt(7).Values.Add(surprise / n);

                    // Reset counters
                    anger = 0;
                    contempt = 0;
                    disgust = 0;
                    fear = 0;
                    happiness = 0;
                    neutral = 0;
                    sadness = 0;
                    surprise = 0;
                    n = 0;
                }

                n++;

                var emotionData = session.GetEmotionDataList();

                float avg = 0;

                // Anger
                foreach (var data in emotionData)
                {
                    avg += data.Anger;
                }
                avg /= emotionData.Count;
                anger += avg;
                avg = 0;

                // Contempt
                foreach (var data in emotionData)
                {
                    avg += data.Contempt;
                }
                avg /= emotionData.Count;
                contempt += avg;
                avg = 0;

                // Disgust
                foreach (var data in emotionData)
                {
                    avg += data.Disgust;
                }
                avg /= emotionData.Count;
                disgust += avg;
                avg = 0;

                // Fear
                foreach (var data in emotionData)
                {
                    avg += data.Fear;
                }
                avg /= emotionData.Count;
                fear += avg;
                avg = 0;

                // Happiness
                foreach (var data in emotionData)
                {
                    avg += data.Happiness;
                }
                avg /= emotionData.Count;
                happiness += avg;
                avg = 0;

                // Neutral
                foreach (var data in emotionData)
                {
                    avg += data.Neutral;
                }
                avg /= emotionData.Count;
                neutral += avg;
                avg = 0;

                // Sadness
                foreach (var data in emotionData)
                {
                    avg += data.Sadness;
                }
                avg /= emotionData.Count;
                sadness += avg;
                avg = 0;

                // Surprise
                foreach (var data in emotionData)
                {
                    avg += data.Surprise;
                }
                avg /= emotionData.Count;
                surprise += avg;
            }
        }

        public void UpdateSeriesSessionBasis(TherapySession session)
        {
            if (session == null)
            {
                return;
            }

            List<EmotionData> emotionData = session.GetEmotionDataList();

            // Anger
            foreach (var data in emotionData)
            {
                Series.ElementAt(0).Values.Add(data.Anger);
            }

            // Contempt
            foreach (var data in emotionData)
            {
                Series.ElementAt(1).Values.Add(data.Contempt);
            }

            // Disgust
            foreach (var data in emotionData)
            {
                Series.ElementAt(2).Values.Add(data.Disgust);
            }

            // Fear
            foreach (var data in emotionData)
            {
                Series.ElementAt(3).Values.Add(data.Fear);
            }

            // Happiness
            foreach (var data in emotionData)
            {
                Series.ElementAt(4).Values.Add(data.Happiness);
            }

            // Neutral
            foreach (var data in emotionData)
            {
                Series.ElementAt(5).Values.Add(data.Neutral);
            }

            // Sadness
            foreach (var data in emotionData)
            {
                Series.ElementAt(6).Values.Add(data.Sadness);
            }

            // Surprise
            foreach (var data in emotionData)
            {
                Series.ElementAt(7).Values.Add(data.Surprise);
            }
        }
    }
}
