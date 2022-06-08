namespace OrderReturn.Dtos.DHL
{
    public class DHLReturnResponse
    {
        public string ShipmentNumber { get; set; }
        public string LabelData { get; set; }
        public string QrLabelData { get; set; }
        public string RoutingCode { get; set; }
    }
}
