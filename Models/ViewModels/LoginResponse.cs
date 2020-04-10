namespace Models.ViewModels
{
    public class LoginResponse
    {
        public string ErrorMessage { get; set; }

        public UserVM User { get; set; }
    }
}
