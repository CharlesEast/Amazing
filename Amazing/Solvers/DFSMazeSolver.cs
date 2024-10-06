// File: Solvers/DFSMazeSolver.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazing.Models;
using Amazing.Drawing;
using System.Windows.Media;

namespace Amazing.Solvers
{
    public class DFSMazeSolver : IMazeSolver
    {
        public async Task<List<Cell>> SolveMaze(Maze maze, MazeDrawer drawer)
        {
            var stack = new Stack<Cell>();
            var visited = new bool[maze.Rows, maze.Cols];
            var predecessor = new Cell[maze.Rows, maze.Cols];

            var start = maze.Cells[0, 0];
            var end = maze.Cells[maze.Rows - 1, maze.Cols - 1];

            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (visited[current.Row, current.Col])
                    continue;

                visited[current.Row, current.Col] = true;
                drawer.HighlightCell(current, Brushes.LightBlue);
                await Task.Delay(10); // Adjust delay as needed for visualization

                if (current == end)
                {
                    return ReconstructPath(predecessor, start, end);
                }

                var neighbors = GetAccessibleNeighbors(current, maze);
                // Shuffle neighbors to ensure randomness
                Shuffle(neighbors);

                foreach (var neighbor in neighbors)
                {
                    if (!visited[neighbor.Row, neighbor.Col])
                    {
                        stack.Push(neighbor);
                        predecessor[neighbor.Row, neighbor.Col] = current;
                    }
                }
            }

            return null; // No path found
        }

        private List<Cell> ReconstructPath(Cell[,] predecessor, Cell start, Cell end)
        {
            var path = new List<Cell>();
            var current = end;

            while (current != null)
            {
                path.Add(current);
                if (current == start)
                    break;
                current = predecessor[current.Row, current.Col];
            }

            path.Reverse();
            return path;
        }

        private List<Cell> GetAccessibleNeighbors(Cell cell, Maze maze)
        {
            var neighbors = new List<Cell>();
            int row = cell.Row;
            int col = cell.Col;
            var cells = maze.Cells;

            // Top
            if (!cell.Walls[0] && row > 0)
                neighbors.Add(cells[row - 1, col]);
            // Right
            if (!cell.Walls[1] && col < maze.Cols - 1)
                neighbors.Add(cells[row, col + 1]);
            // Bottom
            if (!cell.Walls[2] && row < maze.Rows - 1)
                neighbors.Add(cells[row + 1, col]);
            // Left
            if (!cell.Walls[3] && col > 0)
                neighbors.Add(cells[row, col - 1]);

            return neighbors;
        }

        private void Shuffle<T>(List<T> list)
        {
            var rand = new System.Random();
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int j = rand.Next(i, n);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
