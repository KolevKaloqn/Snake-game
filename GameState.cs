using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();

        //This list contains the positions currently ocupied by the snake
        //We use a LinkedList because it allows us to delete and add from both ends of the list
        //The first element is the head of the snake and the last element is the tail
        private readonly LinkedList<Possition> snakePosition = new LinkedList<Possition>();

        //Will be used to figure out where the food should spawn
        private readonly Random random = new Random();

        public GameState(int rows, int cols) 
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            //when the game starts the snake's direction will be right
            Dir = Direction.Right;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            //we want the snake to be in the middle
            int r = Rows / 2;

            for (int i = 1; i <= 3; i++)
            {
                Grid[r, i] = GridValue.Snake;
                snakePosition.AddFirst(new Possition(r, i));
            }
        }

        private IEnumerable<Possition> EmptyPositions()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Possition(r, c);
                    }
                }
            }
        }

        private void AddFood()
        { 
            List<Possition> empty = new List<Possition> (EmptyPositions());

            if (empty.Count == 0 )
            {
                return;
            }

            Possition pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food;
        }

        public Possition HeadPosition()
        {
            return snakePosition.First.Value;
        }

        public Possition TailPosition()
        { 
            return snakePosition.Last.Value;
        }

        public IEnumerable<Possition> SnakePositions()
        {
            return snakePosition;
        }

        //The AddHead() and RemoveTail() methods are used to move the snake
        //In order to move the snake foreward we just need to make the tail position 0 (enum value ) on the grid and add the new head position
        //If there is food on the next position we dont remove the tail 
        private void AddHead(Possition pos)
        { 
            snakePosition.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Possition tail = snakePosition.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakePosition.RemoveLast();
        }

        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0)
            {
                //if the list is empty we return the curr direction
                return Dir;
            }
            //we return the last chage in the list to improve user experience
            return dirChanges.Last.Value;
        }

        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 2)
            {
                //if the list has 2 directions it is full and cant take more directions
                //otherwise the user can spam keys
                return false;
            }

            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }

        public void ChageDirection(Direction dir)
        {
            //if can chage direction
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
        }

        
        private bool OutsideGrid(Possition pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Cols;
        }

        //If the snake goes outside of the Grid or hits itself the game will end
        private GridValue WillHit(Possition newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if (newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Column];
        }

        public void Move()
        {
            if (dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            Possition newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if (hit == GridValue.Outside || hit == GridValue.Snake) 
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
