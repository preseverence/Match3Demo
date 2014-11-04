using System;

namespace BrokenEvent.Match3
{
  class Match3Board
  {
    private readonly Match3Tile[,] board;
    private readonly int width;
    private readonly int height;
    private readonly int maxTile;
    private Random random = new Random();

    private const int MIN_TO_COLLECT = 3;

    /// <summary>
    /// Creates instance of the match 3 board core for a one game
    /// </summary>
    /// <param name="width">Width of the board in tiles</param>
    /// <param name="height">Height of the board in tiles</param>
    /// <param name="maxTile">Count of tiles colors</param>
    public Match3Board(int width, int height, int maxTile)
    {
      this.width = width;
      this.height = height;
      this.maxTile = maxTile;
      board = new Match3Tile[width, height];
    }

    /// <summary>
    /// <see cref="Match3Board.GetCollectedCount"/> search direction
    /// </summary>
    public enum Direction
    {
      Left,
      Right,
      Up,
      Down
    }

    /// <summary>
    /// Access to tiles placed on field
    /// </summary>
    /// <param name="x">X-coordinate of the tile</param>
    /// <param name="y">Y-coordinate of the tile</param>
    /// <returns>Tile at given coordinates or null if there is empty</returns>
    public Match3Tile this[int x, int y]
    {
      get { return board[x, y]; }
    }

    /// <summary>
    /// Width of the board in tiles
    /// </summary>
    public int Width
    {
      get { return width; }
    }
    /// <summary>
    /// Height of the board in tiles
    /// </summary>
    public int Height
    {
      get { return height; }
    }

    /// <summary>
    /// Generate the initial tiles. Should be called before game is started
    /// </summary>
    public void Generate()
    {
      ClearBoard();

      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
          int tile;
          do
          {
            tile = random.Next(maxTile);
          }
          while (!CanPlaceOnGenerate(x, y, tile));

          board[x, y] = onTileCreated != null ? onTileCreated(tile) : new Match3Tile(tile);
        }
    }

    /// <summary>
    /// Fills the board with nulls
    /// </summary>
    private void ClearBoard()
    {
      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
          board[x, y] = null;
    }

    /// <summary>
    /// Checks if the tile of given color at given coordinates will not form triples with the neighbours.
    /// </summary>
    /// <param name="x">X-coordinate of the tile</param>
    /// <param name="y">Y-coordinate of the tile</param>
    /// <param name="tile">Tile color</param>
    /// <returns>True if tile can be placed, false if not</returns>
    private bool CanPlaceOnGenerate(int x, int y, int tile)
    {
      if (GetCollectedCount(x, y, tile, Direction.Left, null) >= MIN_TO_COLLECT)
        return false;
      if (GetCollectedCount(x, y, tile, Direction.Right, null) >= MIN_TO_COLLECT)
        return false;
      if (GetCollectedCount(x, y, tile, Direction.Up, null) >= MIN_TO_COLLECT)
        return false;
      if (GetCollectedCount(x, y, tile, Direction.Down, null) >= MIN_TO_COLLECT)
        return false;
      return true;
    }

    /// <summary>
    /// Checks the exchange coordinates to be at neightbour points
    /// </summary>
    /// <param name="x1">X-coordinate of the first exchange point</param>
    /// <param name="y1">Y-coordinate of the first exchange point</param>
    /// <param name="x2">X-coordinate of the second exchange point</param>
    /// <param name="y2">Y-coordinate of the second exchange point</param>
    /// <returns>True if coords is ok and false otherwise</returns>
    private static bool IsExchangeCorrect(int x1, int y1, int x2, int y2)
    {
      int dx = x2 - x1;
      if (dx > 1 || dx < -1)
        return false;
      int dy = y2 - y1;
      if (dy > 1 || dy < -1)
        return false;

      return true;
    }

