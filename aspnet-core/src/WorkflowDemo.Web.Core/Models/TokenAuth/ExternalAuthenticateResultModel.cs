namespace WorkflowDemo.Models.TokenAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalAuthenticateResultModel
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
        public bool WaitingForActivation { get; set; }
    }
}
