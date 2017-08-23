using System;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Useful methods.
    /// </summary>
    public static class Functions
    {
        public static void DrawVector(double startX, double startY, double vX, double vY, Axes axes)
        {
            string command = String.Format(@"quiver({0}, {1}, {2}, {3}, {4}, 0)", axes.HandleName, startX, startY, vX, vY); // Last zero indicates no auto-scaling (draw the vector exactly).
            axes.Application.Execute(command);
        }

        public static void DrawVector(double startX, double startY, double startZ, double vX, double vY, double vZ, Axes axes)
        {
            string command = String.Format(@"quiver3({0}, {1}, {2}, {3}, {4}, {5}, {6}, 0)", axes.HandleName, startX, startY, startZ, vX, vY, vZ); // Last zero indicates no auto-scaling (draw the vector exactly).
            axes.Application.Execute(command);
        }

        public static void DrawText(double xPosition, double yPosition, string text, Axes axes)
        {
            string command = String.Format(@"text({0}, {1}, '{2}', 'Parent', {3})", xPosition, yPosition, text, axes.HandleName);
            axes.Application.Execute(command);
        }

        public static void DrawText(double xPosition, double yPosition, double zPosition, string text, Axes axes)
        {
            string command = String.Format(@"text({0}, {1}, {2}, '{3}', 'Parent', {4})", xPosition, yPosition, zPosition, text, axes.HandleName);
            axes.Application.Execute(command);
        }
    }
}
