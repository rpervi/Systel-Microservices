using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyService.Domain.Entities;

[Table("companymaster", Schema = "dbo")]
public class companymaster
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("companyid")]
    public int companyid { get; set; }

    [Column("cname")]
    [MaxLength(200)]
    public string? cname { get; set; }

    [Column("ccode")]
    [MaxLength(50)]
    public string? ccode { get; set; }

    [Column("cdesc")]
    public string? cdesc { get; set; }

    [Column("caddress")]
    [MaxLength(300)]
    public string? caddress { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public string? email { get; set; }

    [Column("phone")]
    [MaxLength(100)]
    public string? phone { get; set; }

    [Column("website")]
    [MaxLength(100)]
    public string? website { get; set; }

    [Column("category")]
    [MaxLength(100)]
    public string? category { get; set; }

    [Column("subcategory")]
    [MaxLength(100)]
    public string? subcategory { get; set; }

    [Column("contactperson")]
    [MaxLength(100)]
    public string? contactperson { get; set; }

    [Column("isactive")]
    public bool isactive { get; set; }

    [Column("isdeleted")]
    public bool isdeleted { get; set; }

    [Column("createdby")]
    [MaxLength(50)]
    public string? createdby { get; set; }

    [Column("createdon")]
    public DateTime? createdon { get; set; }

    [Column("modifiedby")]
    [MaxLength(50)]
    public string? modifiedby { get; set; }

    [Column("modifiedon")]
    public DateTime? modifiedon { get; set; }

    [Column("ctype")]
    [MaxLength(50)]
    public string? ctype { get; set; }

    [Column("isdefault")]
    public int? isdefault { get; set; }

    [Column("tagline")]
    [MaxLength(2000)]
    public string? tagline { get; set; }

    [Column("fax")]
    [MaxLength(20)]
    public string? fax { get; set; }

    [Column("toll")]
    [MaxLength(20)]
    public string? toll { get; set; }

    [Column("companylogo")]
    public string? companylogo { get; set; }

    [Column("certificationname")]
    [MaxLength(50)]
    public string? certificationname { get; set; }

    [Column("certificationlink")]
    public string? certificationlink { get; set; }

    [Column("notes")]
    public string? notes { get; set; }

    [Column("tnc")]
    public string? tnc { get; set; }
}