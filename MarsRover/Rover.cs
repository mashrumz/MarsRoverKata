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

        public bool MoveForward()
        {
            try
            {
                Position = Grid.WrapAround(Position, ForwardMovementPositionDeltas[Direction]);
                return true;
            }
            catch (ObstacleException)
            {
                return false;
            }
        }

        public bool MoveBackward()
        {
            try
            {
                Position = Grid.WrapAround(Position, BackwardMovementPositionDeltas[Direction]);
                return true;
            }
            catch (ObstacleException)
            {
                return false;
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
    }
}
