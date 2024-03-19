using System.Diagnostics;

namespace transparent_button
{
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
            var buttonRestart = new Button
            {
                Size = new Size(200,80),
                Text = "Restart",
                Location = new Point(
                    (ClientSize.Width - 200) / 2,
                    (ClientSize.Height - 80) / 2
                ),
                Font = new Font("Kaushan Script", 18),
                BackColor = Color.LimeGreen,
            };

            // Change shape of Button control, removing transparent pixels.
            Bitmap bitmap = new Bitmap(buttonRestart.Width, buttonRestart.Height);
            buttonRestart.DrawToBitmap(bitmap, buttonRestart.ClientRectangle);
            Region region = new Region(buttonRestart.ClientRectangle);
            for (int x = 0; x < buttonRestart.Width; x++) for (int y = 0; y < buttonRestart.Height; y++)
                {
                    if (bitmap.GetPixel(x, y).Equals(Color.LimeGreen))
                    {
                        region.Exclude(new Rectangle(x, y, 1, 1));
                    }
                }
            buttonRestart.Region = region;
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
}
