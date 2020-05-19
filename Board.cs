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
                    fields[i, j] = new Field(i, j);
                }
            }
        }
        public Board(Board board) {
            this.fields = new Field[boardSize, boardSize];
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    this.fields[i, j] = new Field(i, j);
                    if (board.fields[i, j].val == null) {
                        this.fields[i, j].val = null;
                    } else {
                        this.fields[i, j].val = new Pawn(board.fields[i, j].val.player,i,j);
                    }
                }
            }
            this.players = new List<Player>(board.players);
            this.playersTurn = new Player(board.playersTurn, this);
            this.turnNum = board.turnNum;
        }
        public Board(Board board, Move move) {
            this.fields = new Field[boardSize, boardSize];
            Move moveCopy;
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    this.fields[i, j] = new Field(i, j);
                    if (board.fields[i, j].val == null) {
                        this.fields[i, j].val = null;
                    } else {
                        if (board.fields[i,j].val is Pawn) {
                            this.fields[i, j].val = new Pawn(board.fields[i, j].val.player, i, j);
                        }
                        if (board.fields[i,j].val is King) {
                            this.fields[i, j].val = new King(board.fields[i, j].val.player, i, j);
                        }
                    }
                }
            }
            this.turnNum = board.turnNum;
            this.players = new List<Player>();
            var p1 = new Human(board.players[0], this);
            var p2 = new Computer(board.players[1], this);
            this.playersTurn = players[turnNum % 2];
            if (move.piece is Pawn) {
                moveCopy = new Move(new Pawn(move.piece.player, move.piece.x, move.piece.y), this.fields[move.moveTo.x, move.moveTo.y]);
            } else {
                moveCopy = new Move(new King(move.piece.player, move.piece.x, move.piece.y), this.fields[move.moveTo.x, move.moveTo.y]);
            }
            this.MakeMove(moveCopy);
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
            if (move == null || turnNum > 150) {
                return;
            }
            if (move.isPromotion && move.piece is Pawn) {
                ((Pawn)move.piece).Promote(this);
            }
            playersTurn.activePiece = null;
            playersTurn.canAttack = false;
            playersTurn = players[++turnNum % 2];
            if (playersTurn.IsDefeated()) {
                winner = players[++turnNum % 2];
            }
        }
        public void MakeMove(Move move) {
            if (move == null || turnNum > 150) {
                return;
            }
            fields[move.piece.x, move.piece.y].val = null;
            move.piece.x = move.moveTo.x;
            move.piece.y = move.moveTo.y;
            fields[move.moveTo.x, move.moveTo.y].val = move.piece;
            if (move.attackedPiece != null) {
                playersTurn.activePiece = move.piece;
                fields[move.attackedPiece.x, move.attackedPiece.y].val = null;
            } else {
                NextTurn(move);
                return;
            }
            foreach (var f in fields) {
                if (move.piece.CheckAttack(this, f) != null) {
                    return;
                }
            }
            NextTurn(move);
        }
        public void MakeTurn(Turn turn) {
            foreach (var move in turn.moves) {
                fields[move.piece.x, move.piece.y].val = null;
                move.piece.x = move.moveTo.x;
                move.piece.y = move.moveTo.y;
                fields[move.moveTo.x, move.moveTo.y].val = move.piece;
                if (move.attackedPiece != null) {
                    fields[move.attackedPiece.x, move.attackedPiece.y].val = null;
                }
            }
            NextTurn(turn.moves[turn.moves.Count - 1]);
        }
    }
}
