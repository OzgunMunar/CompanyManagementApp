using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreMVCApp.Models;

public partial class Order
{
    [Key] public Guid OrderGuid { get; set; }

    [Required]public DateTime OrderDate { get; set; }
    [Required]public bool OrderIsShipped { get; set; }
    [Required]public string OrderComments { get; set; }
    [Required]public Guid OrderCustomerGuid { get; set; }
    [Required]public bool IsActive { get; set; }

    [ForeignKey("OrderCustomerGuid")] public Customer Customers { get; set; }
}
