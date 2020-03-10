using System;
using System.Collections.Generic;

namespace MarsRover
{
    public class Rover
    {
        private readonly IGrid Grid;

        public Rover(IGrid grid)
        {
            Grid = grid;
        }

        public (int x, int y) Position { get; set; }
        public char Direction
        {
            get
            {
                return directions[currentDirectionIndex];
            }

            set
            {
                var index = Array.IndexOf(directions, value);
                if (index >= 0)
                    currentDirectionIndex = index;
            }
        }

        private readonly char[] directions = new[] { 'N', 'E', 'S', 'W' };
        private int currentDirectionIndex = 0;

        private readonly IDictionary<char, (int, int)> ForwardMovementPositionDeltas =
            new Dictionary<char, (int, int)>
                {
                    {'N', (0, 1)},
                    {'S', (0, -1)},
                    {'W', (-1, 0)},
                    {'E', (1, 0)}
                };

        private readonly IDictionary<char, (int, int)> BackwardMovementPositionDeltas =
            new Dictionary<char, (int, int)>
                {
                    {'N', (0, -1)},
                    {'S', (0, 1)},
                    {'W', (1, 0)},
                    {'E', (-1, 0)}
                };

        public void MoveForward()
        {
            try
            {
                Position = Grid.WrapAround(Position, ForwardMovementPositionDeltas[Direction]);
            }
            catch (ObstacleException)
            {
                throw;
            }
        }

        public void MoveBackward()
        {
            try
            {
                Position = Grid.WrapAround(Position, BackwardMovementPositionDeltas[Direction]);
            }
            catch (ObstacleException)
            {
                throw;
            }
        }

        public void TurnLeft()
        {
            currentDirectionIndex = (currentDirectionIndex + 3) % 4;
        }

        public void TurnRight()
        {
            currentDirectionIndex = (currentDirectionIndex + 1) % 4;
        }

        public void ExecuteCommand(char command)
        {
            try
            {
                switch (command)
                {
                    case 'l':
                        TurnLeft();
                        break;
                    case 'r':
                        TurnRight();
                        break;
                    case 'f':
                        MoveForward();
                        break;
                    case 'b':
                        MoveBackward();
                        break;
                }
            }
            catch (ObstacleException)
            {
                throw;
            }
        }

        public void ExecuteCommandSequence(char[] sequence)
        {
            try
            {
                foreach (var command in sequence)
                {
                    Console.Write($"Executing command {command}: ");
                    ExecuteCommand(command);
                    Console.WriteLine($"Success. Current position {Position}. Facing {Direction}");
                }
            }
            catch (ObstacleException ex)
            {
                Console.WriteLine($"Coudln't execute command. Obstacle detected at {ex.Position}");
            }
        }
    }
}
