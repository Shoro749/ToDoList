using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.models
{
    public class Lists
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        public User User { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
