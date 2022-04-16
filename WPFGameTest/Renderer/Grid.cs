﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFGameTest.Helpers;
using WPFGameTest.Logic;

namespace WPFGameTest.Renderer
{
    public class LevelGrid
    {
        public int[,] Map { get; set; }
        public Vector2 CellSize { get; private set; }

        public LevelGrid(Canvas canvas, int rows, int cols, int width, int height)
        {
            Map = new int[rows, cols];
            CellSize = new Vector2(width, height);

            for (int i = 0; i < rows; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.White);
                line.StrokeThickness = 1;

                line.X1 = 0; line.X2 = rows * width;
                line.Y1 = i * height; line.Y2 = i * height;
                canvas.Children.Add(line);
            }

            for (int i = 0; i < cols; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.White);
                line.StrokeThickness = 1;

                line.X1 = i * width; line.X2 = i * width;
                line.Y1 = 0; line.Y2 = cols * height;
                canvas.Children.Add(line);
            }
        }
    }
}