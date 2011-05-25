using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using AcrossTheRoom.Models;

namespace AcrossTheRoom
{
    public partial class DisplayBoardPage : PhoneApplicationPage
    {
        const int FONT_SCALE_MULTIPLYER = 25;
        const int SPEED_MULTIPLIER = 1;
        const int MAX_TIME = 15;
        const int PIXELS_PER_SECOND = 200;

        private double _speed = 2.5;

        public DisplayBoardPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(DisplayBoardPage_Loaded);

        }

        void DisplayBoardPage_Loaded(object sender, RoutedEventArgs e)
        {
            Play();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id = "";
            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                Message msg = MessageData.Instance.Messages.FirstOrDefault(m => m.id == id);
                if (msg != null)
                {
                    this.DataContext = msg;

                    MarqueeTextBlock.Text = msg.Text;

                    MainCanvas.Background = ColorHelper.GetBrushFromString(msg.BackgroundColor);
                    MarqueeTextBlock.Foreground = ColorHelper.GetBrushFromString(msg.ForegroundColor);

                    int fontScale = ((Message)this.DataContext).FontScale;
                    if (fontScale == 0) fontScale = 1;


                    MarqueeTextBlock.FontSize = 150 + (fontScale * FONT_SCALE_MULTIPLYER);
                    System.Diagnostics.Debug.WriteLine("FontScale: " + fontScale.ToString());
                    System.Diagnostics.Debug.WriteLine("FontSize: " + MarqueeTextBlock.FontSize.ToString());

                    _speed = ((Message)this.DataContext).Speed;
                    if (_speed == 0) _speed = 1;
                    System.Diagnostics.Debug.WriteLine("Speed: " + _speed.ToString());

                }
                else
                {
                    MarqueeTextBlock.Text = "Message not found.";
                }
            }
        }

        public void Play()
        {
            RightToLeftMarquee();
        }

        private void RightToLeftMarquee()
        {
            //<DoubleAnimation Duration="5" To="-942" 
            //  Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.TranslateX)" 
            //  Storyboard.TargetName="textBlock" d:IsOptimized="True"/>

            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(2000, 480));
            MainCanvas.Clip = rectangleGeometry;

            MarqueeTextBlock.Width = MarqueeTextBlock.ActualWidth;
            double height = 480 - MarqueeTextBlock.ActualHeight;
            MarqueeTextBlock.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 800;
            doubleAnimation.To = -MarqueeTextBlock.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Calculate duration
            double marqueeWidth = MarqueeTextBlock.ActualWidth;
            double duration = marqueeWidth / (_speed * PIXELS_PER_SECOND);

            System.Diagnostics.Debug.WriteLine("Width: " + marqueeWidth.ToString() + "         Duration: " + duration.ToString());
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration)); 
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, MarqueeTextBlock);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();

        }

    }
}