using System.ComponentModel.DataAnnotations;

namespace App.UseCase.Models.Orders;

public class CreateOrderDto
{
    [Required]
    public int CollectionId { get; set; }
}
