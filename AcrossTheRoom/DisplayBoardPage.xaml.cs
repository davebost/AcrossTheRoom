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
        const int MAX_FONT_SIZE = 250;
        const int SPEED_MULTIPLIER = 1;
        const int MAX_TIME = 11;

        private int _speed = 5;

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

                    MainCanvas.Background = ColorHelper.GetBrushFromString(msg.BackgroundColor);
                    MarqueeTextBlock.Foreground = ColorHelper.GetBrushFromString(msg.ForegroundColor);

                    int fontScale = ((Message)this.DataContext).FontScale;

                    if (fontScale == 0) fontScale = 1;
                    MarqueeTextBlock.FontSize = (5 * 40);

                    // 40, 80, 120, 160, 200
                    _speed = ((Message)this.DataContext).Speed;
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
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(800, 480));
            MainCanvas.Clip = rectangleGeometry;
            double height = 480 - MarqueeTextBlock.ActualHeight;
            MarqueeTextBlock.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 800;
            doubleAnimation.To = -MarqueeTextBlock.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(MAX_TIME - (_speed * SPEED_MULTIPLIER)));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, MarqueeTextBlock);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();
        }

    }
}