using System.Collections.Generic;

namespace Checkers {
    public class Board {
        public int boardSize = 8;
        public Field[,] fields;
        public List<Player> players;
        public Player playersTurn;
        public int turnNum = 0;
        public Player winner;

        public Board() {
            players = new List<Player>();
            fields = new Field[boardSize, boardSize];
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    fields[i, j] = new Field(this, i, j);
                }
            }
        }

        public Field this[int x, int y] {
            get {
                return fields[x, y];
            }
            set {
                fields[x, y] = value;
            }
        }
        public void NextTurn(Move move) {
            if (move.isPromotion && move.piece is Pawn) {
                ((Pawn)move.piece).Promote();
            }
            playersTurn.activePiece = null;
            playersTurn = players[++turnNum % 2];
            playersTurn.UpdateAvailableMoves();
            if (playersTurn.IsDefeated()) {
                winner = players[++turnNum % 2];
            }
        }
        public void MakeMove(Move move) {
            if (move == null) {
                return;
            }
            if (playersTurn.activePiece != null && move.piece != playersTurn.activePiece) {
                playersTurn.UpdateAvailableMoves();
                return;
            }
            foreach (var f in fields) {
                if (f.val is Piece) {
                    f.val.availableMoves = new HashSet<Move>();
                }
            }
            fields[move.piece.field.x, move.piece.field.y].val = null;
            fields[move.moveTo.x, move.moveTo.y].val = move.piece;
            move.piece.field = move.moveTo;
            if (move.attackedPiece != null) {
                playersTurn.activePiece = move.piece;
                fields[move.attackedPiece.field.x, move.attackedPiece.field.y].val = null;
                players[(turnNum + 1) % 2].pieces.Remove(move.attackedPiece);
            } else {
                NextTurn(move);
                return;
            }
            foreach (var f in fields) {
                if (move.piece.CheckAttack(f) != null) {
                    return;
                }
            }
            NextTurn(move);
        }
    }
}
