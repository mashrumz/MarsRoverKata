using MarsRover;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarsRoverTest
{
    [TestFixture]
    public class GridTest
    {
        private static IEnumerable<TestCaseData> ObstacleTestData
        {
            get
            {
                yield return new TestCaseData((0,1), (1, 2));
                yield return new TestCaseData((1, 0), (2, 1));
                yield return new TestCaseData((-1, 0), (0, 1));
                yield return new TestCaseData((0, -1), (1, 0));
            }
        }

        Grid grid;

        [SetUp]
        public void SetUp()
        {
            grid = new Grid(3, 3);
        }

        [Test]
        public void PositiveDelta_WrapAroundHorizontalTest()
        {
            var position = grid.WrapAround((2, 0), (1, 0));
            Assert.AreEqual((0, 0), position);
        }

        [Test]
        public void PositiveDelta_WrapAroundVerticalTest()
        {
            var position = grid.WrapAround((0, 2), (0, 1));
            Assert.AreEqual((0, 0), position);
        }

        [Test]
        public void PositiveDelta_WrapAroundDiagonalTest()
        {
            var position = grid.WrapAround((2, 2), (1, 1));
            Assert.AreEqual((0, 0), position);
        }

        [Test]
        public void NegativeDelta_WrapAroundHorizontalTest()
        {
            var position = grid.WrapAround((0, 0), (-1, 0));
            Assert.AreEqual((2, 0), position);
        }

        [Test]
        public void NegativeDelta_WrapAroundVerticalTest()
        {
            var position = grid.WrapAround((0, 0), (0, -1));
            Assert.AreEqual((0, 2), position);
        }

        [Test]
        public void NegativeDelta_WrapAroundDiagonalTest()
        {
            var position = grid.WrapAround((0, 0), (-1, -1));
            Assert.AreEqual((2, 2), position);
        }

        [Test, TestCaseSource(nameof(ObstacleTestData))]
        public void ThrowsObstacleExceptionTest((int, int) delta, (int, int) expectedPosition)
        {
            grid.AddObstacle(expectedPosition);
            var ex = Assert.Throws<ObstacleException>(() => grid.WrapAround((1, 1), delta));
            Assert.AreEqual(expectedPosition, ex.Position);
        }
    }
}
