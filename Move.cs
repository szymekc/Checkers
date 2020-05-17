using System;
using System.Diagnostics.CodeAnalysis;

namespace Checkers {
    public class Move : IEquatable<Move> {
        public Piece piece;
        public Field moveTo;
        public Piece attackedPiece;
        public bool isPromotion;

        public Move(Piece piece, Field moveTo) {
            this.piece = piece;
            this.moveTo = moveTo;
            if (piece.color == Color.Black && moveTo.y == 0 && piece is Pawn) {
                isPromotion = true;
            }
            if (piece.color == Color.White && moveTo.y == 7 && piece is Pawn) {
                isPromotion = true;
            }
        }

        public bool Equals([AllowNull] Move other) {
            return other.piece == piece && other.moveTo == moveTo && other.attackedPiece == attackedPiece;
        }
    }
}
