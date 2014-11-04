using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrokenEvent.Match3Demo
{
  class Meter: Control
  {
    private float value;

    private Color backColorTop;
    private Color backColorBottom;
    private Padding padding;
    private int distance;
    private int lineWidth;
    private bool invert;
    private MeterSegmentCollection segments;
    private bool vertical = true;

    public Meter()
    {
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.DoubleBuffer, true);

      segments = new MeterSegmentCollection(this);

      padding = new Padding(2);
      distance = 2;
      lineWidth = 2;
      backColorTop = Color.Gray;
      backColorBottom = Color.Black;
    }

    #region Public properties

    public float Value
    {
      get { return value; }
      set
      {
        this.value = value;
        Invalidate();
      }
    }

    public Color BackColorTop
    {
      get { return backColorTop; }
      set
      {
        backColorTop = value;
        Invalidate();
      }
    }

    public Color BackColorBottom
    {
      get { return backColorBottom; }
      set
      {
        backColorBottom = value;
        Invalidate();
      }
    }

    public Padding InnerPadding
    {
      get { return padding; }
      set
      {
        padding = value;
        Invalidate();
      }
    }

    public int Distance
    {
      get { return distance; }
      set
      {
        distance = value;

        Invalidate();
      }
    }

    public int LineWidth
    {
      get { return lineWidth; }
      set
      {
        if (lineWidth == value)
          return;

        lineWidth = value;
        foreach (MeterSegment segment in segments)
          segment.Owner = this; // useless operation, just to recreate pens

        Invalidate();
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public MeterSegmentCollection Segments
    {
      get { return segments; }
    }

    public bool Invert
    {
      get { return invert; }
      set
      {
        invert = value;
        Invalidate();
      }
    }

    public bool Vertical
    {
      get { return vertical; }
      set { vertical = value; }
    }

    #endregion

    private void PaintVertical(Graphics g)
    {
      int y = padding.Top + lineWidth / 2;
      int height = ClientSize.Height;

      int x1 = padding.Left;
      int x2 = ClientSize.Width - padding.Right;
      if (lineWidth == 1)
        x2--;

      while (y < height - padding.Bottom)
      {
        float v = (float)(height - y - padding.Top) / (height - padding.Top - padding.Bottom);
        MeterSegment segment = FindSegment(invert ? 1f - v : v);

        g.DrawLine(v <= value ? segment.ActivePen : segment.BackPen, x1, y, x2, y);
        y += distance + lineWidth;
      }     
    }

    private void PaintHorizontal(Graphics g)
    {
      int x = padding.Left + lineWidth / 2;
      int width = ClientSize.Width;

      int y1 = padding.Top;
      int y2 = ClientSize.Height - padding.Bottom;
      if (lineWidth == 1)
        y2--;

      while (x < width - padding.Right)
      {
        float v = (float)(x - padding.Left) / (width - padding.Left - padding.Right);
        MeterSegment segment = FindSegment(invert ? 1f - v : v);

        g.DrawLine(v <= value ? segment.ActivePen : segment.BackPen, x, y1, x, y2);
        x += distance + lineWidth;
      }
    }

    #region Overrides

    protected override void OnPaint(PaintEventArgs e)
    {
      if (segments.Count == 0)
        return;

      if (vertical)
        PaintVertical(e.Graphics);
      else
        PaintHorizontal(e.Graphics);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
      if (vertical)
        using (Brush brush = new LinearGradientBrush(ClientRectangle, backColorTop, backColorBottom, LinearGradientMode.Vertical))
          pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
      else
        using (Brush brush = new LinearGradientBrush(ClientRectangle, backColorBottom, backColorTop, LinearGradientMode.Horizontal))
          pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
    }

    #endregion

    private MeterSegment FindSegment(float value)
    {
      if (segments.Count == 0)
        return null;

      for (int i = 0; i < segments.Count - 1; i++)
      {
        if (value > segments[i].Percent)
          continue;
        return segments[i];
      }

      return segments[segments.Count - 1];
    }
  }

  class MeterSegmentCollection : Collection<MeterSegment>
  {
    private readonly Meter owner;

    public MeterSegmentCollection(Meter owner)
    {
      this.owner = owner;
    }

    protected override void InsertItem(int index, MeterSegment item)
    {
      base.InsertItem(index, item);
      item.Owner = owner;
    }
  }

  class MeterSegment
  {
    private Color activeColor;
    private Color backColor;
    private Pen activePen;
    private Pen backPen;
    private float percent;
    private Meter owner;

    public Color ActiveColor
    {
      get { return activeColor; }
      set
      {
        activeColor = value;
        if (activePen != null)
          activePen.Dispose();
        if (owner != null)
          activePen = new Pen(activeColor, owner.LineWidth);
      }
    }

    public Color BackColor
    {
      get { return backColor; }
      set
      {
        backColor = value;
        if(backPen != null)
          backPen.Dispose();
        if (owner != null)
          backPen = new Pen(backColor, owner.LineWidth);
      }
    }

    public Pen ActivePen
    {
      get { return activePen; }
    }

    public Pen BackPen
    {
      get { return backPen; }
    }

    public float Percent
    {
      get { return percent; }
      set { percent = value; }
    }

    [ReadOnly(true)]
    [Browsable(false)]
    public Meter Owner
    {
      get { return owner; }
      set
      {
        owner = value;
        activePen = new Pen(activeColor, owner.LineWidth);
        backPen = new Pen(backColor, owner.LineWidth);
      }
    }
  }
}
