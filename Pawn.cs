using System;

namespace Checkers {
    public class Pawn : Piece {
        public int direction;
        public Pawn(Board board, Player player, Color color) : base(board, player, color) {
            if (this.color == Color.White) {
                direction = -1;
            } else {
                direction = 1;
            }
        }

        override public Move CheckMove(Field field) {
            if (field.val is Piece) {
                return null;
            }
            if (Math.Abs(this.field.x - field.x) == 1 && this.field.y - field.y == 1 * direction) {
                return new Move(this, field);
            }
            return null;
        }
        public override Move CheckAttack(Field field) {
            var idx = this.field.x - (2 * (this.field.x - field.x));
            var idy = this.field.y - 2 * direction;
            if (idx < 0 || idx > 7 || idy < 0 || idy > 7) {
                return null;
            }
            if (field.val is Piece && field.val.color != this.color) {
                if (Math.Abs(this.field.x - field.x) == 1 && this.field.y - field.y == 1 * direction) {
                    var dest = board[idx, idy];
                    if (dest.val == null) {
                        return new Move(this, dest) { attackedPiece = field.val };
                    }
                }
            }
            return null;
        }
        public void Promote() {
            var king = new King(this);
            this.field.val = king;
        }
    }
}
