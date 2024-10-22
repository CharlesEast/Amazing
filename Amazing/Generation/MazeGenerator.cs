using Amazing.Models;

namespace Amazing.Generation
{
    public class MazeGenerator
    {
        private readonly Maze _maze;
        private readonly Random _rand;

        public MazeGenerator(Maze maze)
        {
            _maze = maze;
            _rand = new Random();
        }

        public void GenerateMaze()
        {
            var stack = new Stack<Cell>();
            var current = _maze.Cells[0, 0];
            current.Visited = true;
            stack.Push(current);

            while (stack.Count > 0)
            {
                current = stack.Pop();
                var neighbors = GetUnvisitedNeighbors(current);

                if (neighbors.Count > 0)
                {
                    stack.Push(current);

                    // Choose a random neighbor
                    var next = neighbors[_rand.Next(neighbors.Count)];
                    RemoveWalls(current, next);
                    next.Visited = true;
                    stack.Push(next);
                }
            }
        }

        private List<Cell> GetUnvisitedNeighbors(Cell cell)
        {
            var neighbors = new List<Cell>();
            int row = cell.Row;
            int col = cell.Col;

            // Top
            if (row > 0 && !_maze.Cells[row - 1, col].Visited)
                neighbors.Add(_maze.Cells[row - 1, col]);
            // Right
            if (col < _maze.Cols - 1 && !_maze.Cells[row, col + 1].Visited)
                neighbors.Add(_maze.Cells[row, col + 1]);
            // Bottom
            if (row < _maze.Rows - 1 && !_maze.Cells[row + 1, col].Visited)
                neighbors.Add(_maze.Cells[row + 1, col]);
            // Left
            if (col > 0 && !_maze.Cells[row, col - 1].Visited)
                neighbors.Add(_maze.Cells[row, col - 1]);

            return neighbors;
        }

        private void RemoveWalls(Cell current, Cell next)
        {
            int dr = next.Row - current.Row;
            int dc = next.Col - current.Col;

            if (dr == -1) // Next is top of current
            {
                current.Walls[0] = false;
                next.Walls[2] = false;
            }
            else if (dr == 1) // Next is bottom of current
            {
                current.Walls[2] = false;
                next.Walls[0] = false;
            }
            else if (dc == -1) // Next is left of current
            {
                current.Walls[3] = false;
                next.Walls[1] = false;
            }
            else if (dc == 1) // Next is right of current
            {
                current.Walls[1] = false;
                next.Walls[3] = false;
            }
        }
    }
}
