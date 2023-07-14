using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreMVCApp.Models;

public partial class Customer
{
    [Key] public Guid CustomerGuid { get; set; }

    [Required] public string CustomerName { get; set; }
    [Required] public int CustomerPhoneNumber { get; set; }
    [Required] public string CustomerAddress { get; set; }
    [Required] public string CustomerPostalCode { get; set; }
    [Required] public Guid CustomerBoughtFromEmployeeGuid { get; set; }
    [Required] public bool IsActive { get; set; }

    [ForeignKey("CustomerBoughtFromEmployeeGuid")] public Employee Employees { get; set; }

    public ICollection<Order> Orders { get; set; }

}
