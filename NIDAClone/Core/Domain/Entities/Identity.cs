using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Identity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

    }
}