using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Common
{
    public abstract class UniqueEntityData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
