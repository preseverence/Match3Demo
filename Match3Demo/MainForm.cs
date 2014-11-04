using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BrokenEvent.Match3;
using BrokenEvent.Match3Demo.Properties;

namespace BrokenEvent.Match3Demo
{
  public partial class MainForm : Form
  {
    private Match3Board board;
    private Match3AI ai;
    private Color[] tilesColors = new Color[]{Color.Red, Color.Blue, Color.Green, Color.Gold, Color.DarkViolet};
    private Brush[] tilesBrushes;
    private PlayerInfo player;
    private PlayerInfo enemy;

    private const int BOARD_WIDTH = 300;
    private const int BOARD_HEIGHT = 300;
    private const int BOARD_OFFSET_X = 122;
    private const int BOARD_OFFSET_Y = 10;

    private Point selected = emptyPoint;
    private static readonly Point emptyPoint = new Point(-1, -1);

    private class PlayerInfo
    {
      private const int MAX_HP = 1000;
      private float hp = MAX_HP;
      private Meter progressBar;
      private Label label;

      public PlayerInfo(Meter progressBar, Label label)
      {
        this.progressBar = progressBar;
        this.label = label;
        progressBar.Value = (int)hp;
        label.Text = hp + "/" + MAX_HP;
      }

      public float Hp
      {
        get { return hp; }
        set
        {
          hp = value;
          if (hp < 0)
            hp = 0;
          if (hp > MAX_HP)
            hp = MAX_HP;

          progressBar.Value = hp/MAX_HP;
          label.Text = hp + "/" + MAX_HP;
        }
      }

      public bool IsDead
      {
        get { return hp < 0.0001f; }
      }
    }

    private enum State
    {
      Idle,
      MakingTurn,
      Collapsing,
      Falling,
    }

    private State state = State.Idle;
    private bool isAITurn;

    private class AnimatedTile
    {
      private int x;
      private int y;
      private Match3Tile tile;
      private float stateValue;

      public AnimatedTile(int x, int y, Match3Tile tile)
      {
        this.x = x;
        this.y = y;
        this.tile = tile;
        stateValue = 0;
      }

      public float StateValue
      {
        get { return stateValue; }
        set
        {
          stateValue = value;
          if (stateValue > 1f)
            stateValue = 1f;
        }
      }

      public int X
      {
        get { return x; }
      }

      public int Y
      {
        get { return y; }
      }

      public Match3Tile Tile
      {
        get { return tile; }
      }
    }

    private class MovingTile: AnimatedTile
    {
      private int targetX;
      private int targetY;

      public MovingTile(int x, int y, Match3Tile tile, int targetX, int targetY) : base(x, y, tile)
      {
        this.targetX = targetX;
        this.targetY = targetY;
      }

      public int TargetX
      {
        get { return targetX; }
      }

      public int TargetY
      {
        get { return targetY; }
      }
    }

    private List<AnimatedTile> fadingTiles = new List<AnimatedTile>();
    private List<MovingTile> movingTiles = new List<MovingTile>();

    public MainForm()
    {
      InitializeComponent();
      board = new Match3Board(8, 8, 5);
      ai = new Match3AI(board);

      tilesBrushes = new Brush[tilesColors.Length];
      for (int i = 0; i < tilesColors.Length; i++)
        tilesBrushes[i] = new SolidBrush(tilesColors[i]);

      board.OnTileCollapsed += BoardOnOnTileCollapsed;
      board.OnTileMoved += BoardOnOnTileMoved;

      Stopwatch sw = new Stopwatch();
      sw.Start();

      board.Generate();

      sw.Stop();

      slblComment.Text = "Board generated in " + sw.ElapsedMilliseconds + " ms";

      player = new PlayerInfo(mPlayer, lblPlayerHP);
      enemy = new PlayerInfo(mEnemy, lblEnemyHP);

      Icon = Resources.BrokenEvent;
    }

