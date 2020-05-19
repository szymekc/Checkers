using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers {
    public class Turn {
        public List<Move> moves;

        public Turn(List<Move> moves) {
            this.moves = moves;
        }
        public Turn(HashSet<Move> move) {
            this.moves = move.ToList();
        }
    }
}
