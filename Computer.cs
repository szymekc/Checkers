using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers {
    public class Computer : Player {
        public Computer(Color color, Board board) : base(color, board) {
        }
        public Computer(Player player, Board board) : base(player, board) {

        }
        public int EvaluateBoard(Board board) {
            var me = board.players.Find((a) => a.color == this.color);
            var enemy = board.players.Find((a) => a.color != this.color);
            int score = 0;
            foreach (Field field in board.fields) {
                if (field.val != null) {
                    if (field.val.color == this.color) {
                        score += EvaluatePiecePreferEdges(field);
                    } else {
                        score -= EvaluatePiecePreferEdges(field);
                    }
                }
            }
            return score;
        }
        public int EvaluatePieceOnField(Field field) {
            int pawnValue = 10;
            int kingValue = 30;
            double distanceMult;
            if (field == null || field.val == null) {
                return 0;
            }
            if (field.val.color == Color.Black) {
                distanceMult = (double)(1.0 + ((board.boardSize-1 - field.y) / 10.0));
            } else {
                distanceMult = (double)((field.y) / 10.0) + 1.0;
            }
            if (field.val is Pawn) {
                return (int)(pawnValue * distanceMult);
            } else return (int)(kingValue);
        }
        public int EvaluatePiecePreferEdges(Field field) {
            int pawnValue = 10;
            int kingValue = 20;
            double distanceMult = 1.0;
            if (field == null || field.val == null) {
                return 0;
            }
            if (field.x == 0 || field.x == 7 || field.y == 0 || field.y == 7) {
                distanceMult = 1.2;
            }
            if (field.val is Pawn) {
                return (int)(pawnValue * distanceMult);
            } else return (int)(kingValue);
        }
        public int Minimax(Turn turn, int depth, bool isMaximizing) {
            int value = 0;
            Board copy = board;
            foreach (var move in turn.moves) {
                copy = new Board(copy, move);
            }
            if (depth == 0 || copy.playersTurn.GetAvailableMoves().Count == 0) {
                return EvaluateBoard(copy);
            }
            if (isMaximizing) {
                value = Int32.MinValue;
                foreach (var child in copy.playersTurn.GetAvailableTurns()) {
                    value = Math.Max(value, Minimax(child, depth - 1, false));
                }
            } else {
                value = Int32.MaxValue;
                foreach (var child in copy.playersTurn.GetAvailableTurns()) {
                    value = Math.Min(value, Minimax(child, depth - 1, true));
                }
            }
            return value;
        }
        public int AlphaBeta(Turn turn, int depth, int alpha, int beta, bool isMaximizing) {
            int value = 0;
            Board copy = board;
            foreach (var move in turn.moves) {
                copy = new Board(copy, move);
            }
            if (depth == 0 || copy.playersTurn.GetAvailableTurns().Count == 0) {
                return EvaluateBoard(copy);
            }
            if (isMaximizing) {
                value = Int32.MinValue;
                foreach (var child in copy.playersTurn.GetAvailableTurns()) {
                    value = Math.Max(value, AlphaBeta(child, depth - 1, alpha, beta, false));
                    alpha = Math.Max(alpha, value);
                    if (alpha >= beta) {
                        break;
                    }
                }
            } else {
                value = Int32.MaxValue;
                foreach (var child in copy.playersTurn.GetAvailableTurns()) {
                    value = Math.Min(value, AlphaBeta(child, depth - 1, alpha, beta, true));
                    beta = Math.Min(beta, value);
                    if (alpha >= beta) {
                        break;
                    }
                }
            }
            return value;
        }
        public Turn GetBestTurnMinimax() {
            Random rnd = new Random();
            int bestValue = Int32.MinValue;
            List<Turn> best = new List<Turn>();
            foreach (var turn in this.GetAvailableTurns()) {
                int value = Minimax(turn, 5, true);
                if (value >= bestValue) {
                    bestValue = value;
                    best.Add(turn);
                }
            }
            return best.ElementAt(rnd.Next(best.Count));
        }
        public Turn GetBestTurn() {
            Random rnd = new Random();
            int bestValue = Int32.MinValue;
            List<Turn> best = new List<Turn>();
            foreach (var turn in this.GetAvailableTurns()) {
                int value = AlphaBeta(turn, 5, Int32.MinValue, Int32.MaxValue, true);
                if (value >= bestValue) {
                    bestValue = value;
                    best.Add(turn);
                }
            }
            return best.ElementAt(rnd.Next(best.Count));
        }
    }
}
