// File: AmazingTests/MazeGeneratorTests.cs
using FluentAssertions;
using Xunit;
using Amazing.Models;
using Amazing.Generation;

namespace AmazingTests
{
    public class MazeGeneratorTests
    {
        [Fact]
        public void GenerateMaze_ShouldMarkAllCellsAsVisited()
        {
            // Arrange
            var maze = new Maze(10, 10);
            var generator = new MazeGenerator(maze);

            // Act
            generator.GenerateMaze();

            // Assert
            foreach (var cell in maze.Cells)
            {
                cell.Visited.Should().BeTrue(because: $"cell at ({cell.Row}, {cell.Col}) should be visited after maze generation");
            }
        }

        [Fact]
        public void GenerateMaze_ShouldRemoveSomeWalls()
        {
            // Arrange
            var maze = new Maze(10, 10);
            var generator = new MazeGenerator(maze);

            // Act
            generator.GenerateMaze();

            // Assert
            int wallsRemoved = 0;
            foreach (var cell in maze.Cells)
            {
                wallsRemoved += cell.Walls.Count(w => !w);
            }
            wallsRemoved.Should().BeGreaterThan(0, because: "at least one wall should be removed during maze generation");
        }
    }
}
