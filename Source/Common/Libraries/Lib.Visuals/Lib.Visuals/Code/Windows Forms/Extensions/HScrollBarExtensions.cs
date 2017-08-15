using System.Windows.Forms;


namespace Minex.Common.Lib.Visuals.WindowsForms.Extensions
{
    public static class HScrollBarExtensions
    {
        public static int ActualMaximum(this HScrollBar scrollBar)
        {
            int output = 1 + scrollBar.Maximum - scrollBar.LargeChange;
            return output;
        }
    }
}
