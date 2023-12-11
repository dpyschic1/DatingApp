using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group
    {
        //generate an empty constructor for entity framework schema and migrations
        public Group()
        {
            
        }
        public Group(string name)
        {
            Name = name;
        }
        [Key] //means it is going to be unique
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}