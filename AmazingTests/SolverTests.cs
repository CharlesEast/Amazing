// File: AmazingTests/SolverTests.cs
using FluentAssertions;
using Xunit;
using Amazing.Models;
using Amazing.Generation;
using Amazing.Solvers;
using Amazing.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Media;

namespace AmazingTests
{
    public class SolverTests
    {
        [Theory]
        [InlineData(typeof(DFSMazeSolver))]
        [InlineData(typeof(BFSMazeSolver))]
        public async Task Solvers_ShouldFindPath_WhenPathExists(Type solverType)
        {
            // Arrange
            var maze = new Maze(10, 10);
            var generator = new MazeGenerator(maze);
            generator.GenerateMaze();
            var drawer = new MazeDrawerStub();

            IMazeSolver solver = (IMazeSolver)Activator.CreateInstance(solverType);

            // Act
            var path = await solver.SolveMaze(maze, drawer);

            // Assert
            path.Should().NotBeNull(because: $"{solverType.Name} should find a path in a generated maze");
            path.Should().NotBeEmpty(because: "a valid path should contain at least the start and end cells");
            path[0].Should().Be(maze.Cells[0, 0], because: "the path should start at the maze's entry point");
            path[^1].Should().Be(maze.Cells[maze.Rows - 1, maze.Cols - 1], because: "the path should end at the maze's exit point");
        }

        [Fact]
        public async Task Solver_ShouldHandleEmptyMaze()
        {
            // Arrange
            var maze = new Maze(5, 5);

            // Remove all walls
            foreach (var cell in maze.Cells)
            {
                cell.Walls = new bool[] { false, false, false, false };
            }

            var solver = new BFSMazeSolver();
            var drawer = new MazeDrawerStub();

            // Act
            var path = await solver.SolveMaze(maze, drawer);

            // Assert
            path.Should().NotBeNull(because: "there should be a path in an empty maze");
            path.Should().ContainInOrder(maze.Cells[0, 0], maze.Cells[4, 4]);
        }

        [Fact]
        public async Task Solver_ShouldReturnNull_ForFullyBlockedMaze()
        {
            // Arrange
            var maze = new Maze(5, 5);

            // All walls are intact by default
            var solver = new BFSMazeSolver();
            var drawer = new MazeDrawerStub();

            // Act
            var path = await solver.SolveMaze(maze, drawer);

            // Assert
            path.Should().BeNull(because: "no path exists in a fully blocked maze");
        }

        [Fact]
        public async Task Solvers_Path_ShouldBeContinuousAndConnected()
        {
            // Arrange
            var maze = new Maze(10, 10);
            var generator = new MazeGenerator(maze);
            generator.GenerateMaze();
            var solver = new BFSMazeSolver();
            var drawer = new MazeDrawerStub();

            // Act
            var path = await solver.SolveMaze(maze, drawer);

            // Assert
            path.Should().NotBeNull();
            for (int i = 0; i < path.Count - 1; i++)
            {
                var current = path[i];
                var next = path[i + 1];
                // Check if next is a neighbor of current
                bool isNeighbor = (System.Math.Abs(current.Row - next.Row) == 1 && current.Col == next.Col) ||
                                  (System.Math.Abs(current.Col - next.Col) == 1 && current.Row == next.Row);
                isNeighbor.Should().BeTrue($"cell at ({current.Row}, {current.Col}) should be adjacent to cell at ({next.Row}, {next.Col})");

                // Check if there's no wall between them
                if (current.Row - next.Row == 1) // Next is top
                    current.Walls[0].Should().BeFalse("there should be no wall between current cell and its top neighbor");
                if (current.Row - next.Row == -1) // Next is bottom
                    current.Walls[2].Should().BeFalse("there should be no wall between current cell and its bottom neighbor");
                if (current.Col - next.Col == 1) // Next is left
                    current.Walls[3].Should().BeFalse("there should be no wall between current cell and its left neighbor");
                if (current.Col - next.Col == -1) // Next is right
                    current.Walls[1].Should().BeFalse("there should be no wall between current cell and its right neighbor");
            }
        }
    }

    // Stub for IMazeDrawer since we don't need actual drawing in tests
    public class MazeDrawerStub : IMazeDrawer
    {
        public void HighlightCell(Cell cell, Brush color)
        {
            // Stub implementation: Do nothing
        }

        public void DrawMaze(Maze maze)
        {
            // Stub implementation: Do nothing
        }

        public void DrawSolutionPath(List<Cell> path)
        {
            // Stub implementation: Do nothing
        }
    }
}
