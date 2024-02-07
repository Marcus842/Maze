using Maze.Logic.Models;

namespace Maze.Logic.Services
{
    public class MazeService : IMazeService
    {
        private Random _rnd = new Random();

        private Room[,] _maze;
        private int _mazeSize = 0;
        private int _numberOfCells = 0;
        private int _xEntrance = 0;
        private int _yEntrance = 0;
        private int _entranceId = 0;
        private int _treasureId = 0;

        public void BuildMaze(int size)
        {
            if (size < 2)
            {
                throw new ArgumentException("size needs to be exceed 2 to build valid maze");
            }
            _maze = new Room[size, size];

            _mazeSize = size;
            _numberOfCells = _mazeSize * _mazeSize;

            SetEntrance(size);
            SetTreasure(size);
            SetRooms(size);
        }

        public int GetEntranceRoom() => _entranceId;

        public string GetDescription(int roomId)
        {
            if (!RoomIdIsValid(roomId))
            {
                throw new ArgumentException("room id is not valid");
            }

            var room = GetRoom(roomId);

            return room.Description;
        }

        public bool HasTreasure(int roomId)
        {
            if (!RoomIdIsValid(roomId))
            {
                return false;
            }

            return roomId == _treasureId;
        }

        public int? GetRoom(int roomId, char direction)
        {
            int? returnId = null;

            if (!RoomIdIsValid(roomId))
            {
                return returnId;
            }

            var y = GetYCoordinateFromRoomId(roomId);
            var x = GetXCoordinateFromRoomId(roomId);

            switch (direction)
            {
                case 'N':
                    if (y < (_mazeSize - 1))
                    {
                        returnId = roomId + _mazeSize;
                    }
                    break;
                case 'S':
                    if (y != 0)
                    {
                        returnId = roomId - _mazeSize;
                    }
                    break;
                case 'W':
                    if (x != 0)
                    {
                        returnId = roomId - 1;
                    }
                    break;
                case 'E':
                    if (x != (_mazeSize - 1))
                    {
                        returnId = roomId + 1;
                    }
                    break;
            }

            return returnId;
        }

        public bool CauseInjury(int roomId)
        {
            if (!RoomIdIsValid(roomId))
            {
                return false;
            }

            var room = GetRoom(roomId);

            var isHarmed = PlayerHarmed(room);

            return isHarmed;
        }

        private Room GetRoom(int roomId)
        {
            var y = GetYCoordinateFromRoomId(roomId);
            var x = GetXCoordinateFromRoomId(roomId);
            var room = _maze[x, y];
            return room;
        }

        private bool PlayerHarmed(Room room)
        {
            if (!Enum.TryParse(room.Description, out RoomType eDescription))
            {
                return false;
            }
            var isHarmed = false;

            var injury = "";
            switch (eDescription)
            {
                case RoomType.Marsh:
                    UpdateInjuryAndHarmedBool("player sank", 30, ref isHarmed, ref injury);
                    break;
                case RoomType.Desert:
                    UpdateInjuryAndHarmedBool("player got dehydrated", 20, ref isHarmed, ref injury);
                    break;
                case RoomType.Forest:
                    UpdateInjuryAndHarmedBool("player got lost and starved", 50, ref isHarmed, ref injury);
                    break;
                case RoomType.Hills:
                    UpdateInjuryAndHarmedBool("player fell down", 20,ref isHarmed, ref injury);
                    break;
                default: isHarmed = false; break;
            }

            if (room.Description.Contains(injury))
            {
                room.Description = room.Description + " " + injury;
            }
            return isHarmed;
        }

        private void UpdateInjuryAndHarmedBool(string typeOfInjury, int probability, ref bool isHarmed, ref string injury)
        {
            if (TrapCausesHarm(probability))
            {
                injury = typeOfInjury;
                isHarmed = true;
            }
        }

        private bool RoomIdIsValid(int roomId)
        {
            if (roomId < 0 || roomId>= _numberOfCells)
            {
                return false;
            }
            return true;
        }

        private bool IsTrapActivated(int probabilityForActivatingTrap)
        {
            int chance = _rnd.Next(1, 101);
            if (chance <= probabilityForActivatingTrap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetRooms(int size)
        {
            var values = GetValidRoomTypesToSet();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (_maze[x, y]?.Description == null)
                    {
                        var enumIndex = _rnd.Next(values.Count);
                        var roomType = values[enumIndex];

                        _maze[x, y] = new Room();
                        _maze[x, y].Id = CreateCellIdBasedOnCoordinates(y, x);

                        _maze[x, y].Description = roomType.ToString();
                    }
                }
            }
        }

        public List<RoomType> GetValidRoomTypesToSet()
        {
            IEnumerable<RoomType> query = Enum.GetValues(typeof(RoomType)).Cast<RoomType>()
                            .Where(r => r != RoomType.Entrance
                                && r != RoomType.Treasure);
            return query.ToList();
        }

        private bool TrapCausesHarm(int probability)
        {

            if (IsTrapActivated(probability))
            {
                return true;
            }
            return false;
        }

        private void SetEntrance(int size)
        {
            int maxIndex = size - 1;
            _yEntrance = _rnd.Next(0, size);
            if (_yEntrance > 0 && _yEntrance < maxIndex)
            {
                _xEntrance = _rnd.Next(0, 2) * maxIndex;
            }
            else
            {
                _xEntrance = _rnd.Next(0, size);
            }
            _entranceId = CreateCellIdBasedOnCoordinates(_yEntrance, _xEntrance);

            _maze[_xEntrance, _yEntrance] = new Room() { Description = RoomType.Entrance.ToString(), Id = _entranceId };
        }

        private void SetTreasure(int size)
        {
            int maxIndex = size - 1;

            int xTreasure = GetRandomCoordinateForEntrance(size, _xEntrance, maxIndex);

            int yTreasure = GetRandomCoordinateForEntrance(size, _yEntrance, maxIndex);

            _treasureId = CreateCellIdBasedOnCoordinates(yTreasure, xTreasure);

            _maze[xTreasure, yTreasure] = new Room() { Description = RoomType.Treasure.ToString(), Id = _treasureId };
        }

        private int GetRandomCoordinateForEntrance(int size, int entrance_coordinate, int maxIndex)
        {
            int treasure_coordinate;

            if (entrance_coordinate == 0)
            {
                treasure_coordinate = _rnd.Next(1, size);
            }
            else if (entrance_coordinate == maxIndex)
            {
                treasure_coordinate = _rnd.Next(0, maxIndex);
            }
            else
            {
                treasure_coordinate = _rnd.Next(0, size);
            }
            return treasure_coordinate;
        }


        private int GetXCoordinateFromRoomId(int roomId)
        {
            return roomId % _mazeSize;
        }

        private int GetYCoordinateFromRoomId(int roomId)
        {
            return roomId / _mazeSize;
        }

        private int CreateCellIdBasedOnCoordinates(int y, int x)
        {
            var id = (y * _mazeSize) + x;
            return id;
        }
    }
}
