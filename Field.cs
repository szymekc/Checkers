using System;
using System.Diagnostics.CodeAnalysis;

namespace Checkers {
    public class Field : IEquatable<Field> {
        public Piece val;
        public int x;
        public int y;

        public Field(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public bool Equals([AllowNull] Field other) {
            return this.x == other.x && this.y == other.y;
        }
    }
}
