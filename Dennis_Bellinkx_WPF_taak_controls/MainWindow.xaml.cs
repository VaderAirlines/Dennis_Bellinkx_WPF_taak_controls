using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Dennis_Bellinkx_WPF_taak_controls {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        // fields
        Timer imageTimer = null;
        Timer marqueeTimer = null;
        Timer dateTimeTimer = null;

        List<string> availableImages = new List<string>() { "image_2.png", "image_1.jpg" };
        Queue<string> availableImageQueue = null;

        double marqueeContainerLeft = 0;
        double marqueeContainerRight = 0;
        bool goLeft = false;

        // .ctors
        public MainWindow() {
            InitializeComponent();
        }


        // initializers
        private void init(object sender, RoutedEventArgs e) {
            initFields();

            imageTimer.Start();
            marqueeTimer.Start();
            dateTimeTimer.Start();

            addLeftScrollerButtons(100);
            addRightScrollerItems(15);
        }

        private void addLeftScrollerButtons(int amount) {
            for (int i = 0; i < amount; i++) {
                leftScrollerGrid.Children.Add(new Button() { Content = "test", Height = 75, Margin = new System.Windows.Thickness(10) });
            }
        }

        private void addRightScrollerItems(int amount) {
            for (int i = 0; i < amount; i++) {
                Expander expTemp = new Expander();
                expTemp.Header = "Dit is een expander " + i;
                expTemp.HorizontalAlignment = HorizontalAlignment.Left;
                expTemp.Foreground = Brushes.LightGray;
                expTemp.Height = 50;
                expTemp.Margin = new Thickness(10);
                expTemp.IsExpanded = false;
                expTemp.Content = "Dit is de content van een expander.";

                rightScrollerGrid.Children.Add(expTemp);
            }

        }

        private void initFields() {
            availableImageQueue = new Queue<string>(availableImages);

            getMarqueeContainerWidth();

            imageTimer = new Timer(5000);
            imageTimer.Elapsed += changeCenterImageSource;

            marqueeTimer = new Timer(1);
            marqueeTimer.Elapsed += moveMarquee;

            dateTimeTimer = new Timer(1000);
            dateTimeTimer.Elapsed += displayDateTime;

            tbkCurrentUser.Text = Environment.UserName;
        }

        private void recalculateFields(object sender, SizeChangedEventArgs e) {
            getMarqueeContainerWidth();
            tbkMarquee.Margin = new Thickness(0, 0, 0, 0);
        }


        // timer handlers
        private void changeCenterImageSource(object sender, EventArgs e) {
            Dispatcher.InvokeAsync(() => {
                string nextImage = getNextImage();
                imgCenterImage.Source = new BitmapImage(new Uri("images/" + nextImage, UriKind.Relative));
            });
        }

        private void moveMarquee(object sender, EventArgs e) {
            Dispatcher.InvokeAsync(() => {
                if (tbkMarquee.Margin.Left > marqueeContainerRight) { goLeft = true; };
                if (tbkMarquee.Margin.Left < 0) { goLeft = false; }

                if (goLeft) {
                    tbkMarquee.Margin = new Thickness(tbkMarquee.Margin.Left - 1, 0, 0, 0);
                } else {
                    tbkMarquee.Margin = new Thickness(tbkMarquee.Margin.Left + 1, 0, 0, 0);
                }
            });
        }

        private void displayDateTime(object sender, EventArgs e) {
            Dispatcher.InvokeAsync(() => {
                string dateAndTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                tbkDateTime.Text = dateAndTime;
            });
        }


        // helpers
        private string getNextImage() {
            string topImage = availableImageQueue.Dequeue();
            availableImageQueue.Enqueue(topImage);

            return topImage;
        }

        private void getMarqueeContainerWidth() {
            marqueeContainerLeft = grdMarquee.TranslatePoint(new Point(0, 0), this).X;
            double grdWidth = grdMarquee.ActualWidth;
            double tbkWidth = tbkMarquee.ActualWidth;
            marqueeContainerRight = grdWidth - tbkWidth;
        }

    }
}
