using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace lifeLINK.Pages
{
    public class SignupModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            userInfo.firstName = Request.Form["fname"];
            userInfo.lastName = Request.Form["lname"];
            userInfo.email = Request.Form["email"];
            userInfo.phone = Request.Form["phone"];
            userInfo.address = Request.Form["address"];
            userInfo.password = Request.Form["password"];

            if (userInfo.firstName.Length == 0 || userInfo.lastName.Length == 0 || userInfo.email.Length == 0 || userInfo.phone.Length == 0 || userInfo.address.Length == 0 || userInfo.password.Length == 0)
            {
                errorMessage = "All fields are required MR/MRS";
                return;
            }

            try
            {
                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=lifeLink;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Users " +
                                 "(firstName, lastName, email, phone, address, password) VALUES " +
                                 "(@fname, @lname, @email, @phone, @address, @password);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@fname", userInfo.firstName);
                        command.Parameters.AddWithValue("@lname", userInfo.lastName);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@phone", userInfo.phone);
                        command.Parameters.AddWithValue("@address", userInfo.address);
                        command.Parameters.AddWithValue("@password", userInfo.password);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            userInfo.firstName = "";
            userInfo.lastName = "";
            userInfo.email = "";
            userInfo.phone = "";
            userInfo.address = "";
            userInfo.password = "";

            successMessage = "Account Created Successfully";

            Response.Redirect("/Login");
        }

        public class UserInfo
        {
            public String? id;
            public String? firstName;
            public String? lastName;
            public String? email;
            public String? phone;
            public String? address;
            public String? password;
        }
    }
}
