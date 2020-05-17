using System.Collections.Generic;

namespace Checkers {
    public abstract class Piece {
        public Color color;
        public Field field;
        public Player player;

        protected Piece(Player player, Color color) {
            this.color = color;
            this.player = player;
        }
        public Piece(Field field, Player player, Piece piece) {
            this.color = piece.color;
            this.field = field;
            this.player = player;
        }

        public HashSet<Move> GetAvailableMoves(Board board) {
            var availableMoves = new HashSet<Move>();
            foreach (Field f in board.fields) {
                var move = CheckMove(board, f);
                if (move != null) {
                    availableMoves.Add(move);
                }
            }
            return availableMoves;
        }
        public HashSet<Move> GetAvailableAttacks(Board board) {
            var availableAttacks = new HashSet<Move>();
            foreach (Field f in board.fields) {
                var attack = CheckAttack(board, f);
                if (attack != null) {
                    player.canAttack = true;
                    availableAttacks.Add(attack);
                }
            }
            return availableAttacks;
        }
        public HashSet<Move> GetMovesOrAttacks(Board board) {
            if (GetAvailableAttacks(board).Count > 0 || player.canAttack) {
                return GetAvailableAttacks(board);
            }
            return GetAvailableMoves(board);
        }
        public abstract Move CheckMove(Board board, Field field);
        public abstract Move CheckAttack(Board board, Field field);
    }
}
