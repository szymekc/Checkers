using System.Collections;
using System.Collections.Generic;

namespace Checkers {
    public class Board {
        public int boardSize = 8;
        public Field[,] fields;
        public List<Player> players;
        public Player playersTurn;
        public int turnNum = 0;
        public Player winner;
        public bool isBase = true;

        public Board() {
            players = new List<Player>();
            fields = new Field[boardSize, boardSize];
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    fields[i, j] = new Field(this, i, j);
                }
            }
        }
        public Board(Board board, Move move) {
            this.isBase = false;
            this.fields = new Field[boardSize,boardSize];
            this.fields = (Field[,])board.fields.Clone();
            this.players = new List<Player>(board.players);
            this.playersTurn = new Player(board.playersTurn);
            this.turnNum = board.turnNum;
            this.MakeMove(move);
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
            playersTurn.canAttack = false;
            playersTurn = players[++turnNum % 2];
            if (playersTurn.IsDefeated()) {
                winner = players[++turnNum % 2];
            }
            if (playersTurn is Computer && isBase) {
                MakeMove(((Computer)playersTurn).Minimax(this, 0, 3).Item2);
            }
        }
        public void MakeMove(Move move) {
            if (move == null) {
                return;
            }
            fields[move.piece.field.x, move.piece.field.y].val = null;
            fields[move.moveTo.x, move.moveTo.y].val = move.piece;
            move.piece.field = move.moveTo;
            if (move.attackedPiece != null) {
                playersTurn.activePiece = move.piece;
                fields[move.attackedPiece.field.x, move.attackedPiece.field.y].val = null;
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
