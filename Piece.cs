using System.Collections.Generic;

namespace Checkers {
    public abstract class Piece {
        public Color color;
        public HashSet<Move> availableMoves;
        public Field field;
        public Board board;
        public Player player;

        protected Piece(Board board, Player player, Color color) {
            this.board = board;
            this.color = color;
            this.player = player;
            availableMoves = new HashSet<Move>();
        }

        public HashSet<Move> GetAvailableMoves() {
            foreach (Field f in board.fields) {
                var attack = CheckAttack(f);
                if (attack != null) {
                    availableMoves.Add(attack);
                }
            }
            if (availableMoves.Count > 0 || player.canAttack) {
                return availableMoves;
            }
            foreach (Field f in board.fields) {
                var move = CheckMove(f);
                if (move != null) {
                    availableMoves.Add(move);
                }
            }
            return availableMoves;
        }

        public abstract Move CheckMove(Field field);
        public abstract Move CheckAttack(Field field);
    }
}
