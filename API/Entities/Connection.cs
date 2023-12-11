namespace API.Entities
{
    public class Connection
    {
        public Connection()
        {
        }

        public Connection(string connectionID, string username)
        {
            ConnectionId = connectionID;
            Username = username;
        }

        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
}