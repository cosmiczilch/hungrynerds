namespace Game {
    public class Player {

        public enum PlayerType_t {
            Us,
            Other
        }

        public PlayerType_t PlayerType {
            get; private set;
        }

        public Player(PlayerType_t playerType) {
            this.PlayerType = playerType;
        }
    }
}