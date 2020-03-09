using System;
using System.Collections.Generic;

namespace MarsRover
{
    public class Rover
    {
        private readonly int Width;
        private readonly int Height;

        public Rover(int width, int height)
        {
            Width = width;
            Height = height;
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
            CalculateNewPosition(ForwardMovementPositionDeltas);
        }

        public void MoveBackward()
        {
            CalculateNewPosition(BackwardMovementPositionDeltas);
        }

        private void CalculateNewPosition(IDictionary<char, (int, int)> positionDeltas)
        {
            var (dx, dy) = positionDeltas[Direction];
            (int x, int y) newPosition = Position;
            newPosition.x += dx;
            newPosition.y += dy;
            WrapAroundTheGrid(newPosition);
        }

        private void WrapAroundTheGrid((int x, int y) newPosition)
        {
            if (newPosition.x >= Width || newPosition.x < 0)
            {
                newPosition.x = (newPosition.x + Width) % Width;
            }

            if (newPosition.y >= Height || newPosition.y < 0)
            {
                newPosition.y = (newPosition.y + Height) % Height;
            }

            Position = newPosition;
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
