namespace Commerce.Command.Contract.Shared
{
    public class AuthResult
    {
        public bool Success { get; set; }  
        public string Token { get; set; }  
        public List<string> Errors { get; set; } = new List<string>(); 

        public AuthResult() { }

        public AuthResult(bool success, string token = "", List<string> errors = null)
        {
            Success = success;
            Token = token;
            Errors = errors ?? new List<string>();
        }
    }
}
