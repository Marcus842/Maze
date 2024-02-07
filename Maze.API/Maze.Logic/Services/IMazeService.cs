using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Logic.Services
{
    public interface IMazeService
    {
        public void BuildMaze(int size);

        public int GetEntranceRoom();

        public string GetDescription(int roomId);

        public bool HasTreasure(int roomId);

        public int? GetRoom(int roomId, char direction);

        public bool CauseInjury(int roomId);
    }
}
