using Maze.Logic.Models;
using Maze.Logic.Services;

namespace Maze.Tests
{
    [TestClass]
    public class UnitTests
    {
        private readonly IMazeService _mazeService;
        private readonly Room[,] _maze;

        public UnitTests()
        {
            _mazeService = new MazeService();
            _mazeService.BuildMaze(3);
        }

        [TestMethod]
        public void VerifyThatEntranceIsAtABorder()
        {
            var idOfEntranceRoom = _mazeService.GetEntranceRoom();
            var validIds = new int[] { 0, 1, 2, 3, 5, 6, 7, 8 };
            var isAtABorder = false;
            foreach (var validId in validIds)
            {
                if(validId == idOfEntranceRoom)
                {
                    isAtABorder = true;
                };
            }
            Assert.IsTrue(isAtABorder);
        }

        [TestMethod]
        public void TestEntranceAndDescriptionMethod()
        {
            var idOfEntranceRoom=_mazeService.GetEntranceRoom();
            var entranceRoomDescription = _mazeService.GetDescription(idOfEntranceRoom);

            Assert.AreEqual(entranceRoomDescription, RoomType.Entrance.ToString());
        }

        [TestMethod]
        public void VerifyThatMazeContainOneTreasure()
        {
            int numberOfTreasures = 0;
            for (int i = 0; i <= 8; i++)
            {
                if(_mazeService.HasTreasure(i))
                {
                    numberOfTreasures = 1;
                }
            }
            Assert.IsTrue(numberOfTreasures==1);
        }

        [TestMethod]
        public void TestGetRoom()
        {
            var roomId = _mazeService.GetRoom(0, 'E');

            Assert.IsTrue(roomId == 1);
        }

        [TestMethod]
        public void TestPassBorder()
        {
            var roomId = _mazeService.GetRoom(0, 'S');

            Assert.IsTrue(roomId == null);
        }

        [TestMethod]
        public void TestGetRoomInvalidId()
        {
            var roomId = _mazeService.GetRoom(10, 'E');

            Assert.IsTrue(roomId == null);
        }
    }
}