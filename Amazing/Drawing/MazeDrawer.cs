using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Amazing.Models;

namespace Amazing.Drawing
{
    public class MazeDrawer
    {
        private readonly Canvas _canvas;
        private double _cellSize;

        public MazeDrawer(Canvas canvas, double cellSize)
        {
            _canvas = canvas;
            _cellSize = cellSize;
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
        }

        private void DrawLine(double x1, double y1, double x2, double y2, Brush color)
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
