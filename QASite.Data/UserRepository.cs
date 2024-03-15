namespace QASite.Data
{
    public class UserRepository
    {
        private string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, string password)
        {
            var context = new StackOverflowContext(_connectionString);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = passwordHash;
            context.Users.Add(user);
            context.SaveChanges();
        }
        public User GetUserByEmail(string email)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public User GetUserForLogin(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }
            return user;
        }
        public List<User> GetUsers()
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Users.ToList();
        }
    }
}