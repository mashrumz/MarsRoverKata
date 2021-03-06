using ApprovalTests;
using ApprovalTests.Reporters;
using MarsRover;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarsRoverTest
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class RoverTest
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

        private static IEnumerable <TestCaseData> GridTestData
        {
            get
            {
                yield return new TestCaseData((1, 1));
                yield return new TestCaseData((2, 1));
                yield return new TestCaseData((1, 2));
                yield return new TestCaseData((2, 2));
            }
        }

        private static IEnumerable<TestCaseData> SequenceTestData
        {
            get
            {
                yield return new TestCaseData((0, 0), 'N', new[] { 'f', 'l', 'f', 'r', 'b'}, new[] {'N','W','W','N', 'N'}, new[] { (0, 1), (0, 1), (2, 1), (2, 1), (2,0) });
                yield return new TestCaseData((0, 2), 'N', new[] { 'f', 'l', 'f', 'r', 'b' }, new[] { 'N', 'W', 'W', 'N', 'N' }, new[] { (0, 0), (0, 0), (2, 0), (2, 0), (2, 2) });
                yield return new TestCaseData((1, 1), 'S', new[] { 'f', 'l', 'f', 'l', 'f', 'l', 'f' }, new[] { 'S', 'E', 'E', 'N', 'N', 'W', 'W' }, new[] { (1, 0), (1, 0), (2, 0), (2, 0), (2, 1), (2, 1), (1, 1) });
            }
        }

        private Rover rover;
        private Mock<IGrid> gridMock = new Mock<IGrid>();

        [SetUp]
        public void Setup()
        {
            gridMock.Reset();
            rover = new Rover(new Grid(3,3));
        }

        [Test, TestCaseSource(nameof(GridTestData))]
        public void MoveForwardTest((int, int) expectedPosition)
        {
            gridMock.Setup(g => g.WrapAround(It.IsAny<(int, int)>(), It.IsAny<(int, int)>()))
                .Returns(expectedPosition);
            rover = new Rover(gridMock.Object);
            rover.Position = (0, 0);
            rover.Direction = 'N';
            Assert.DoesNotThrow(() => rover.MoveForward());
            Assert.AreEqual(expectedPosition, rover.Position);
        }

        [Test, TestCaseSource(nameof(GridTestData))]
        public void MoveBackwardTest((int, int) expectedPosition)
        {
            gridMock.Setup(g => g.WrapAround(It.IsAny<(int, int)>(), It.IsAny<(int, int)>()))
                .Returns(expectedPosition);
            rover = new Rover(gridMock.Object);
            rover.Position = (0, 0);
            rover.Direction = 'N';
            Assert.DoesNotThrow(() => rover.MoveBackward());
            Assert.AreEqual(expectedPosition, rover.Position);
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

        [Test]
        public void CatchingObstacleExceptionOnMovingForwardTest()
        {
            gridMock.Setup(g => g.WrapAround(It.IsAny<(int, int)>(), It.IsAny<(int, int)>()))
                .Throws<ObstacleException>();
            rover = new Rover(gridMock.Object);
            rover.Direction = 'N';
            rover.Position = (0, 0);
            Assert.Throws<ObstacleException>(() => rover.MoveForward());
            Assert.AreEqual((0, 0), rover.Position);
        }

        [Test]
        public void CatchingObstacleExceptionOnMovingBackwardTest()
        {
            gridMock.Setup(g => g.WrapAround(It.IsAny<(int, int)>(), It.IsAny<(int, int)>()))
                .Throws<ObstacleException>();
            rover = new Rover(gridMock.Object);
            rover.Direction = 'N';
            rover.Position = (0, 0);
            Assert.Throws<ObstacleException>(() => rover.MoveBackward());
            Assert.AreEqual((0, 0), rover.Position);
        }

        [Test]
        public void MovingForwardObstacleTest()
        {
            var grid = new Grid(3, 3);
            grid.AddObstacle((0, 1));
            rover = new Rover(grid);
            rover.Direction = 'N';
            rover.Position = (0, 0);
            Assert.Throws<ObstacleException>(() => rover.MoveForward());
            Assert.AreEqual((0, 0), rover.Position);
        }

        [Test]
        public void MovingBackwardObstacleTest()
        {
            var grid = new Grid(3, 3);
            grid.AddObstacle((0, 0));
            rover = new Rover(grid);
            rover.Direction = 'N';
            rover.Position = (0, 1);
            Assert.Throws<ObstacleException>(() => rover.MoveBackward());
            Assert.AreEqual((0, 1), rover.Position);
        }

        [Test, TestCaseSource(nameof(SequenceTestData))]
        public void ProcessSequenceTest((int, int) startingPosition, char startingDirection, char[] sequence, char[] expectedDirections, (int, int)[] expectedPositions)
        {
            var grid = new Grid(3, 3);
            rover = new Rover(grid)
            {
                Direction = startingDirection,
                Position = startingPosition
            };

            for (var i = 0; i < sequence.Length; i++)
            {
                rover.ExecuteCommand(sequence[i]);
                Assert.AreEqual(expectedDirections[i], rover.Direction);
                Assert.AreEqual(expectedPositions[i], rover.Position);
            }
        }

        [Test]
        public void ProcessSequence_ApprovalTest()
        {
            StringBuilder fakeoutput = new StringBuilder();
            Console.SetOut(new StringWriter(fakeoutput));
            Console.SetIn(new StringReader("a\n"));

            rover = new Rover(new Grid(3, 3))
            {
                Direction = 'S',
                Position = (1,1)
            };
            rover.ExecuteCommandSequence(new[] { 'f', 'l', 'f', 'l', 'f', 'l', 'f' });

            var output = fakeoutput.ToString();
            Approvals.Verify(output);
        }

        [Test]
        public void ProcessSequenceWithObstacle_ApprovalTest()
        {
            StringBuilder fakeoutput = new StringBuilder();
            Console.SetOut(new StringWriter(fakeoutput));
            Console.SetIn(new StringReader("a\n"));

            var grid = new Grid(3, 3);
            grid.AddObstacle((2, 1));
            rover = new Rover(grid)
            {
                Direction = 'S',
                Position = (1, 1)
            };

            rover.ExecuteCommandSequence(new[] { 'f', 'l', 'f', 'l', 'f', 'l', 'f' });

            var output = fakeoutput.ToString();
            Approvals.Verify(output);
        }
    }
}