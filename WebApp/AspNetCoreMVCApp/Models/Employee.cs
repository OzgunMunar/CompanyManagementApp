using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreMVCApp.Models;

public partial class Employee
{
    [Key] public Guid EmployeeGuid { get; set; }

    [Required] public string EmployeeName { get; set; }
    [Required] public int EmployeePhoneNumber { get; set; }
    [Required] public string EmployeeEmail { get; set; }
    [Required] public Guid EmployeeJobGuid { get; set; }
    [Required] public Guid EmployeeOfficeGuid { get; set; }
    [Required] public Guid EmployeeManagerGuid { get; set; }
    [Required] public string EmployeePassword { get; set; }
    [Required] public DateTime EmployeeStartDate { get; set; }
    [Required] public bool IsActive { get; set; }

    [ForeignKey("EmployeeOfficeGuid")] public Office Office { get; set; }
    [ForeignKey("EmployeeJobGuid")] public EmployeeJob EmployeeJob { get; set; }

    public ICollection<Customer> Customers { get; set; }

}
