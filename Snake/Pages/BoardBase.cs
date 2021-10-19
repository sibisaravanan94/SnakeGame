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
        public double PROBABILITY_OF_DIRECTION_REVERSAL_FOOD { get; set; }
        public bool foodShouldReverseDirection { get; set; }
        protected override async Task OnInitializedAsync()
        {
            score = 0;
            PROBABILITY_OF_DIRECTION_REVERSAL_FOOD = 0.3;
            foodShouldReverseDirection = false;
            setBoard();
            setDirection(Direction.RIGHT);
            snake = setsnake();
            snakeCells = new HashSet<int>();
            snakeCells.Add(snake.head.value.cell);
            foodCell = snake.head.value.cell + 5;
            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += async (s, e) =>
            {
                moveSnake();
                await InvokeAsync(StateHasChanged);
            };
            t.Interval = 150;
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
                className = (foodShouldReverseDirection)? "cell cell-purple" : "cell cell-red";
            else
                className = "cell";
            return className;
        }

        public void moveSnake()
        {
            Node newHeadNode = getCoordsInDirection(snake.head, direction);
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
                growSnake();
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
        public Node getCoordsInDirection(LinkedListNode node, Direction newdirection)
        {
            int row = node.value.row;
            int col = node.value.col;
            //int cell = board[snake.head.value.row, snake.head.value.col];
            if (newdirection == Direction.UP)
            {
                row --;
            }
            if (newdirection == Direction.RIGHT)
            {
                col++;
            }
            if (newdirection == Direction.DOWN)
            {
                row++;
            }
            if (newdirection == Direction.LEFT)
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
            Direction oppositeDirection = getOppositeDirection((int)direction);
            if (snakeCells.Count > 1 && (Direction)newDirection == oppositeDirection)
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
            
            foodShouldReverseDirection = (randomIntFromInterval(1, 11) < 3) ? true : false;
            
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

        public void growSnake()
        {
            Node newTailNode = getGrowthNodeCoords();
            if (isOutOfBounds(newTailNode))
            {
                handleGameOver();
                return;
            }
            newTailNode.cell = board[newTailNode.row, newTailNode.col];

            LinkedListNode newTail = new LinkedListNode(newTailNode);
            LinkedListNode currentTail = snake.tail;
            snake.tail = newTail;
            snake.tail.next = currentTail;

            snakeCells.Add(newTailNode.cell);
        }

        public Node getGrowthNodeCoords()
        {
            int tailDirection = getNextNodeDirection();
            Direction growthDirection = getOppositeDirection(tailDirection);
            Node newtailNode = getCoordsInDirection(snake.tail, growthDirection);
            return newtailNode;
        }
        public int getNextNodeDirection()
        {
            LinkedListNode tail = snake.tail;
            if (tail.next == null) 
                return (int)direction;
            int nextRow = tail.next.value.row;
            int nextCol = tail.next.value.col;
            int currentRow = tail.value.row;
            int currentCol = tail.value.col;
            if (nextRow == currentRow && nextCol == currentCol + 1)
            {
                return (int)Direction.RIGHT;
            }
            if (nextRow == currentRow && nextCol == currentCol - 1)
            {
                return (int)Direction.LEFT;
            }
            if (nextCol == currentCol && nextRow == currentRow + 1)
            {
                return (int)Direction.DOWN;
            }
            if (nextCol == currentCol && nextRow == currentRow - 1)
            {
                return (int)Direction.UP;
            }
            return -1;
        }
        public Direction getOppositeDirection(int direction)
        {
            if ((Direction)direction == Direction.UP) return Direction.DOWN;
            if ((Direction)direction == Direction.RIGHT) return Direction.LEFT;
            if ((Direction)direction == Direction.DOWN) return Direction.UP;
            if ((Direction)direction == Direction.LEFT) return Direction.RIGHT;
            return Direction.RIGHT;
        }
    }

}
