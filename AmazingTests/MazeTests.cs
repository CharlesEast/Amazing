// File: AmazingTests/MazeTests.cs
using FluentAssertions;
using Xunit;
using Amazing.Models;

namespace AmazingTests
{
    public class MazeTests
    {
        [Fact]
        public void MazeInitialization_ShouldCreateCorrectSize()
        {
            // Arrange
            int rows = 20;
            int cols = 30;

            // Act
            var maze = new Maze(rows, cols);

            // Assert
            maze.Rows.Should().Be(rows, because: "the maze should have the specified number of rows");
            maze.Cols.Should().Be(cols, because: "the maze should have the specified number of columns");
            maze.Cells.GetLength(0).Should().Be(rows, because: "the cells should match the number of rows");
            maze.Cells.GetLength(1).Should().Be(cols, because: "the cells should match the number of columns");
        }
    }
}
