using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game.Renderer
{
    public class RectCheckbox
    {
        public Rectangle Border { get; private set; }
        public bool Checked { get; set; }

        private SolidColorBrush checkedColor;
        private Panel panel;

        public RectCheckbox(Panel collection, int width, int height, SolidColorBrush color)
        {
            panel = collection;
            checkedColor = color;

            Border = new Rectangle();
            Border.Width = width;
            Border.Height = height;
            Border.Fill = new SolidColorBrush(Colors.Gray);
            Border.Stroke = new SolidColorBrush(Colors.White);

            Border.HorizontalAlignment = HorizontalAlignment.Center;
            Border.VerticalAlignment = VerticalAlignment.Center;

            Border.MouseDown += (s, e) => 
            {
                if (Checked)
                {
                    Border.Fill = new SolidColorBrush(Colors.Gray);
                    Checked = false;
                }
                else
                {
                    Border.Fill = checkedColor;
                    Checked = true;
                }
            };

            collection.Children.Add(Border);
        }

        public void SetPosition(double x, double y)
        {
            Canvas.SetLeft(Border, x);
            Canvas.SetTop(Border, y);
        }
    }

    public class RectButton
    {
        public Rectangle Border { get; private set; }
        public Label Label { get; private set; }
        public RectButton(Panel collection, int width, int height, SolidColorBrush color)
        {
            Border = new Rectangle();
            Border.Width = width;
            Border.Height = height;
            Border.Fill = color;

            Label = new Label();
            Label.FontFamily = new FontFamily("Century Gothic");

            collection.Children.Add(Border);
            collection.Children.Add(Label);
        }

        public void SetPosition(int x, int y)
        {
            Canvas.SetTop(Border, y);
            Canvas.SetLeft(Border, x);
            Canvas.SetTop(Label, y + Border.Height / 2 - Label.DesiredSize.Height / 2);
            Canvas.SetLeft(Label, x + Border.Width / 2 - Label.DesiredSize.Width / 2);
        }

        public void SetText(string text)
        {
            Label.Content = text;
            Label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }

        public void SetTextSize(int size)
        {
            Label.FontSize = size;
            Label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }

        public void AddOnClickEvent(MouseButtonEventHandler click)
        {
            Border.MouseDown += click;
            Label.MouseDown += click;
        }

        public void AddOnMouseEnterEvent(MouseEventHandler e)
        {
            Border.MouseEnter += e;
            Label.MouseEnter += e;
        }

        public void AddOnMouseLeaveEvent(MouseEventHandler e)
        {
            Border.MouseLeave += e;
        }
    }

}
