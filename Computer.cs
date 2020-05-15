using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers {
    class Computer : Player {
        public Computer(Color color, Board board) : base(color, board) {
        }
        public int EvaluateBoard(Board board) {
            var me = board.players.Find((a) => a.color == this.color);
            var enemy = board.players.Find((a) => a.color != this.color);
            throw new NotImplementedException();
        }
    }
}
