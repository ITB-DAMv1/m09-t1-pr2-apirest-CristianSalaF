namespace T1PR2_Client.Model
{
    public class GameDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string DeveloperTeamName { get; set; }

        public string? ImageUrl { get; set; }

        public int VoteCount { get; set; }
    }
}
