using System;
using System.Collections.Generic;
using System.Drawing;

namespace BrokenEvent.Match3
{
  /// <summary>
  /// Match 3 AI
  /// </summary>
  class Match3AI
  {
    private Match3Board board;
    private Random random;

    /// <summary>
    /// Creates instance
    /// </summary>
    /// <param name="board">Board to play</param>
    public Match3AI(Match3Board board)
    {
      this.board = board;
      random = new Random();
    }

    private class PossibleTurn
    {
      private int x1;
      private int y1;
      private int x2;
      private int y2;
      private int count;

      public PossibleTurn(int x1, int y1, int x2, int y2, int count)
      {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.count = count;
      }

      public int X1
      {
        get { return x1; }
      }

      public int Y1
      {
        get { return y1; }
      }

      public int X2
      {
        get { return x2; }
      }

      public int Y2
      {
        get { return y2; }
      }

      public int Count
      {
        get { return count; }
        set { count = value; }
      }

      public override string ToString()
      {
        return x1 + "x" + y1 + "->" + x2 + "x" + y2 + ": " + count;
      }
    }

    private List<PossibleTurn> possibleTurns = new List<PossibleTurn>();
    private Point possibleTurn1, possibleTurn2;

    /// <summary>
    /// Makes a turn
    /// </summary>
    /// <param name="start">Start point of exchange made with turn</param>
    /// <param name="end">End point of exchange made with turn</param>
    public void MakeTurn(out Point start, out Point end)
    {
      possibleTurns.Clear();

      // try every possible combination and handle the callbacks
      for (int x = 0; x < board.Width - 1; x++)
        for (int y = 0; y < board.Height - 1; y++)
        {
          possibleTurn1.X = x;
          possibleTurn1.Y = y;
          board.CanExchange(x, y, possibleTurn2.X = x + 1, possibleTurn2.Y = y, OnPossibleTurnFound);
          board.CanExchange(x, y, possibleTurn2.X = x, possibleTurn2.Y = y + 1, OnPossibleTurnFound);
        }

      // find maximal effectiveness
      int max = 0;
      foreach (PossibleTurn turn in possibleTurns)
        if (max < turn.Count)
          max = turn.Count;

      // remove all turns with effectiveness less the maxumal
      int i = 0;
      while (i < possibleTurns.Count)
        if (possibleTurns[i].Count < max)
          possibleTurns.RemoveAt(i);
        else
          i++;

      // make random turn from all that remains
      i = random.Next(possibleTurns.Count);
      start = new Point(possibleTurns[i].X1, possibleTurns[i].Y1);
      end = new Point(possibleTurns[i].X2, possibleTurns[i].Y2);
    }

    /// <summary>
    /// <see cref="Match3Board.CanExchange"/> callback. Stores found turns
    /// </summary>
    /// <param name="count">Count of tiles</param>
    /// <param name="merge">Tile</param>
    /// <returns>Returns false not to collapse found tiles</returns>
    private bool OnPossibleTurnFound(int count, Match3Merge merge)
    {
      PossibleTurn foundTurn = null;
      foreach (PossibleTurn turn in possibleTurns)
      {
        if (turn.X1 == possibleTurn1.X && turn.Y1 == possibleTurn1.Y &&
            turn.X2 == possibleTurn2.X && turn.Y2 == possibleTurn2.Y)
        {
          foundTurn = turn;
          break;
        }
      }

      if (foundTurn != null)
        foundTurn.Count += count;
      else
        possibleTurns.Add(new PossibleTurn(possibleTurn1.X, possibleTurn1.Y, possibleTurn2.X, possibleTurn2.Y, count));

      return false;
    }
  }
}
