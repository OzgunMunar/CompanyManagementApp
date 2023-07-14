using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVCApp.Models;

public partial class EmployeeJob
{
    [Key] public Guid EmployeeJobTitleGuid { get; set; }

    [Required] public string EmployeeJobTitle { get; set; }
    [Required] public bool IsActive { get; set; }

    public ICollection<Employee> Employees { get; set; }
}
