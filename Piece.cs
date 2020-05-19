using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Checkers {
    public abstract class Piece {
        public Color color;
        public Player player;
        public int x;
        public int y;
        public Piece(Player player, int x, int y) {
            this.color = player.color;
            this.player = player;
            this.x = x;
            this.y = y;
        }
        public Field GetField(Board board) {
            return board.fields[x, y];
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
