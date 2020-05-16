using System.Collections.Generic;
using System.Linq;

namespace Checkers {
    public class Player {
        public Color color;
        public Board board;
        public Piece activePiece;
        public bool canAttack;


        public Player(Player player) {
            this.color = player.color;
            this.board = player.board;
            this.activePiece = player.activePiece;
            this.canAttack = player.canAttack;
        }
        public Player(Color color, Board board) {
            this.color = color;
            this.board = board;
            if (color == Color.White) {
                for (int x = 0; x < board.boardSize; x += 2) {
                    for (int y = 0; y < 3; y++) {
                        Pawn pawn = new Pawn(this.board, this, this.color) { field = board[x + (y % 2), y] };
                        board.fields[x + (y % 2), y].val = pawn;
                    }
                }
            } else {
                for (int x = 0; x < board.boardSize; x += 2) {
                    for (int y = 7; y > 4; y--) {
                        Pawn pawn = new Pawn(this.board, this, this.color) { field = board[x + (y % 2), y] };
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
                return activePiece.GetAvailableAttacks();
            }
            foreach (var piece in pieces) {
                allMoves.UnionWith(piece.GetMovesOrAttacks());
            }
            if (this.canAttack) {
                allMoves.RemoveWhere((a) => a.attackedPiece == null);
            }
            return allMoves;
        }
        public bool IsDefeated() {
            return GetAvailableMoves().Count == 0;
        }
    }
}