    private void BoardOnOnTileMoved(int x1, int y1, int x2, int y2, Match3Tile tile)
    {
      if (!tmAnimation.Enabled)
        return;

      movingTiles.Add(new MovingTile(x1, y1, tile, x2, y2));
      tile.Visible = false;
    }

    private void BoardOnOnTileCollapsed(int x, int y, Match3Tile tile)
    {
      if (!tmAnimation.Enabled)
        return;

      fadingTiles.Add(new AnimatedTile(x, y, tile));
      tile.Visible = false;
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      float dx = (float)BOARD_WIDTH / board.Width;
      float dy = (float)BOARD_HEIGHT / board.Height;

      float marginX = dx * 0.1f;
      float marginY = dy * 0.1f;

      using (Pen pen = new Pen(Color.Black))
      {
        float i = BOARD_OFFSET_X;
        for (; i < BOARD_WIDTH; i += dx)
          e.Graphics.DrawLine(pen, i, BOARD_OFFSET_Y, i, BOARD_OFFSET_Y + BOARD_HEIGHT);

        i = BOARD_OFFSET_Y;
        for (; i < BOARD_WIDTH; i += dy)
          e.Graphics.DrawLine(pen, BOARD_OFFSET_X, i, BOARD_OFFSET_X + BOARD_WIDTH, i);

        e.Graphics.DrawLine(pen, BOARD_OFFSET_X + BOARD_WIDTH, BOARD_OFFSET_Y, BOARD_OFFSET_X + BOARD_WIDTH, BOARD_OFFSET_Y + BOARD_HEIGHT);
        e.Graphics.DrawLine(pen, BOARD_OFFSET_X, BOARD_OFFSET_Y + BOARD_HEIGHT, BOARD_OFFSET_X + BOARD_WIDTH, BOARD_OFFSET_Y + BOARD_HEIGHT);
      }

      for (int x = 0; x < board.Width; x++)
        for (int y = 0; y < board.Height; y++)
        {
          Match3Tile tile = board[x, y];
          if (tile != null && tile.Visible)
            e.Graphics.FillRectangle(tilesBrushes[tile.TileColor], BOARD_OFFSET_X + x * dx + marginX, BOARD_OFFSET_Y + y * dy + marginY, dx - marginX * 2, dy - marginY * 2);
        }

      if (selected != emptyPoint)
        using (Pen pen = new Pen(Color.Red))
          e.Graphics.DrawRectangle(pen, BOARD_OFFSET_X + selected.X * dx, BOARD_OFFSET_Y + selected.Y * dy, dx, dy);

      foreach (AnimatedTile tile in fadingTiles)
      {
        using (Brush brush = new SolidBrush(Color.FromArgb((int)((1f - tile.StateValue) * 255), tilesColors[tile.Tile.TileColor])))
          e.Graphics.FillRectangle(brush, BOARD_OFFSET_X + tile.X * dx + marginX, BOARD_OFFSET_Y + tile.Y * dy + marginY, dx - marginX * 2, dy - marginY * 2);
      }

      foreach (MovingTile tile in movingTiles)
      {
        float x = Lerp(tile.X, tile.TargetX, tile.StateValue);
        float y = Lerp(tile.Y, tile.TargetY, tile.StateValue);

        e.Graphics.FillRectangle(tilesBrushes[tile.Tile.TileColor], BOARD_OFFSET_X + x * dx + marginX, BOARD_OFFSET_Y + y * dy + marginY, dx - marginX * 2, dy - marginY * 2);
      }
    }

    private static float Lerp(int start, int end, float k)
    {
      return start * (1f - k) + end * k;
    }

    private void MakeTurn(int x, int y)
    {
      board.DoExchange(selected.X, selected.Y, x, y);
      state = State.MakingTurn;
    }

