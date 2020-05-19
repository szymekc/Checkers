namespace Checkers {
    public class Human : Player {
        public Human(Color color, Board board) : base(color, board) {
        }
        public Human(Player player, Board board) : base(player, board) {

        }
    }
}
