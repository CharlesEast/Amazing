namespace Amazing.Models
{
    public class Maze
    {
        public int Rows { get; }
        public int Cols { get; }
        public Cell[,] Cells { get; }

        public Maze(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Cells = new Cell[Rows, Cols];

            InitializeCells();
        }

        private void InitializeCells()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    Cells[r, c] = new Cell(r, c);
        }
    }
}
