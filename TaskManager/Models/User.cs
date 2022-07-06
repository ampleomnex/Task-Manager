using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string UserName { get; }


        public User(string firstName, string lastName, string email, string userName, Guid id = new Guid())
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
        }
    }
}
