using System.Collections.Generic;

namespace Checkers {
    public abstract class Piece {
        public Color color;
        public Field field;
        public Board board;
        public Player player;

        protected Piece(Board board, Player player, Color color) {
            this.board = board;
            this.color = color;
            this.player = player;
        }

        public HashSet<Move> GetAvailableMoves() {
            var availableMoves = new HashSet<Move>();
            foreach (Field f in board.fields) {
                var move = CheckMove(f);
                if (move != null) {
                    availableMoves.Add(move);
                }
            }
            return availableMoves;
        }
        public HashSet<Move> GetAvailableAttacks() {
            var availableAttacks = new HashSet<Move>();
            foreach (Field f in board.fields) {
                var attack = CheckAttack(f);
                if (attack != null) {
                    player.canAttack = true;
                    availableAttacks.Add(attack);
                }
            }
            return availableAttacks;
        }
        public HashSet<Move> GetMovesOrAttacks() {
            if (GetAvailableAttacks().Count > 0 || player.canAttack) {
                return GetAvailableAttacks();
            }
            return GetAvailableMoves();
        }
        public abstract Move CheckMove(Field field);
        public abstract Move CheckAttack(Field field);
    }
}
