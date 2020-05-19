using System;

namespace Checkers {
    public class King : Piece {
        public King(Player player, int x, int y) : base(player, x, y) {
        }
        public King(Pawn pawn) : base(pawn.player, pawn.x, pawn.y) {
            this.color = pawn.color;
            this.player = pawn.player;
        }
        public override Move CheckAttack(Board board, Field field) {
            var idx = x - (2 * (x - field.x));
            var idy = y - (2 * (y - field.y));
            if (idx < 0 || idx > 7 || idy < 0 || idy > 7) {
                return null;
            }
            if (field.val is Piece && field.val.color != this.color) {
                if (Math.Abs(x - field.x) == 1 && Math.Abs(y - field.y) == 1) {
                    var dest = board[idx, idy];
                    if (dest.val == null) {
                        return new Move(this, dest) { attackedPiece = field.val };
                    }
                }
            }
            return null;
        }

        override public Move CheckMove(Board board, Field field) {
            if (field.val != null) {
                return null;
            }
            if (Math.Abs(x - field.x) == 1 && Math.Abs(y - field.y) == 1) {
                return new Move(this, field);
            }
            return null;
        }
    }
}
