# Transparent Button

Winforms buttons do not support this kind of transparency. What I have done in the past is override `Button` to make a custom behavior where the button is painted to match what's behind it like a cloaking technique.


[Placeholder]

```
class TransparentButton : Button
{
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        if (!DesignMode)
        {
            // Detect size/location changes
            if ((PointToScreen(Location) != _prevLocation) || (Size != _prevSize))
            {
                bool isInitial = false;
                if ((BackgroundImage == null) || !BackgroundImage.Size.Equals(Size))
                {
                    isInitial = true;
                    BackgroundImage = new Bitmap(Width, Height);
                }
                if (MouseButtons.Equals(MouseButtons.None))
                {
                    // Hide button, take screenshot, show button again
                    Visible = false;
                    BeginInvoke(async () =>
                    {
                        Parent?.Refresh();
                        if (isInitial) await Task.Delay(250);
                        using (var graphics = Graphics.FromImage(BackgroundImage))
                        {
                            graphics.CopyFromScreen(PointToScreen(new Point()), new Point(), Size);
                        }
                        Visible = true;
                    });
                }
                else
                {
                    using (var graphics = Graphics.FromImage(BackgroundImage))
                        graphics.FillRectangle(Brushes.LightGray, graphics.ClipBounds);
                }
            }
            _prevLocation = PointToScreen(Location);
            _prevSize = Size;
        }
    }
    Point _prevLocation = new Point(int.MaxValue, int.MaxValue);
    Size _prevSize = new Size(int.MaxValue, int.MaxValue);
    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);
        Refresh();
    }
}
```
**Main Form**

Here's the main form code that outs this snippet in some context.

```
public partial class MainForm : Form
{
    Random _rando = new Random(1);
    public MainForm()
    {
        InitializeComponent();
        for (int row = 0; row < tableLayoutPanel.RowCount; row++)
        {
            for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
            {
                var number = _rando.Next(1, 65);
                var button = new Button
                {
                    Name = $"button_{col}_{row}",
                    Tag = number,
                    Text = $"{number}",
                    Anchor = (AnchorStyles)0xf,
                    Font = new Font(Font, FontStyle.Bold),
                    BackColor = localGetGradientColor(number)
                };
                tableLayoutPanel.Controls.Add(button,col, row);
                button.Click += Any_Clicked;
                Color localGetGradientColor(int number)
                {
                    Color startColor = Color.FromArgb(0x8E, 0xC6, 0x3F);
                    Color endColor = Color.FromArgb(0x01, 0xAC, 0xC6);
                    float scale = (float)(number - 1) / 64;
                    int r = (int)(startColor.R + (endColor.R - startColor.R) * scale);
                    int g = (int)(startColor.G + (endColor.G - startColor.G) * scale);
                    int b = (int)(startColor.B + (endColor.B - startColor.B) * scale);
                    return Color.FromArgb(r, g, b);
                }
            }
        }
        var buttonRestart = new TransparentButton
        {
            Size = new Size(200,80),
            Text = "Restart",
            Location = new Point(
                (ClientSize.Width - 200) / 2,
                (ClientSize.Height - 80) / 2
            ),
            Font = new Font("Kaushan Script", 18),
        };
        SizeChanged += (sender, e) =>
        {
            buttonRestart.Location = new Point(
                (ClientSize.Width - 200) / 2,
                (ClientSize.Height - 80) / 2
            );
        };
        buttonRestart.Click += async(sender, e) =>
        {
            buttonRestart.Hide();
            await Task.Delay(TimeSpan.FromSeconds(2.5));
            buttonRestart.Show();
        };
        Controls.Add(buttonRestart);
        Controls.SetChildIndex(buttonRestart, 0);
    }
    private void Any_Clicked(object? sender, EventArgs e)
    {
    }
    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await Task.Delay(TimeSpan.FromSeconds(2.5));
    }
}
```