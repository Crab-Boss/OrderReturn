namespace OrderReturn.Dtos.GLS
{
    public class GetGLSReturnLabelInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string ReturnReason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SenderDto Sender { get; set; }
    }
}
