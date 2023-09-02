using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Entities
{
    public class TodoItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTime? DueDate { get; set; }
    }
}
