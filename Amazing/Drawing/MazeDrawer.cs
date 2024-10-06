// File: Amazing/Drawing/MazeDrawer.cs
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using Amazing.Models;
using System.Collections.Generic;

namespace Amazing.Drawing
{
    public class MazeDrawer : IMazeDrawer
    {
        protected readonly Canvas _canvas;
        protected readonly double _cellSize;

        // Keep track of label TextBlocks to manage them during redraws
        private TextBlock _startLabel;
        private TextBlock _endLabel;

        public MazeDrawer(Canvas canvas, double cellSize)
        {
            _canvas = canvas;
            _cellSize = cellSize;
        }

        protected void DrawLine(double x1, double y1, double x2, double y2, Brush color)
        {
            var line = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = color,
                StrokeThickness = 1
            };
            _canvas.Children.Add(line);
        }

        private void DrawLabel(string text, int row, int col, Brush foreground, double fontSize = 16)
        {
            // Create a TextBlock for the label
            var label = new TextBlock
            {
                Text = text,
                Foreground = foreground,
                FontWeight = FontWeights.Bold,
                FontSize = fontSize
            };

            // Measure the size of the text
            label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var size = label.DesiredSize;

            // Calculate the position to center the label within the cell
            double x = col * _cellSize + (_cellSize - size.Width) / 2;
            double y = row * _cellSize + (_cellSize - size.Height) / 2;

            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, y);

            // Add the label to the canvas
            _canvas.Children.Add(label);
        }

        public void DrawMaze(Maze maze)
        {
            _canvas.Children.Clear();

            foreach (var cell in maze.Cells)
            {
                double x = cell.Col * _cellSize;
                double y = cell.Row * _cellSize;

                // Draw walls
                if (cell.Walls[0]) // Top
                    DrawLine(x, y, x + _cellSize, y, Brushes.Black);
                if (cell.Walls[1]) // Right
                    DrawLine(x + _cellSize, y, x + _cellSize, y + _cellSize, Brushes.Black);
                if (cell.Walls[2]) // Bottom
                    DrawLine(x, y + _cellSize, x + _cellSize, y + _cellSize, Brushes.Black);
                if (cell.Walls[3]) // Left
                    DrawLine(x, y, x, y + _cellSize, Brushes.Black);
            }

            // Add "S" and "E" labels
            DrawStartAndEndLabels(maze);
        }

        private void DrawStartAndEndLabels(Maze maze)
        {
            // Remove existing labels if they exist
            if (_startLabel != null)
            {
                _canvas.Children.Remove(_startLabel);
                _startLabel = null;
            }
            if (_endLabel != null)
            {
                _canvas.Children.Remove(_endLabel);
                _endLabel = null;
            }

            // Draw "S" at the start cell (0,0)
            _startLabel = new TextBlock
            {
                Text = "S",
                Foreground = Brushes.Green,
                FontWeight = FontWeights.Bold,
                FontSize = 16
            };
            _startLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var startSize = _startLabel.DesiredSize;
            double startX = 0 * _cellSize + (_cellSize - startSize.Width) / 2;
            double startY = 0 * _cellSize + (_cellSize - startSize.Height) / 2;
            Canvas.SetLeft(_startLabel, startX);
            Canvas.SetTop(_startLabel, startY);
            _canvas.Children.Add(_startLabel);

            // Draw "E" at the end cell (Rows-1, Cols-1)
            int endRow = maze.Rows - 1;
            int endCol = maze.Cols - 1;

            _endLabel = new TextBlock
            {
                Text = "E",
                Foreground = Brushes.Red,
                FontWeight = FontWeights.Bold,
                FontSize = 16
            };
            _endLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var endSize = _endLabel.DesiredSize;
            double endX = endCol * _cellSize + (_cellSize - endSize.Width) / 2;
            double endY = endRow * _cellSize + (_cellSize - endSize.Height) / 2;
            Canvas.SetLeft(_endLabel, endX);
            Canvas.SetTop(_endLabel, endY);
            _canvas.Children.Add(_endLabel);
        }

        public void DrawSolutionPath(List<Cell> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                var current = path[i];
                var next = path[i + 1];

                if (IsAdjacent(current, next))
                {
                    double x1 = current.Col * _cellSize + _cellSize / 2;
                    double y1 = current.Row * _cellSize + _cellSize / 2;
                    double x2 = next.Col * _cellSize + _cellSize / 2;
                    double y2 = next.Row * _cellSize + _cellSize / 2;

                    DrawLine(x1, y1, x2, y2, Brushes.Red);
                }
            }
        }

        private bool IsAdjacent(Cell a, Cell b)
        {
            return (System.Math.Abs(a.Row - b.Row) == 1 && a.Col == b.Col) ||
                   (System.Math.Abs(a.Col - b.Col) == 1 && a.Row == b.Row);
        }

        public void HighlightCell(Cell cell, Brush color)
        {
            double x = cell.Col * _cellSize;
            double y = cell.Row * _cellSize;

            var rect = new Rectangle()
            {
                Width = _cellSize,
                Height = _cellSize,
                Fill = color,
                Opacity = 0.5
            };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            _canvas.Children.Add(rect);
        }
    }
}
