using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BudgetApp.Data.Entites
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        [Required]
        public required string UserID { get; set; }
        public int AccountID { get; set; }
        public int CategoryID { get; set; }
        public int ReceiptID { get; set; }
        [Required]
        [StringLength(200)]
        public required string Merchant { get; set; }
        [Required]
        public int AmountCents { get; set; }
        [Required]
        [StringLength(3)]
        public required string Currency { get; set; }
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; } = null;
    }
}
