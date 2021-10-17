using Microsoft.AspNetCore.Components;
using Snake.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Pages
{
    public class BoardBase : ComponentBase
    {
        public int[,] board { get; set; }
        public const int boardSize = 15;
        public int testCounter { get; set; }
        public HashSet<int> snakeCells { get; set; }
        protected override async Task OnInitializedAsync()
        {

            testCounter = 0;
            setBoard();
            LinkedList snake = setsnake();
            snakeCells = new HashSet<int>();
            snakeCells.Add(snake.head.value.cell);
            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += async (s, e) =>
            {
                testCounter++;
                await InvokeAsync(StateHasChanged);
            };
            t.Interval = 1000;
            t.Start(); 
            //return base.OnInitializedAsync();
        }

        public void setBoard()
        {
            board = new int[boardSize, boardSize];
            int counter = 1;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = counter;
                    counter++;
                }
            }
        }
        public LinkedList setsnake()
        {
            LinkedList node = new LinkedList(getStartingSnakeLLValue());
            return node;
        }
        public string getClassName(int cellValue, HashSet<int> snakeCells)
        {
            string className = "";
            if(snakeCells.Contains(cellValue))
                className = "cell cell-green";
            else
                className = "cell";
            return className;
        }

        public void moveSnake(int row, int col)
        {
            int[] nextHeadCoords = new int[2];
            int nextCell = board[nextHeadCoords[0], nextHeadCoords[1]];
            //LinkedListNode newHead = 
        }

        public Node getStartingSnakeLLValue()
        {
            int rowSize = board.GetLength(0);
            int colSize = board.GetLength(1);
            int startingRow = rowSize/3;
            int startingCol = colSize/3;
            int startingCell = board[startingRow,startingCol];
            
            return new Node(startingRow, startingCol, startingCell);
        }
    }

}