    /// <summary>
    /// Checks if the tiles after exchange will form triples (or more tiles figures)
    /// </summary>
    /// <param name="x1">X-coordinate of the first exchange point</param>
    /// <param name="y1">Y-coordinate of the first exchange point</param>
    /// <param name="x2">X-coordinate of the second exchange point</param>
    /// <param name="y2">Y-coordinate of the second exchange point</param>
    /// <param name="onCollected">Be called on any successful combination after exchange. May be null if no callback required</param>
    /// <returns>True if tiles after exchange forms some figures and false otherwise</returns>
    public bool CanExchange(int x1, int y1, int x2, int y2, OnCollectedDelegate onCollected)
    {
      if (!IsExchangeCorrect(x1, y1, x2, y2))
        return false;

      // just do exchange
      Match3Tile last1 = board[x1, y1];
      Match3Tile last2 = board[x2, y2];

      board[x1, y1] = last2;
      board[x2, y2] = last1;

      // check 
      Match3Merge merge = board[x1, y1].CreateMerge();

      // first tile horizontal
      int count1 = GetCollectedCount(x1, y1, last2.TileColor, Direction.Left, merge);
      int count2 = GetCollectedCount(x1, y1, last2.TileColor, Direction.Right, merge);
      int count = count1 + count2 - 1;
      if (onCollected != null && count >= MIN_TO_COLLECT)
        onCollected(count, merge);

      // first tile vertical
      merge.Clear();
      if (count < MIN_TO_COLLECT || onCollected != null)
      {
        count1 = GetCollectedCount(x1, y1, last2.TileColor, Direction.Up, merge);
        count2 = GetCollectedCount(x1, y1, last2.TileColor, Direction.Down, merge);
        count = count1 + count2 - 1;
        if (onCollected != null && count >= MIN_TO_COLLECT)
          onCollected(count, merge);
      }

      // second tile horizontal
      merge.Clear();
      if (count < MIN_TO_COLLECT || onCollected != null)
      {
        count1 = GetCollectedCount(x2, y2, last1.TileColor, Direction.Left, merge);
        count2 = GetCollectedCount(x2, y2, last1.TileColor, Direction.Right, merge);
        count = count1 + count2 - 1;
        if (onCollected != null && count >= MIN_TO_COLLECT)
          onCollected(count, merge);
      }

      // second tile vertical
      merge.Clear();
      if (count < MIN_TO_COLLECT)
      {
        count1 = GetCollectedCount(x2, y2, last1.TileColor, Direction.Up, merge);
        count2 = GetCollectedCount(x2, y2, last1.TileColor, Direction.Down, merge);
        count = count1 + count2 - 1;
        if (onCollected != null && count >= MIN_TO_COLLECT)
          onCollected(count, merge);
      }

      // and rollback
      board[x1, y1] = last1;
      board[x2, y2] = last2;

      return count >= MIN_TO_COLLECT;
    }

    /// <summary>
    /// Gets the count of tiles of one color from given point in given direction
    /// </summary>
    /// <param name="x">X-coordinate of the tile</param>
    /// <param name="y">Y-coordinate of the tile</param>
    /// <param name="tile">Tile color</param>
    /// <param name="direction">Search direction</param>
    /// <param name="merge">Object to merge found tiles, may be null if no merge required</param>
    /// <returns>The count of tiles. At least one (given tile)</returns>
    private int GetCollectedCount(int x, int y, int tile, Direction direction, Match3Merge merge)
    {
      if (tile == -1)
        return 0;

      int collected = 1;
      int i;

      switch (direction)
      {
        case Direction.Left:
          i = x - 1;
          while (i >= 0 && i < x && board[i, y] != null && !board[i, y].ToDelete && board[i, y].TileColor == tile)
          {
            if (merge != null)
              merge.Add(board[i, y]);
            collected++;
            i--;
          }
          break;
        case Direction.Right:
          i = x + 1;
          while (i < width && i > x && board[i, y] != null && !board[i, y].ToDelete && board[i, y].TileColor == tile)
          {
            if (merge != null)
              merge.Add(board[i, y]);
            collected++;
            i++;
          }
          break;
        case Direction.Up:
          i = y - 1;
          while (i >= 0 && i < y && board[x, i] != null && !board[x, i].ToDelete && board[x, i].TileColor == tile)
          {
            if (merge != null)
              merge.Add(board[x, i]);
            collected++;
            i--;
          }
          break;
        case Direction.Down:
          i = y + 1;
          while (i < height && i > y && board[x, i] != null && !board[x, i].ToDelete && board[x, i].TileColor == tile)
          {
            if (merge != null)
              merge.Add(board[x, i]);
            collected++;
            i++;
          }
          break;
      }

      return collected;
    }

