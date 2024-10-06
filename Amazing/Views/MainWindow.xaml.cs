// File: Views/MainWindow.xaml.cs
using System;
using System.Threading.Tasks;
using System.Windows;
using Amazing.Models;
using Amazing.Generation;
using Amazing.Drawing;
using Amazing.Solvers;
using System.Windows.Controls;

namespace Amazing
{
    public partial class MainWindow : Window
    {
        // Default maze dimensions
        private int rows = 60;
        private int cols = 60;
        private double cellSize;

        private Maze maze;
        private MazeGenerator generator;
        private MazeDrawer drawer;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the "Generate and Solve Amazing Maze" button click.
        /// </summary>
        private async void GenerateSolveMaze_Click(object sender, RoutedEventArgs e)
        {
            // Disable the button to prevent multiple clicks
            GenerateSolveMazeButton.IsEnabled = false;
            try
            {
                // Generate the maze
                GenerateMaze();

                // Solve the maze asynchronously
                await SolveMaze();
            }
            catch (Exception ex)
            {
                // Display any unexpected errors
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Re-enable the button after processing
                GenerateSolveMazeButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Generates the maze using MazeGenerator and draws it using MazeDrawer.
        /// </summary>
        private void GenerateMaze()
        {
            // Optional: Retrieve rows and cols from user inputs if implemented
            // For simplicity, using default values. Uncomment below if you have input fields.
            /*
            if (int.TryParse(RowsTextBox.Text, out int inputRows) && int.TryParse(ColsTextBox.Text, out int inputCols))
            {
                if (inputRows > 0 && inputCols > 0)
                {
                    rows = inputRows;
                    cols = inputCols;
                }
                else
                {
                    MessageBox.Show("Rows and Columns must be positive integers. Using default values.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid input for Rows or Columns. Using default values.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            */

            // Initialize the maze
            maze = new Maze(rows, cols);

            // Generate the maze structure
            generator = new MazeGenerator(maze);
            generator.GenerateMaze();

            // Calculate cell size based on current canvas size
            CalculateCellSize();

            // Initialize the drawer and draw the maze
            drawer = new MazeDrawer(MazeCanvas, cellSize);
            drawer.DrawMaze(maze);
        }

        /// <summary>
        /// Calculates the size of each cell to ensure the maze fits within the canvas.
        /// </summary>
        private void CalculateCellSize()
        {
            // Get the available width and height from the MazeCanvas
            double canvasWidth = MazeCanvas.ActualWidth;
            double canvasHeight = MazeCanvas.ActualHeight;

            // Fallback to ScrollViewer size if Canvas size is not initialized
            if (canvasWidth == 0 || canvasHeight == 0)
            {
                canvasWidth = MazeScrollViewer.ActualWidth;
                canvasHeight = MazeScrollViewer.ActualHeight;
            }

            // Calculate the cell size to fit the maze within the available space
            cellSize = Math.Min(canvasWidth / cols, canvasHeight / rows);
        }

        /// <summary>
        /// Solves the maze using the selected solver.
        /// </summary>
        private async Task SolveMaze()
        {
            // Get the selected solver based on user selection
            IMazeSolver solver = GetSelectedSolver();

            if (solver == null)
            {
                MessageBox.Show("Please select a solver algorithm.", "No Solver Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Solve the maze
            var path = await solver.SolveMaze(maze, drawer);

            if (path != null)
            {
                // Draw the solution path
                drawer.DrawSolutionPath(path);
            }
            else
            {
                MessageBox.Show("No path found!", "Maze Solver", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Determines which solver is selected by the user.
        /// </summary>
        /// <returns>An instance of a class that implements IMazeSolver.</returns>
        private IMazeSolver GetSelectedSolver()
        {
            if (DFS_RadioButton.IsChecked == true)
                return new DFSMazeSolver();
            if (BFS_RadioButton.IsChecked == true)
                return new BFSMazeSolver();

            return null; // No solver selected
        }

        /// <summary>
        /// Event handler for window size changes to scale the maze accordingly.
        /// </summary>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (maze != null && drawer != null)
            {
                // Recalculate cell size based on new window size
                CalculateCellSize();

                // Re-initialize the drawer with the new cell size
                drawer = new MazeDrawer(MazeCanvas, cellSize);

                // Redraw the maze with the updated cell size
                drawer.DrawMaze(maze);
            }
        }
    }
}
