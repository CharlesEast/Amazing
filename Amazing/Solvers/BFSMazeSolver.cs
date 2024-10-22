// File: Amazing/Solvers/BFSMazeSolver.cs
using Amazing.Models;
using Amazing.Drawing;
using System.Windows.Media;

namespace Amazing.Solvers
{
    public class BFSMazeSolver : IMazeSolver
    {
        public async Task<List<Cell>> SolveMaze(Maze maze, IMazeDrawer drawer)
        {
            var queue = new Queue<Cell>();
            var visited = new bool[maze.Rows, maze.Cols];
            var predecessor = new Cell[maze.Rows, maze.Cols];

            var start = maze.Cells[0, 0];
            var end = maze.Cells[maze.Rows - 1, maze.Cols - 1];

            queue.Enqueue(start);
            visited[start.Row, start.Col] = true;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                // Optionally, highlight the current cell as it's being processed
                drawer.HighlightCell(current, Brushes.LightBlue);
                await Task.Delay(10); // Adjust delay as needed for visualization

                if (current == end)
                {
                    return ReconstructPath(predecessor, start, end) ?? new List<Cell>();
                }

                foreach (var neighbor in GetAccessibleNeighbors(current, maze))
                {
                    if (!visited[neighbor.Row, neighbor.Col])
                    {
                        queue.Enqueue(neighbor);
                        visited[neighbor.Row, neighbor.Col] = true;
                        predecessor[neighbor.Row, neighbor.Col] = current;
                    }
                }
            }

            return new List<Cell>(); // No path found
        }

        private List<Cell>? ReconstructPath(Cell[,] predecessor, Cell start, Cell end)
        {
            var path = new List<Cell>();
            var current = end;

            while (current != start)
            {
                path.Add(current);
                current = predecessor[current.Row, current.Col];
                if (current == null)
                    return null; // No path found
            }

            path.Add(start);
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
    }
}