    private bool OnCollected(int count, Match3Merge merge)
    {
      float effect = 100;

      if (merge.TileColor == 2)
      {
        effect = 50;

        CalculateEffect(ref effect, count);
        if (isAITurn)
          enemy.Hp += effect;
        else
          player.Hp += effect;
      }
      else
      {
        CalculateEffect(ref effect, count);
        if (isAITurn)
          player.Hp -= effect;
        else
          enemy.Hp -= effect;      
      }

      return true;
    }

    private static void CalculateEffect(ref float effect, int count)
    {
      while (count > 3)
      {
        effect *= 1.5f;
        count--;
      }   
    }

    public bool CheckVictory()
    {
      if (player.IsDead)
      {
        tmAnimation.Enabled = false;
        MessageBox.Show(this, "Player loses");
        Close();
        return true;
      }

      if (enemy.IsDead)
      {
        tmAnimation.Enabled = false;
        MessageBox.Show(this, "Player wins");
        Close();
        return true;
      }

      return false;
    }

    private void MainForm_MouseDown(object sender, MouseEventArgs e)
    {
      if (state != State.Idle || isAITurn)
        return;

      float dx = (float)BOARD_WIDTH / board.Width;
      float dy = (float)BOARD_HEIGHT / board.Height;

      int x = (int)((e.X - BOARD_OFFSET_X) / dx);
      int y = (int)((e.Y - BOARD_OFFSET_Y) / dy);

      if (x >= board.Width || y >= board.Height || x < 0 || y < 0)
        return;

      if (selected == emptyPoint)
      {
        selected = new Point(x, y);
        Invalidate();
      }
      else
      {
        if (selected.X == x && selected.Y == y)
        {
          selected = emptyPoint;
          Invalidate();
          return;
        }

        if (board.CanExchange(selected.X, selected.Y, x, y, null))
          MakeTurn(x, y);
        else
          slblComment.Text = "Unable to make turn";

        selected = emptyPoint;
        Invalidate();
      }
    }

    private void tmAnimation_Tick(object sender, EventArgs e)
    {
      const float dt = 0.13f;
      int i = 0;
      bool hasChanges = false;

      while (i < fadingTiles.Count)
      {
        if (fadingTiles[i].StateValue >= 0.999f)
        {
          fadingTiles.RemoveAt(i);
          continue;
        }

        fadingTiles[i].StateValue += dt;
        hasChanges = true;
        i++;
      }

      i = 0;
      while (i < movingTiles.Count)
      {
        if (movingTiles[i].StateValue >= 0.999f)
        {
          movingTiles[i].Tile.Visible = true;
          movingTiles.RemoveAt(i);
          continue;
        }

        movingTiles[i].StateValue += dt;
        hasChanges = true;
        i++;
      }

      if (hasChanges)
        Invalidate();

      if (movingTiles.Count > 0 || fadingTiles.Count > 0)
        return;

      switch (state)
      {
        case State.MakingTurn:
          board.SolveBoard(OnCollected);
          state = State.Collapsing;
          CheckVictory();
          break;
        case State.Collapsing:
          if (board.MakeFall())
            state = State.Falling;
          else
          {
            if (CheckVictory())
              return;

            if (!board.CheckBoardHasTurns())
            {
              board.Generate();
              Invalidate();
            }

            if (isAITurn)
            {
              state = State.Idle;
              isAITurn = false;
            }
            else
            {
              isAITurn = true;
              AITurn();
            }
          }
          break;
        case State.Falling:
          board.SolveBoard(OnCollected);
          if (CheckVictory())
            return;
          state = State.Collapsing;
          break;
      }
    }

    private void AITurn()
    {
      Point start, end;
      Stopwatch sw = new Stopwatch();
      sw.Start();

      ai.MakeTurn(out start, out end);

      sw.Stop();

      slblComment.Text = "AI turns in " + sw.ElapsedMilliseconds + " ms";

      board.DoExchange(start.X, start.Y, end.X, end.Y);
      state = State.MakingTurn;
    }
  }
}
