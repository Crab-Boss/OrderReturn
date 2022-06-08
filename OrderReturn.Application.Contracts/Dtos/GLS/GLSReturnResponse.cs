namespace OrderReturn.Dtos.GLS
{
    public class GLSReturnResponse
    {
        public string Name { get; set; }
        public string ReturnId { get; set; }
        public string ParceNumber { get; set; }
        public string ParcelNumberWithChecksum { get; set; }
        public string TrackingId { get; set; }
        public string SignedLabelUrl { get; set; }
        public string SignedQrCodeUrl { get; set; }
        public string SignedLabelKey { get; set; }
        public string SignedQrCodeKey { get; set; }
    }
}
