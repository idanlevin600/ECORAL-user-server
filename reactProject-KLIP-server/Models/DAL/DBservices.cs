using System.Data;
using System.Data.SqlClient;

namespace reactProject_KLIP_server.Models.DAL
{
    public class DBservices
    {
        public DBservices()
        {

        }

        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        // Create the SqlCommand using a stored procedure
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }


        //===============================================================
        //                        *                    *
        //                        *    USERS  METHODS  *
        //                        *                    *
        //===============================================================

        //This method returns all users
        public List<User> GetAllUsers()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedure("SP_GetAllUsers_React", con, null);             // create the command


            List<User> userList = new List<User>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    User u = new User();
                    u.UserId = Convert.ToInt32(dataReader["userId"]);
                    u.Name = dataReader["name"].ToString();
                    u.Password = dataReader["password"].ToString();
                    u.Email = dataReader["email"].ToString();
                    u.Gender = dataReader["gender"].ToString();
                    u.BirthDate = Convert.ToDateTime(dataReader["birthdate"]);

                    userList.Add(u);
                }
                return userList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        // This method insert a user to the users table 
        public int InsertUser(User user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@name", user.Name);
            paramDic.Add("@email", user.Email);
            paramDic.Add("@password", user.Password);
            paramDic.Add("@gender", user.Gender);
            paramDic.Add("@birthdate", user.BirthDate);
            //paramDic.Add("@regDate", user.RegistrationTime);



            cmd = CreateCommandWithStoredProcedure("SP_UserRegistration_React", con, paramDic);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //This method returns user by id
        public User GetUserById(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userId", id);


            cmd = CreateCommandWithStoredProcedure("SP_GetUserById_React", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            User u = new User();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.UserId = Convert.ToInt32(dataReader["userId"]);
                    u.Name = dataReader["name"].ToString();
                    u.Password = dataReader["password"].ToString();
                    u.Email = dataReader["email"].ToString();
                    u.Gender = dataReader["gender"].ToString();
                    u.BirthDate = Convert.ToDateTime(dataReader["birthdate"]);

                }

                return u;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }
        }

        // This method is for the login user
        public User LoginUser(string email, string password)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userEmail", email);
            paramDic.Add("@userPassword", password);

            cmd = CreateCommandWithStoredProcedure("SP_userLogin_React", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataReader.Read();

                User u = new User();
                u.UserId = Convert.ToInt32(dataReader["userId"]);
                u.Name = dataReader["name"].ToString();
                u.Password = dataReader["password"].ToString();
                u.Email = dataReader["email"].ToString();
                u.Gender = dataReader["gender"].ToString();
                u.BirthDate = Convert.ToDateTime(dataReader["birthdate"]);

                return u;
            }
            catch (Exception ex)
            {
                // write to log
                return null;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }

        }

        // This methode delete a user from the users table
        public int DeleteUser(string email)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);
   
            cmd = CreateCommandWithStoredProcedure("SP_deleteUser_React", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }
        }

        //This method returns user by email
        public User GetUserByEmail(string email)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);


            cmd = CreateCommandWithStoredProcedure("SP_GetUserByEmail_React", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            User u = new User();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.UserId = Convert.ToInt32(dataReader["userId"]);
                    u.Name = dataReader["name"].ToString();
                    u.Password = dataReader["password"].ToString();
                    u.Email = dataReader["email"].ToString();
                    u.Gender = dataReader["gender"].ToString();
                    u.BirthDate = Convert.ToDateTime(dataReader["birthdate"]);

                }

                return u;
            }
            catch (Exception ex)
            {
                // write to log
                return null;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }
        }

        // This methode update a user password from the users table
        public int UpdateUserPassword(string email, string password)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);
            paramDic.Add("@password", password);

            cmd = CreateCommandWithStoredProcedure("SP_UpdateUserPassword_React", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }
        }
    
    }


}
