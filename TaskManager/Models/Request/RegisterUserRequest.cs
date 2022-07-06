namespace TaskManager.Models.Request
{
    public class RegisterUserRequest
    {
        private static Random random = new Random();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public RegisterUserRequest(string firstName, string lastName, string email, string userName, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
            Phone = phone;
            
        }
    }
}
