namespace WorkflowDemo.Models.TokenAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateResultModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ExpireInSeconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }
    }
}
