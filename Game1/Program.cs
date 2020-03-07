using System;

namespace Game1
{
////#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //using (var game = new VBLib.Game1())
            //    game.Run();

            //var game = new VBLib.Game1();
            //game.Run();

            var Frm = new VBGL.FrmMenu();
            Frm.ShowDialog();
            

        }
    }
//#endif
}
