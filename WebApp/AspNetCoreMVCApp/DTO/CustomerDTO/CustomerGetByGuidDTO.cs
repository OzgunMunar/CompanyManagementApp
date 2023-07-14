namespace AspNetCoreMVCApp.DTO.CustomerDTO
{
    public class CustomerGetByGuidDTO
    {

        public Guid _CustomerGuid { get; set; }
        public string _CustomerName { get; set; }
        public int _CustomerPhoneNumber { get; set; }
        public string _CustomerAddress { get; set; }
        public string _CustomerPostalCode { get; set; }
        public Guid _CustomerBoughtFromEmployeeGuid { get; set; }
        public string _CustomerBoughtFromEmployeeName { get; set; }
        public bool _IsActive { get; set; }

    }
}
