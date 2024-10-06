// File: Amazing/Drawing/IMazeDrawer.cs
using System.Collections.Generic;
using System.Windows.Media;
using Amazing.Models;

namespace Amazing.Drawing
{
    public interface IMazeDrawer
    {
        void DrawMaze(Maze maze);
        void DrawSolutionPath(List<Cell> path);
        void HighlightCell(Cell cell, Brush color);
    }
}
