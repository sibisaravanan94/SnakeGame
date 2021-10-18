using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        public LinkedList snake { get; set; }
        public HashSet<int> snakeCells { get; set; }
        public Direction direction { get; set; }
        public int foodCell { get; set; }
        public int score { get; set; }
        protected override async Task OnInitializedAsync()
        {

            testCounter = 0;
            score = 0;
            setBoard();
            setDirection(Direction.RIGHT);
            snake = setsnake();
            snakeCells = new HashSet<int>();
            snakeCells.Add(snake.head.value.cell);
            foodCell = snake.head.value.cell + 5;
            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += async (s, e) =>
            {
                testCounter++;
                moveSnake();
                await InvokeAsync(StateHasChanged);
            };
            t.Interval = 500;
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
            if (snakeCells.Contains(cellValue))
                className = "cell cell-green";
            else if (cellValue == foodCell)
                className = "cell cell-red";
            else
                className = "cell";
            return className;
        }

        public void moveSnake()
        {
            Node newHeadNode = getCoordsInDirection();
            if (isOutOfBounds(newHeadNode))
            {
                handleGameOver();
                return;
            }
            newHeadNode.cell = board[newHeadNode.row, newHeadNode.col];
            if (snakeCells.Contains(newHeadNode.cell))
            { 
                handleGameOver();
                return;
            }

            LinkedListNode newHead = new LinkedListNode(newHeadNode);
            LinkedListNode currentHead = snake.head;
            snake.head = newHead;
            currentHead.next = newHead;

            

            snakeCells.Remove(snake.tail.value.cell);
            snakeCells.Add(snake.head.value.cell);

            snake.tail = snake.tail.next;
            if (snake.tail == null)
                snake.tail = snake.head;

            bool foodConsumed = snake.head.value.cell == foodCell;
            if (foodConsumed)
            {
                foodCell = handleFoodConsumption();
                score++;
            }
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
        public void setDirection(Direction newDirection)
        {
            direction = newDirection;
        }
        public Node getCoordsInDirection()
        {
            int row = snake.head.value.row;
            int col = snake.head.value.col;
            //int cell = board[snake.head.value.row, snake.head.value.col];
            if (direction == Direction.UP)
            {
                row --;
            }
            if (direction == Direction.RIGHT)
            {
                col++;
            }
            if (direction == Direction.DOWN)
            {
                row++;
            }
            if (direction == Direction.LEFT)
            {
                col--;
            }
            //cell = board[row, col];
            return new Node(row,col,0);
        }
        protected void KeyDown(KeyboardEventArgs e)
        {
            int newDirection = getDirectionFromKey(e.Key);
            if (newDirection == -1 || (Direction)newDirection == direction)
                return;
            setDirection((Direction)newDirection);
        }

        public int getDirectionFromKey(string key)
        {
            if (key == "ArrowUp") return (int)Direction.UP;
            if (key == "ArrowRight") return (int)Direction.RIGHT;
            if (key == "ArrowDown") return (int)Direction.DOWN;
            if (key =="ArrowLeft") return (int)Direction.LEFT;
            return -1;
        }
        public int handleFoodConsumption()
        {
            int maxPossibleCellValue = boardSize * boardSize;
            int nextFoodCell;

            while (true)
            {
                nextFoodCell = randomIntFromInterval(1, maxPossibleCellValue);
                if (snakeCells.Contains(nextFoodCell) || foodCell == nextFoodCell)
                    continue;
                break;
            }
            return nextFoodCell;
        }
        public int randomIntFromInterval(int start, int end)
        {
            Random rand = new Random();
            int number = rand.Next(start, end+1);
            return number;
        }
        public bool isOutOfBounds(Node newHeadNode)
        {
            int row = newHeadNode.row;
            int col = newHeadNode.col;
            if (row < 0 || col < 0)
                return true;
            if (row >= boardSize || col >= boardSize)
                return true;
            return false;
        }
        public void handleGameOver()
        {
            score = 0;
            setDirection(Direction.RIGHT);
            snake = setsnake();
            snakeCells = new HashSet<int>();
            snakeCells.Add(snake.head.value.cell);
            foodCell = snake.head.value.cell + 5;
        }
    }

}
