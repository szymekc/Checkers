using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers {
    class Computer : Player {
        public Computer(Color color, Board board) : base(color, board) {
        }
        public int EvaluateBoard(Board board) {
            var me = board.players.Find((a) => a.color == this.color);
            var enemy = board.players.Find((a) => a.color != this.color);
            int score = 0;
            foreach (Field field in board.fields) {
                if (field.val != null) {
                    if (field.val.color == this.color) {
                        score += EvaluatePiece(field.val);
                    } else {
                        score -= EvaluatePiece(field.val);
                    }
                }
            }
            return score;
        }
        public int EvaluatePiece(Piece piece) {
            int pawnValue = 10;
            int kingValue = 20;
            if (piece is Pawn) {
                return pawnValue;
            } else return kingValue;
        }
        public (int, Move) Minimax(Board board, int depth, int maxDepth) {
            int chosenScore = 0;
            Move chosenMove = null;
            if (depth == maxDepth) {
                chosenScore = EvaluateBoard(board);
                return (chosenScore, chosenMove);
            } else {
                var moves = this.GetAvailableMoves();
                if (moves.Count == 0) {
                    chosenScore = EvaluateBoard(board);
                    return (chosenScore, chosenMove);
                } else {
                    if (board.playersTurn == this) {
                        int bestScore = Int32.MinValue;
                        foreach (var move in moves) {
                            Move bestMove;
                            Board newBoard = new Board(board, move);
                            System.Console.WriteLine(depth);
                            (int the_score, Move the_move) = Minimax(newBoard, depth + 1, maxDepth);
                            if (the_score > bestScore) {
                                bestScore = the_score;
                                bestMove = the_move;
                            }
                        }
                    } else {
                        int bestScore = Int32.MaxValue;
                        foreach (var move in moves) {
                            Move bestMove;
                            Board newBoard = new Board(board, move);
                            (int the_score, Move the_move) = Minimax(newBoard, depth + 1, maxDepth);
                            if (the_score < bestScore) {
                                bestScore = the_score;
                                bestMove = the_move;
                            }
                        }
                    }
                    return (chosenScore, chosenMove);
                }
            }
        }
    }
}
