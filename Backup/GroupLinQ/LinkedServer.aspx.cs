using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class LinkedServer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InsertInStransaction()
        {
            using (var connection = new SqlConnection("server=GB-PC;database=LickedServer;uid=sa;pwd=123;"))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                using (var command = new SqlCommand("Insert_Student", connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentName", "Student 3");
                    command.Parameters.AddWithValue("@Address", "Ha Noi 3");
                    command.Parameters.AddWithValue("@Score", 9);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    finally { }
                }

                using (var command2 = new SqlCommand("TESTLINKED.munchendb.dbo.Users_Insert", connection, transaction))
                {
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.Parameters.AddWithValue("@fullName", "FullName 3");
                    command2.Parameters.AddWithValue("@userName", "UserName 3");
                    command2.Parameters.AddWithValue("@passWord", "PassWord 3");
                    command2.Parameters.AddWithValue("@email", "Email 3");
                    //command2.Parameters.AddWithValue("@createdOn", DateTime.Now);
                    //command2.Parameters.AddWithValue("@deletedOn", DateTime.Now);
                    try
                    {
                        command2.ExecuteNonQuery();
                    }
                    finally { }
                }

                transaction.Commit();
                connection.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertInStransaction();
        }
    }
}