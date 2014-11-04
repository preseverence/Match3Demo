using System;

namespace BrokenEvent.Match3
{
  /// <summary>
  /// Used to merge a set of tiles properties. Inherit to merge a custom tiles
  /// </summary>
  class Match3Merge
  {
    private int tileColor = -1;

    /// <summary>
    /// Add tiles to merge
    /// </summary>
    /// <param name="tile">Tile to add</param>
    public virtual void Add(Match3Tile tile)
    {
      if (tileColor != -1 && tileColor != tile.TileColor)
        throw new Exception("Unsupported merge!");
      tileColor = tile.TileColor;
    }

    /// <summary>
    /// The color of merged tiles
    /// </summary>
    public int TileColor
    {
      get { return tileColor; }
    }

    /// <summary>
    /// Clears merge for reuse
    /// </summary>
    public virtual void Clear()
    {
      tileColor = -1;
    }
  }
}
