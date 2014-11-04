namespace BrokenEvent.Match3
{
  /// <summary>
  /// Match3 tile base class. Inherit (and made a <see cref="Match3Board.OnTileCreate"/> handler) to extend the base tile functionality
  /// </summary>
  class Match3Tile
  {
    private int tileColor;
    private bool visible;
    private bool toDelete;

    /// <summary>
    /// If the tile is visible. Tile may be made invisible when it animates or something
    /// </summary>
    public bool Visible
    {
      get { return visible; }
      set { visible = value; }
    }

    /// <summary>
    /// Creates the instance with given color
    /// </summary>
    /// <param name="tileColor">Tile color</param>
    public Match3Tile(int tileColor)
    {
      this.tileColor = tileColor;
      visible = true;
    }

    /// <summary>
    /// The tile color
    /// </summary>
    public int TileColor
    {
      get { return tileColor; }
    }

    /// <summary>
    /// Creates <see cref="Match3Merge"/> instance to merge tiles properties
    /// </summary>
    /// <returns>Newly created merger instance</returns>
    public virtual Match3Merge CreateMerge()
    {
      return new Match3Merge();
    }

    /// <summary>
    /// Is tile sholud be deleted. Temporary flag is used by <see cref="Match3Board"/> engine.
    /// </summary>
    public bool ToDelete
    {
      get { return toDelete; }
      set { toDelete = value; }
    }
  }
}
