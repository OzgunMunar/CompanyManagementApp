namespace AspNetCoreMVCApp.DTO.OrderDTO
{
    public class OrderGetByGuidDTO
    {
        public Guid _OrderGuid { get; set; }
        public DateTime _OrderDate { get; set; }
        public bool _IsOrderShipped { get; set; }
        public string _OrderComments { get; set; }
        public Guid _OrderCustomerGuid { get; set; }
        public string _CustomerName { get; set; }
        public bool _IsActive { get; set; }
    }
}
