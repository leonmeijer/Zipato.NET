namespace LVMS.Zipato.Model
{
    public class SecurityUpdateItem
    {
        public string UpdateType { get; set; }
        public string ClientSessionId { get; set; }
        public string TransactionId { get; set; }
        public string EventType { get; set; }
        public string SecureSessionId { get; set; }
        public string ClassName { get; set; }
        public bool Success { get; set; }
        public string Nonce { get; set; }
        public string Salt { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
