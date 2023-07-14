using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVCApp.Models;

public partial class Office
{
    [Key] public Guid OfficeGuid { get; set; }

    [Required]public string OfficeCity { get; set; }
    [Required]public int OfficePhoneNumber { get; set; }
    [Required]public string OfficeAddress { get; set; }
    [Required]public string OfficePostalCode { get; set; }
    [Required]public bool IsActive { get; set; }

    public ICollection<Employee> Employees { get; set; }
}
