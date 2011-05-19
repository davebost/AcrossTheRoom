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

namespace AcrossTheRoom
{
    public partial class MarqueeControl : UserControl
    {
        const int SPEED_MULTIPLIER = 1;
        const int MAX_TIME = 11;

        public MarqueeControl()
        {
            InitializeComponent();
            MainCanvas.Height = this.Height;
            MainCanvas.Width = this.Width;
            this.Loaded += new RoutedEventHandler(MarqueeControl_Loaded);
        }

        private int _fontScale = 5;

        public int FontScale
        {
            get { return _fontScale; }
            set { _fontScale = value; }
        }

        public static readonly DependencyProperty ContentProperty =
           DependencyProperty.Register("Content", typeof(string), typeof(MarqueeControl), new PropertyMetadata("",
                                  new PropertyChangedCallback(
                                    OnContentChanged)));

        private int _speed = 5;
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        static void OnContentChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((MarqueeControl)sender).MarqueeTextBlock.Text = Convert.ToString(args.NewValue);
        }

        void MarqueeControl_Loaded(object sender, RoutedEventArgs e)
        {
            MarqueeTextBlock.FontSize = this.FontSize;

            Play();
        }

        public void Play()
        {
            RightToLeftMarquee();
        }       

        private void RightToLeftMarquee()
        {
            //<DoubleAnimation Duration="5" To="-942" Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.TranslateX)" Storyboard.TargetName="textBlock" d:IsOptimized="True"/>

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
