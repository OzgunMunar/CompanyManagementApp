namespace AspNetCoreMVCApp.DTO.OfficeDTO
{
    public class OfficeGetDTO
    {
        public Guid _OfficeGuid { get; set; }
        public string _OfficeCity { get; set; }
        public int _OfficePhoneNumber { get; set; }
        public string _OfficeAddress { get; set; }
        public string _OfficePostalCode { get; set; }
        public bool _IsActive { get; set; }
    }
}
