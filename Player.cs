using System.Collections.Generic;

namespace Checkers {
    public class Player {
        public List<Piece> pieces;
        public Color color;
        public Board board;
        public bool canAttack;
        public Piece activePiece;

        public Player(Color color, Board board) {
            this.color = color;
            this.board = board;
            pieces = new List<Piece>();
            if (color == Color.White) {
                for (int x = 0; x < board.boardSize; x += 2) {
                    for (int y = 0; y < 3; y++) {
                        Pawn pawn = new Pawn(this.board, this, this.color) { field = board.fields[x + (y % 2), y] };
                        board.fields[x + (y % 2), y].val = pawn;
                        pieces.Add(pawn);
                    }
                }
            } else {
                for (int x = 0; x < board.boardSize; x += 2) {
                    for (int y = 7; y > 4; y--) {
                        Pawn pawn = new Pawn(this.board, this, this.color) { field = board.fields[x + (y % 2), y] };
                        board.fields[x + (y % 2), y].val = pawn;
                        pieces.Add(pawn);
                    }
                }
            }
            board.players.Add(this);
            board.playersTurn = board.players[0];
            UpdateAvailableMoves();
        }
        public void UpdateAvailableMoves() {
            canAttack = false;
            if (activePiece != null) {
                foreach (var piece in pieces) {
                    piece.availableMoves = new HashSet<Move>();
                }
                foreach (var move in activePiece.GetAvailableMoves()) {
                    if (move.attackedPiece != null) {
                        canAttack = true;
                    }
                }
                return;
            }
            foreach (var piece in pieces) {
                foreach (var move in piece.GetAvailableMoves()) {
                    if (move.attackedPiece != null) {
                        canAttack = true;
                    }
                }
            }
            if (canAttack) {
                foreach (var piece in pieces) {
                    piece.availableMoves.RemoveWhere((a) => a.attackedPiece == null);
                }
            }
        }
        public bool IsDefeated() {
            if (pieces.Count == 0) {
                return true;
            }
            foreach (var piece in pieces) {
                if (piece.availableMoves.Count > 0) {
                    return false;
                }
            }
            return true;
        }
    }
}
