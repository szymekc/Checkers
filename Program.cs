using System;
using System.Windows.Forms;

namespace Checkers {
    public static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Board board = new Board();
            var player = new Human(Color.Black, board);
            var opponent = new Computer(Color.White, board);
            board.playersTurn = board.players[0];

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Display(board));
        }
    }
}
