using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoListReactApi.API.Models
{
    public class Users
    {
        [Key]
        public int userId { get; set; } //primary key

        public string name { get; set; }


        [JsonIgnore]
        public ICollection<ToDoTasks> todotasks { get; set; }
    }
}
