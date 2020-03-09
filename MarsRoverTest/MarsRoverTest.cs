using MarsRover;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarsRoverTest
{
    public class Tests
    {
        private static IEnumerable<TestCaseData> ForwardMovementTestData
        {
            get
            {
                yield return new TestCaseData((1,1), 'N', (1,2));
                yield return new TestCaseData((1, 1), 'S', (1,0));
                yield return new TestCaseData((1, 1), 'E', (2,1));
                yield return new TestCaseData((1, 1), 'W', (0,1));
            }
        }

        private static IEnumerable<TestCaseData> BackwardMovementTestData
        {
            get
            {
                yield return new TestCaseData((1, 1), 'N', (1,0));
                yield return new TestCaseData((1, 1), 'S', (1,2));
                yield return new TestCaseData((1, 1), 'E', (0,1));
                yield return new TestCaseData((1, 1), 'W', (2,1));
            }
        }

        private static IEnumerable<TestCaseData> TurnLeftTestData
        {
            get
            {
                yield return new TestCaseData('N', 'W');
                yield return new TestCaseData('W', 'S');
                yield return new TestCaseData('S', 'E');
                yield return new TestCaseData('E', 'N');
            }
        }

        private static IEnumerable<TestCaseData> TurnRightTestData
        {
            get
            {
                yield return new TestCaseData('N', 'E');
                yield return new TestCaseData('E', 'S');
                yield return new TestCaseData('S', 'W');
                yield return new TestCaseData('W', 'N');
            }
        }

        private Rover rover;

        [SetUp]
        public void Setup()
        {
            rover = new Rover(3,3);
        }

        [Test, TestCaseSource(nameof(ForwardMovementTestData))]
        public void MoveForwardTest((int x, int y) startingPosition, char direction, (int x, int y) expectedPosition)
        {
            rover.Position = startingPosition;
            rover.Direction = direction;
            rover.MoveForward();
            Assert.AreEqual(expectedPosition, rover.Position);
        }

        [Test, TestCaseSource(nameof(BackwardMovementTestData))]
        public void MoveBackwardTest((int x, int y) startingPosition, char direction, (int x, int y) expectedPosition)
        {
            rover.Position = startingPosition;
            rover.Direction = direction;
            rover.MoveBackward();
            Assert.AreEqual(expectedPosition, rover.Position);
        }

        [Test, TestCaseSource(nameof(TurnLeftTestData))]
        public void TurnLeftTest(char direction, char expectedDirection)
        {
            rover.Direction = direction;
            rover.TurnLeft();
            Assert.AreEqual(expectedDirection, rover.Direction);
        }

        [Test, TestCaseSource(nameof(TurnRightTestData))]
        public void TurnRightTest(char direction, char expectedDirection)
        {
            rover.Direction = direction;
            rover.TurnRight();
            Assert.AreEqual(expectedDirection, rover.Direction);
        }

        [Test]
        public void MovingForwardNorthSouthWrappingTest()
        {
            rover.Direction = 'N';
            rover.Position = (0,2);
            rover.MoveForward();
            Assert.AreEqual((0,0), rover.Position);
        }

        [Test]
        public void MovingBackwardNorthSouthWrappingTest()
        {
            rover.Direction = 'S';
            rover.Position = (0, 2);
            rover.MoveBackward();
            Assert.AreEqual((0, 0), rover.Position);
        }

        [Test]
        public void MovingForwardSouthNorthWrappingTest()
        {
            rover.Direction = 'S';
            rover.Position = (0, 0);
            rover.MoveForward();
            Assert.AreEqual((0, 2), rover.Position);
        }

        [Test]
        public void MovingBackwardSouthNorthWrappingTest()
        {
            rover.Direction = 'N';
            rover.Position = (0, 0);
            rover.MoveBackward();
            Assert.AreEqual((0, 2), rover.Position);
        }

        [Test]
        public void MovingForwardEastWestWrappingTest()
        {
            rover.Direction = 'E';
            rover.Position = (2, 0);
            rover.MoveForward();
            Assert.AreEqual((0, 0), rover.Position);
        }

        [Test]
        public void MovingBackwardEastWestWrappingTest()
        {
            rover.Direction = 'W';
            rover.Position = (2, 0);
            rover.MoveBackward();
            Assert.AreEqual((0, 0), rover.Position);
        }

        [Test]
        public void MovingForwardWestEastWrappingTest()
        {
            rover.Direction = 'W';
            rover.Position = (0, 0);
            rover.MoveForward();
            Assert.AreEqual((2, 0), rover.Position);
        }

        [Test]
        public void MovingBackwardWestEastWrappingTest()
        {
            rover.Direction = 'E';
            rover.Position = (0, 0);
            rover.MoveBackward();
            Assert.AreEqual((2, 0), rover.Position);
        }
    }
}