    /// <summary>
    /// Occurs when collectible found
    /// </summary>
    /// <param name="count">Count of collectible items</param>
    /// <param name="merge">Merge containing data for all collected tiles</param>
    /// <returns>True to collect (clear) found</returns>
    public delegate bool OnCollectedDelegate(int count, Match3Merge merge);

    /// <summary>
    /// Occurs when the tile collapses.
    /// </summary>
    /// <param name="x">X-coordinate of the tile</param>
    /// <param name="y">Y-coordinate of the tile</param>
    /// <param name="tile">The tile being collapsed</param>
    public delegate void OnTileCollapsedDelegate(int x, int y, Match3Tile tile);

    /// <summary>
    /// Occurs when the tile moves 
    /// </summary>
    /// <param name="x1">X-coordinate of moving start point</param>
    /// <param name="y1">Y-coordinate of moving start point</param>
    /// <param name="x2">X-coordinate of moving finish point</param>
    /// <param name="y2">Y-coordinate of moving finish point</param>
    /// <param name="tile">The tile being moved</param>
    public delegate void OnTileMovedDelegate(int x1, int y1, int x2, int y2, Match3Tile tile);

    /// <summary>
    /// Occurs when tile is being created
    /// </summary>
    /// <param name="tileColor">The color of tile to be created</param>
    /// <returns>Created tile of given color</returns>
    public delegate Match3Tile OnTileCreatedDelegate(int tileColor);

    private OnTileCollapsedDelegate onTileCollapsed;
    private OnTileMovedDelegate onTileMoved;
    private OnTileCreatedDelegate onTileCreated;

    /// <summary>
    /// Occurs when the tile collapses (just before nullize it in board). Can be used to start animation and etc
    /// </summary>
    public event OnTileCollapsedDelegate OnTileCollapsed
    {
      add { onTileCollapsed += value; }
      remove { onTileCollapsed -= value; }
    }
    /// <summary>
    /// Occurs when tile is moved (exchange action and fall to empty places)
    /// </summary>
    public event OnTileMovedDelegate OnTileMoved
    {
      add { onTileMoved += value; }
      remove { onTileMoved -= value; }
    }
    /// <summary>
    /// Occurs when tile is being created. Used to created custom tile objects (<see cref="Match3Tile"/> derived classes).
    /// Default implemetation (if this event is not set) creates <see cref="Match3Tile"/> instances.
    /// </summary>
    public event OnTileCreatedDelegate OnTileCreate
    {
      add { onTileCreated += value; }
      remove { onTileCreated -= value; }
    }

    /// <summary>
    /// Checks if the board have any possible exchange turns
    /// </summary>
    /// <returns>True if at least one turn found, else otherwise</returns>
    public bool CheckBoardHasTurns()
    {
      for (int x = 0; x < width - 1; x++)
        for (int y = 0; y < height - 1; y++)
        {
          if (CanExchange(x, y, x + 1, y, null) || CanExchange(x, y, x, y + 1, null))
            return true;
        }

      return false;
    }

