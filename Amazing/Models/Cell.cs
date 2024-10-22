namespace Amazing.Models
{
    public class Cell
    {
        public int Row { get; }
        public int Col { get; }
        public bool[] Walls { get; set; } // Top, Right, Bottom, Left
        public bool Visited { get; set; }

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            Walls = [true, true, true, true];
            Visited = false;
        }
    }
}
