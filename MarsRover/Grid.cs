using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public interface IGrid
    {
        (int, int) WrapAround((int x, int y) position, (int x, int y) delta);
        void AddObstacle((int, int) position);
        void AddObstacles(IEnumerable<(int, int)> positions);
    }

    public class Grid : IGrid
    {
        private readonly int Width;
        private readonly int Height;

        private readonly HashSet<(int, int)> obstacles = new HashSet<(int, int)>();

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void AddObstacle((int, int) position)
        {
            obstacles.Add(position);
        }

        public void AddObstacles(IEnumerable<(int, int)> positions)
        {
            foreach (var position in positions)
            {
                AddObstacle(position);
            }
        }

        public (int, int) WrapAround((int x, int y) position, (int x, int y) delta)
        {
            var newPosition = (x: position.x + delta.x, y: position.y + delta.y);

            if (newPosition.x >= Width || newPosition.x < 0)
            {
                newPosition.x = (newPosition.x + Width) % Width;
            }

            if (newPosition.y >= Height || newPosition.y < 0)
            {
                newPosition.y = (newPosition.y + Height) % Height;
            }

            if (obstacles.Contains(newPosition))
                throw new ObstacleException(newPosition);

            return newPosition;
        }
    }
}
