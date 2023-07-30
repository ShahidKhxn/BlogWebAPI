namespace BlogWebAPI.ResModel
{
    public class ResponseModel
    {
        public object Response { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public int? TotalRecords { get; set; }
        public object Error { get; set; }
        public bool Status { get; set; }
    }
}
