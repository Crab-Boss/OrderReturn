namespace OrderReturn
{
    public class OrderReturnConsts
    {
        public static int MaxOrderNumberLength { get; set; } = 64;
        public static int MaxParamJsonLength { get; set; } = 2048;
        public static int MaxFileNameLength { get; set; } = 64;
        public static int MaxTrackingIdLength { get; set; } = 128;
        public static int MaxSenderNameLength { get; set; } = 64;
        public static int MaxEmailLength { get; set; } = 64;
        public static int MaxPhoneLength { get; set; } = 32;
        public static int MaxReturnReasonLength { get; set;} = 256;

        public static int MaxOSIdLength { get; set; } = 64;
    }
}
