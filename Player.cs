using System.Collections.Generic;
using System.Linq;

namespace Checkers {
    public class Player {
        public Color color;
        public Board board;
        public Piece activePiece;
        public bool canAttack;


        public Player(Player player, Board board) {
            this.color = player.color;
            this.board = board;
            this.activePiece = null;
            this.canAttack = player.canAttack;
            board.players.Add(this);
        }
        public Player(Color color, Board board) {
            this.color = color;
            this.board = board;
            if (color == Color.White) {
                for (int x = 0; x < board.boardSize; x += 2) {
                    for (int y = 0; y < 3; y++) {
                        Pawn pawn = new Pawn(this, x + (y % 2), y);
                        board.fields[x + (y % 2), y].val = pawn;
                    }
                }
            } else {
                for (int x = 0; x < board.boardSize; x += 2) {
                    for (int y = 7; y > 4; y--) {
                        Pawn pawn = new Pawn(this, x + (y % 2), y);
                        board.fields[x + (y % 2), y].val = pawn;
                    }
                }
            }
            board.players.Add(this);
            board.playersTurn = board.players[0];
        }
        public List<Piece> GetPieces() {
            var list = new List<Piece>();
            foreach (Field field in board.fields) {
                if (field.val != null && field.val.color == this.color) {
                    list.Add(field.val);
                }
            }
            return list;
        }
        public HashSet<Move> GetAvailableMoves() {
            canAttack = false;
            var pieces = GetPieces();
            var allMoves = new HashSet<Move>();
            if (activePiece != null) {
                return activePiece.GetAvailableAttacks(board);
            }
            foreach (var piece in pieces) {
                allMoves.UnionWith(piece.GetMovesOrAttacks(board));
            }
            if (this.canAttack) {
                allMoves.RemoveWhere((a) => a.attackedPiece == null);
            }
            return allMoves;
        }
        public HashSet<Turn> GetAvailableTurns() {
            var allMoves = GetAvailableMoves();
            var allTurns = new HashSet<Turn>();
            foreach (var move in allMoves) {
                Turn turn;
                if (move.attackedPiece != null) {
                    var moveList = new HashSet<Move>();
                    moveList.Add(move);
                    turn = new Turn(CheckMultiAttack(moveList));
                } else {
                    turn = new Turn(new List<Move>() { move });
                }
                allTurns.Add(turn);
            }
            return allTurns;
        }
        public HashSet<Move> CheckMultiAttack(HashSet<Move> moveList) {
            var copy = board;
            foreach (var move in moveList) {
                if (move.isPromotion) {
                    return moveList;
                }
                copy = new Board(copy, move);
            }
            foreach (var field in copy.fields) {
                if (copy.fields[moveList.ElementAt(moveList.Count - 1).moveTo.x, moveList.ElementAt(moveList.Count - 1).moveTo.y].val != null && 
                    copy.fields[moveList.ElementAt(moveList.Count - 1).moveTo.x, moveList.ElementAt(moveList.Count - 1).moveTo.y].val.CheckAttack(copy, field) != null){
                    moveList.Add(copy.fields[moveList.ElementAt(moveList.Count - 1).moveTo.x, moveList.ElementAt(moveList.Count - 1).moveTo.y].val.CheckAttack(copy, field));
                    return CheckMultiAttack(moveList);
                }
            }
            return moveList;
        }
        public bool IsDefeated() {
            return GetAvailableMoves().Count == 0;
        }
    }
}
