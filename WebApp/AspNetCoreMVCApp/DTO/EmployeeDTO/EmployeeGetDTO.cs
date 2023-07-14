using Microsoft.CodeAnalysis.Operations;

namespace AspNetCoreMVCApp.DTO.EmployeeDTO
{
    public class EmployeeGetDTO
    {

        public Guid _EmployeeGuid { get; set; }
        public string _EmployeeName { get; set; }
        public int _EmployeePhoneNumber { get; set; }
        public string _EmployeeEmail { get; set; }
        public string _EmployeePassword { get; set; }
        public Guid _EmployeeOfficeGuid { get; set; }
        public string _EmployeeOffice { get; set; }
        public Guid? _EmployeeJobGuid { get; set; }
        public string _EmployeeJobTitle { get; set; }
        public Guid _EmployeeManagerGuid { get; set; }
        public string _EmployeeManager { get; set; }
        public DateTime _EmployeeStartDate { get; set; }

    }
}
