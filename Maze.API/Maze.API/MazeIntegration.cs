using Maze.Logic.Services;

namespace Maze.Api
{
    public class MazeIntegration : IMazeIntegration
    {
        private readonly IMazeService _mazeService;
        public MazeIntegration()
        {
            _mazeService=new MazeService();
        }

        public void BuildMaze(int size)
        {
            _mazeService.BuildMaze(size);
        }

        public bool CausesInjury(int roomId)
        {
            return _mazeService.CauseInjury(roomId);
        }

        public string GetDescription(int roomId)
        {
            return _mazeService.GetDescription(roomId);
        }

        public int GetEntranceRoom()
        {
            return _mazeService.GetEntranceRoom();
        }

        public int? GetRoom(int roomId, char direction)
        {
            return _mazeService.GetRoom(roomId, direction);
        }

        public bool HasTreasure(int roomId)
        {
            return _mazeService.HasTreasure(roomId);
        }
    }
}
