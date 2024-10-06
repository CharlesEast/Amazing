using System.Collections.Generic;
using System.Threading.Tasks;
using Amazing.Drawing;
using Amazing.Models;

namespace Amazing.Solvers
{
    public interface IMazeSolver
    {
        Task<List<Cell>> SolveMaze(Maze maze, MazeDrawer drawer);
    }
}
