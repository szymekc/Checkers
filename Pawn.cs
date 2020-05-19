using System;

namespace Checkers {
    public class Pawn : Piece {
        public int direction;
        public Pawn(Player player,int x,int y) : base(player,x,y) {
            if (this.color == Color.White) {
                direction = -1;
            } else {
                direction = 1;
            }
        }

        override public Move CheckMove(Board board, Field field) {
            if (field.val is Piece) {
                return null;
            }
            if (Math.Abs(x - field.x) == 1 && y - field.y == 1 * direction) {
                return new Move(this, field);
            }
            return null;
        }
        public override Move CheckAttack(Board board, Field field) {
            var idx = x - (2 * (x - field.x));
            var idy = y - 2 * direction;
            if (idx < 0 || idx > 7 || idy < 0 || idy > 7) {
                return null;
            }
            if (field.val is Piece && field.val.color != this.color) {
                if (Math.Abs(x - field.x) == 1 && y - field.y == 1 * direction) {
                    var dest = board[idx, idy];
                    if (dest.val == null) {
                        return new Move(this, dest) { attackedPiece = field.val };
                    }
                }
            }
            return null;
        }
        public void Promote(Board board) {
            var king = new King(this);
            board.fields[x,y].val = king;
        }
    }
}
