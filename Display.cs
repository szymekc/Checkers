using Checkers.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers {
    public partial class Display : Form {
        public Panel selected;
        public Board board;
        public Stopwatch stopwatch = new Stopwatch();
        public TimeSpan ts;
        public Display(Board board) {
            this.board = board;
            InitializeComponent();
            foreach (Control c in BoardLayout.Controls)
                if (c is Panel) {
                    c.Click += HandleCellClick;
                    c.Controls.Find("pic", true)[0].Click += HandleCellClick;
                }
        }

        private async void Form1_Load(object sender, EventArgs e) {
            await UpdateBoard();
        }

        private async void UpdateTurnLabel() {
            if (board.playersTurn.color == Color.Black) {
                if (stopwatch != null) {
                    ts = stopwatch.Elapsed;
                    turn.Text = "Black's turn, time: " + ts.TotalSeconds + " s";
                } else {
                    turn.Text = "Black's turn";
                }
            } else {
                turn.Text = "Red's turn";
            }
            if (board.winner != null) {
                turn.Text = "Game over";
            }
        }
        private async Task UpdateBoard() {
            UpdateTurnLabel();
            foreach (Control c in BoardLayout.Controls) {
                if (c is Panel) {
                    var pos = BoardLayout.GetPositionFromControl(c);
                    var pic = (PictureBox)c.Controls.Find("pic", true)[0];
                    if (board.fields[pos.Column, pos.Row].val == null) {
                        pic.Image = null;
                    } else if (board[pos.Column, pos.Row].val.color == Color.Black && board[pos.Column, pos.Row].val is Pawn) {
                        pic.Image = Resources.black;
                    } else if (board[pos.Column, pos.Row].val.color == Color.White && board[pos.Column, pos.Row].val is Pawn) {
                        pic.Image = Resources.red;
                    } else if (board[pos.Column, pos.Row].val.color == Color.Black && board[pos.Column, pos.Row].val is King) {
                        pic.Image = Resources.blackking;
                    } else {
                        pic.Image = Resources.redking;
                    }
                }
            }
        }
        private async void HandleCellClick(object sender, EventArgs e) {
            Panel p;
            if (sender is PictureBox) {
                p = (Panel)((PictureBox)sender).Parent;
            } else {
                p = (Panel)sender;
            }
            if (selected != null &&
                board[BoardLayout.GetPositionFromControl(selected).Column, BoardLayout.GetPositionFromControl(selected).Row].val != null &&
                board[BoardLayout.GetPositionFromControl(selected).Column, BoardLayout.GetPositionFromControl(selected).Row].val.player == board.playersTurn &&
                board[BoardLayout.GetPositionFromControl(selected).Column, BoardLayout.GetPositionFromControl(selected).Row].val.player is Human) {
                var pos = BoardLayout.GetPositionFromControl(selected);
                var fieldPos = BoardLayout.GetPositionFromControl(p);
                Move move = board.playersTurn.GetAvailableMoves().FirstOrDefault(
                    (a) => a.piece.GetField(board) == board[pos.Column, pos.Row] && a.moveTo.x == fieldPos.Column && a.moveTo.y == fieldPos.Row);
                board.MakeMove(move);
                turn.Text = "Thinking...";
                if (!board.playersTurn.IsDefeated() && board.playersTurn is Computer) {
                    stopwatch.Restart();
                    board.MakeTurn(((Computer)board.playersTurn).GetBestTurn());
                    stopwatch.Stop();
                }
            } 
                await DeselectAll();
                selected = p;
                selected.BackColor = SystemColors.Highlight;
                await UpdateBoard();
                await HighlightMoves();
            

        }
        private async Task Deselect() {
            if (selected == null) {
                return;
            }
            var pos = BoardLayout.GetPositionFromControl(selected);
            if ((pos.Column + pos.Row) % 2 == 0) {
                selected.BackColor = SystemColors.ControlDark;
            } else {
                selected.BackColor = SystemColors.ControlLight;
            }
        }
        private async Task DeselectAll() {
            selected = null;
            foreach (Control c in BoardLayout.Controls) {
                var pos = BoardLayout.GetPositionFromControl(c);
                if (c is Panel) {
                    if ((pos.Column + pos.Row) % 2 == 0) {
                        c.BackColor = SystemColors.ControlDark;
                    } else {
                        c.BackColor = SystemColors.ControlLight;
                    }
                }
            }
        }
        private async Task HighlightMoves() {
            if (selected == null) {
                return;
            }
            var pos = BoardLayout.GetPositionFromControl(selected);
            if (board[pos.Column, pos.Row].val != null) {
                HashSet<Move> list = board[pos.Column, pos.Row].val.GetMovesOrAttacks(board);
                foreach (Move move in list) {
                    var x = move.moveTo.x;
                    var y = move.moveTo.y;
                    var c = BoardLayout.GetControlFromPosition(x, y);
                    c.BackColor = SystemColors.Highlight;
                }
            }
        }
    }
}
