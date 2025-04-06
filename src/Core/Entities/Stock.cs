using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using portfolium.Application.Enums;

namespace portfolium.Core.Entities;

public class Stock {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StockId { get; set; }

    [MaxLength(5)] [Required] public string Symbol { get; set; }

    [MaxLength(30)] [Required] public string CompanyName { get; set; }

    [Column(TypeName = "Decimal(18, 10)")]
    [Range(0, double.MaxValue)]
    public decimal CurrentPrice { get; set; }

    public IndustryType Industry { get; set; }

    public long MarketCap { get; set; }
}