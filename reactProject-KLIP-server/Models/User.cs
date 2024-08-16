using reactProject_KLIP_server.Models.DAL;

namespace reactProject_KLIP_server.Models
{
    public class User
    {
        int userId;
        string name;
        string email;
        string password;
        string gender;
        DateTime birthDate;

        public int UserId { get => userId; set => userId = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Gender { get => gender; set => gender = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }

        static public List<User> getAllUsers()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllUsers();
        }
        
        static public User getUserById(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetUserById(id);
        }
        
        static public User getUserByEmail(string email)
        {
            DBservices dbs = new DBservices();
            return dbs.GetUserByEmail(email);
        }

        public bool Registration()
        {
            DBservices dbs = new DBservices();
            if (dbs.InsertUser(this) > 0)
            {
                return true;
            }
            return false;
        }

        static public User logIn(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.LoginUser(email, password);

            throw new Exception("The data you have entered are not true");
        }

        static public int deleteUser(User u)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteUser(u.email);

            throw new Exception("The data you have entered are not true");
        }

        static public int updateUserPassword(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateUserPassword(email, password);

            throw new Exception("The data you have entered are not true");
        }


    }
}
