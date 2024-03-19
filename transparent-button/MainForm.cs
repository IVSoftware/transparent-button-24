using System.Diagnostics;

namespace transparent_button
{
    public partial class MainForm : Form
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.control.region?view=windowsdesktop-8.0
        public MainForm()
        {
            InitializeComponent();
            button.Paint += (sender, e) =>
            {

                System.Drawing.Drawing2D.GraphicsPath buttonPath =
                    new System.Drawing.Drawing2D.GraphicsPath();

                Rectangle newRectangle = button.ClientRectangle;

                // Decrease the size of the rectangle.
                newRectangle.Inflate(-10, -10);

                // Draw the button's border.
                e.Graphics.DrawEllipse(System.Drawing.Pens.Black, newRectangle);

                // Increase the size of the rectangle to include the border.
                newRectangle.Inflate(1, 1);

                // Create a circle within the new rectangle.
                buttonPath.AddEllipse(newRectangle);

                // Set the button's Region property to the newly created 
                // circle region.
                button.Region = new System.Drawing.Region(buttonPath);

            };
        }
    }
}
