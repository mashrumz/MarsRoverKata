using System;
using System.Runtime.Serialization;

namespace MarsRover
{
    [Serializable]
    public class ObstacleException : Exception, ISerializable
    {
        public readonly (int x, int y) Position;

        public ObstacleException()
        {
        }

        public ObstacleException((int x, int y) position)
        {
            this.Position = position;
        }

        public ObstacleException(string message) : base(message)
        {
        }

        public ObstacleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ObstacleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}