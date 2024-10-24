﻿// File: Amazing/Solvers/IMazeSolver.cs
using Amazing.Models;
using Amazing.Drawing;

namespace Amazing.Solvers
{
    public interface IMazeSolver
    {
        Task<List<Cell>> SolveMaze(Maze maze, IMazeDrawer drawer);
    }
}
