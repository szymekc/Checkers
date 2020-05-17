using System;
using System.Diagnostics.CodeAnalysis;

namespace Checkers {
    public class Field : IEquatable<Field> {
        public Board board;
        public Piece val;
        public int x;
        public int y;

        public Field(Board board, int x, int y) {
            this.board = board;
            this.x = x;
            this.y = y;
        }

        public bool Equals([AllowNull] Field other) {
            return this.x == other.x && this.y == other.y && this.val == other.val;
        }
    }
}
