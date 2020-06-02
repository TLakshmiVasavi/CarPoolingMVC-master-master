namespace CarPoolingMVC.Models
{
    public class AuthResponseVM : UserVM
    {
        public string ErrorMessage { get; set; }

        public bool IsSuccess { get; set; }

        public string Token { get; set; }

        //public UserVM User { get; set; }
    }
}

