namespace T1PR2_APIREST.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
