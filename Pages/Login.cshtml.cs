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
                String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=lifelink;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT COUNT(*) FROM UsersTable WHERE email = @email AND password = @password";
                    String sqlid = "SELECT COUNT(*) FROM AdminTable WHERE email = @email AND password = @password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@password", userInfo.password);

						using (SqlCommand idCommand = new SqlCommand(sqlid, connection))
						{
							idCommand.Parameters.AddWithValue("@email", userInfo.email);
							idCommand.Parameters.AddWithValue("@password", userInfo.password);

							int count = (int)command.ExecuteScalar();
                            int countadmin = (int)idCommand.ExecuteScalar();
                            if (count == 0 && countadmin == 0)
                            {
                                errorMessage = "Invalid email or password.";
                                return;
                            }

                            else if(count != 0 && countadmin == 0)
                            {
								successMessage = "Logged in Successfully";
								Response.Redirect("/");
							}

                            else
                            {
								successMessage = "Logged in Successfully";
								Response.Redirect("/Admin");
							}
                            
                        }


                        userInfo.email = "";
                        userInfo.password = "";
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
