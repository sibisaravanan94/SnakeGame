using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Classes
{
    public class Node
    {
        public int row { get; set; }
        public int col { get; set; }
        public int cell { get; set; }

        public Node(int row, int col, int cell)
        {
            this.row = row;
            this.col = col;
            this.cell = cell;
        }
    }
}
