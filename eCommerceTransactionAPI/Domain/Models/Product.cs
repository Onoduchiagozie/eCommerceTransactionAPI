using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceTransactionAPI.Domain.Models
{
    public class Product
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }         // optional
        public int? Quantity { get; set; }        // optional
        public string? Description { get; set; }  // optional
        public int? Price { get; set; }           // optional
    }
}
