#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ProdsAndCats.Models;
public class Association
{
    [Key]
    public int AssociationId { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int CategoryId { get; set; }

    public Product? Product {get;set;}
    public Category? Category {get;set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}