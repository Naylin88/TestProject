using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Transaction
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TransactionId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }

    }
}
