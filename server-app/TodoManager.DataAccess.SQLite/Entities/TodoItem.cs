using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoManager.DataAccess.SQLite.Entities
{
    [Table("TodoItems")]
    internal class TodoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        public bool IsCompleted { get; set; }
    }
}