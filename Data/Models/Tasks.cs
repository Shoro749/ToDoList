using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.models
{
    public class Tasks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(256)]
        public string Desc { get; set; }

        [Required, StringLength(32)]
        public string Status { get; set; }

        public DateTime Date { get; set; }

        public Lists List { get; set; }
    }
}
