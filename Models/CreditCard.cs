using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DOL.Models;

public class CreditCard
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }


}
