namespace Checkers {
    public class Field {
        public Board board;
        public Piece val;
        public int x;
        public int y;

        public Field(Board board, int x, int y) {
            this.board = board;
            this.x = x;
            this.y = y;
        }
    }
}