    /// <summary>
    /// Tries to collect all possible combinations on board
    /// </summary>
    /// <param name="onCollected">Called when found some combination. If result is true (or <paramref name="onCollected"/> is null) found tiles will be cleared.</param>
    /// <param name="findOne">Stops when found the first collectible combination if true and scans the full board on false</param>
    /// <returns>True if found something and false otherwise</returns>
    public bool SolveBoard(OnCollectedDelegate onCollected, bool findOne = false)
    {
      bool result = false;

      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
          Match3Tile tile = board[x, y];
          if (tile == null)
            continue;
          Match3Merge merge = tile.CreateMerge();

          // horizontal
          int count1 = GetCollectedCount(x, y, tile.TileColor, Direction.Left, merge);
          int count2 = GetCollectedCount(x, y, tile.TileColor, Direction.Right, merge);
          int count = count1 + count2 - 1;

          if (count >= MIN_TO_COLLECT)
          {
            result = true;
            if (findOne)
              return true;

            bool doDelete = onCollected == null || onCollected(count, merge);

            if (doDelete)
              for (int i = x - count1 + 1; i < x + count2; i++)
                board[i, y].ToDelete = true;
          }

          // vertical
          merge.Clear();
          count1 = GetCollectedCount(x, y, tile.TileColor, Direction.Up, merge);
          count2 = GetCollectedCount(x, y, tile.TileColor, Direction.Down, merge);
          count = count1 + count2 - 1;

          if (count1 + count2 - 1 >= MIN_TO_COLLECT)
          {
            result = true;
            if (findOne)
              return true;

            bool doDelete = onCollected == null || onCollected(count, merge);

            if (doDelete)
              for (int i = y - count1 + 1; i < y + count2; i++)
                board[x, i].ToDelete = true;
          }
        }

      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
          if (board[x, y].ToDelete)
          {
            if (onTileCollapsed != null)
              onTileCollapsed(x, y, board[x, y]);
            board[x, y] = null;
          }

      return result;
    }   

    /// <summary>
    /// Makes the tiles fall into empty spaces and generates new if it is necessary.
    /// To process all empty places should be called in cycle until return false
    /// </summary>
    /// <returns>True if found some empty places and made at least one tile falling and false otherwise</returns>
    public bool MakeFall()
    {
      bool result = false;

      for (int y = height - 1; y >= 0; y--)
        for (int x = 0; x < width; x++)
          if (board[x, y] == null)
          {
            result = true;
            int i = y - 1;
            while (i >= 0 && board[x, i] == null)
              i--;

            // borning new tile
            if (i == -1)
            {
              int tile = random.Next(maxTile);
              board[x, y] = onTileCreated != null ? onTileCreated(tile) : new Match3Tile(tile);
              if (onTileMoved != null)
                onTileMoved(x, -1, x, y, board[x, y]);
            }
            else
            {
              // moving found tile down
              board[x, y] = board[x, i];
              board[x, i] = null;
              onTileMoved(x, i, x, y, board[x, y]);
            }
          }

      return result;
    }

    /// <summary>
    /// Does exchange tiles on the board. Will not process the board after exchange and collect something. Call <see cref="SolveBoard"/> to do this.
    /// </summary>
    /// <param name="x1">X-coordinate of the first exchange point</param>
    /// <param name="y1">Y-coordinate of the first exchange point</param>
    /// <param name="x2">X-coordinate of the second exchange point</param>
    /// <param name="y2">Y-coordinate of the second exchange point</param>
    public void DoExchange(int x1, int y1, int x2, int y2)
    {
      if (!IsExchangeCorrect(x1, y1, x1, y1))
        return;

      Match3Tile last = board[x1, y1];
      board[x1, y1] = board[x2, y2];
      board[x2, y2] = last;

      if (onTileMoved != null)
      {
        onTileMoved(x1, y1, x2, y2, board[x2, y2]);
        onTileMoved(x2, y2, x1, y1, board[x1, y1]);
      }
    }
  }
}
