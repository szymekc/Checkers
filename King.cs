using System;

namespace Checkers {
    public class King : Piece {
        public King(Board board, Player player, Color color) : base(board, player, color) {
        }
        public King(Pawn pawn) : base(pawn.board, pawn.player, pawn.color) {
            this.color = pawn.color;
            this.field = pawn.field;
            this.board = pawn.board;
            this.player = pawn.player;
        }
        public override Move CheckAttack(Field field) {
            var idx = this.field.x - (2 * (this.field.x - field.x));
            var idy = this.field.y - (2 * (this.field.y - field.y));
            if (idx < 0 || idx > 7 || idy < 0 || idy > 7) {
                return null;
            }
            if (field.val is Piece && field.val.color != this.color) {
                if (Math.Abs(this.field.x - field.x) == 1 && Math.Abs(this.field.y - field.y) == 1) {
                    var dest = board[idx, idy];
                    if (dest.val == null) {
                        return new Move(this, dest) { attackedPiece = field.val };
                    }
                }
            }
            return null;
        }

        override public Move CheckMove(Field field) {
            if (field.val != null) {
                return null;
            }
            if (Math.Abs(this.field.x - field.x) == 1 && Math.Abs(this.field.y - field.y) == 1) {
                return new Move(this, field);
            }
            return null;
        }
    }
}
