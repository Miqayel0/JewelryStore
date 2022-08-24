using System.ComponentModel.DataAnnotations;

namespace App.UseCase.Models.Orders;

public class OrderDto
{
    [Required]
    public int CollectionId { get; set; }
}
