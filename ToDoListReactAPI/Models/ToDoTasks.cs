using System.ComponentModel.DataAnnotations;

namespace ToDoListReactApi.API.Models
{
    public class ToDoTasks
    {
        [Key]
        public int taskId { get; set; } //primary key

        public int userId { get; set; } //foreign key

        public string Title { get; set; }

        public string status { get; set; }
        
        public Users users { get; set; }

    }
}
