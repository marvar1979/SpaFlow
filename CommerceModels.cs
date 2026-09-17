using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaFlow.Web.Models;

public class ProductCategory : BaseEntity
{
    public int SpaBusinessId { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
}

public class Product : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int ProductCategoryId { get; set; }
    [Required, MaxLength(140)] public string Name { get; set; } = string.Empty;
    [MaxLength(80)] public string? Sku { get; set; }
    [MaxLength(80)] public string? Barcode { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Cost { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal SalePrice { get; set; }
    public decimal Stock { get; set; }
    public decimal MinimumStock { get; set; }
    public ProductCategory Category { get; set; } = null!;
}

public class InventoryMovement : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int BranchId { get; set; }
    public int ProductId { get; set; }
    public InventoryMovementType Type { get; set; }
    public decimal Quantity { get; set; }
    public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;
    [MaxLength(300)] public string? Notes { get; set; }
    public Product Product { get; set; } = null!;
}

public class Sale : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int BranchId { get; set; }
    public int? ClientId { get; set; }
    public DateTime SoldAtUtc { get; set; } = DateTime.UtcNow;
    [Column(TypeName = "decimal(18,2)")] public decimal Subtotal { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Discount { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Total { get; set; }
    public Client? Client { get; set; }
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class SaleItem : BaseEntity
{
    public int SaleId { get; set; }
    public int? ProductId { get; set; }
    public int? SpaServiceId { get; set; }
    public decimal Quantity { get; set; } = 1;
    [Column(TypeName = "decimal(18,2)")] public decimal UnitPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal LineTotal { get; set; }
    public Sale Sale { get; set; } = null!;
    public Product? Product { get; set; }
    public SpaService? SpaService { get; set; }
}

public class Payment : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int? AppointmentId { get; set; }
    public int? SaleId { get; set; }
    public int? ClientId { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Paid;
    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }
    [MaxLength(150)] public string? Reference { get; set; }
    public DateTime PaidAtUtc { get; set; } = DateTime.UtcNow;
    public Appointment? Appointment { get; set; }
    public Sale? Sale { get; set; }
    public Client? Client { get; set; }
}

public class Expense : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int BranchId { get; set; }
    [Required, MaxLength(140)] public string Concept { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }
    public DateTime ExpenseAtUtc { get; set; } = DateTime.UtcNow;
    [MaxLength(300)] public string? Notes { get; set; }
}
