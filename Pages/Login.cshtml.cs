using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static lifeLINK.Pages.SignupModel;

namespace lifeLINK.Pages
{
    public class LoginModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public static int userId { get; set; }


        public void OnGet()
        {
        }

        public void OnPost()
        {
            userInfo.email = Request.Form["email"];
            userInfo.password = Request.Form["password"];

            if (userInfo.email.Length == 0 || userInfo.password.Length == 0)
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
                    String sql = "SELECT COUNT(*) FROM Users WHERE email = @email AND password = @password";
                    String sqlid = "SELECT id FROM Users WHERE email = @email AND password = @password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@password", userInfo.password);

                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            errorMessage = "Invalid email or password.";
                            return;
                        }

                        using (SqlCommand idCommand = new SqlCommand(sqlid, connection))
                        {
                            idCommand.Parameters.AddWithValue("@email", userInfo.email);
                            idCommand.Parameters.AddWithValue("@password", userInfo.password);

                            using (SqlDataReader reader = idCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userInfo.id = reader.GetInt32(0).ToString();

                                }
                            }
                        }


                        userInfo.email = "";
                        userInfo.password = "";

                        successMessage = "Logged in Successfully";
                        Response.Redirect("/");
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


        }
    }
}
