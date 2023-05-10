using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lifeLINK.Pages
{
    public class ContactModel : PageModel
    {
        public ContactUs contactUs = new ContactUs();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() {
            contactUs.names = Request.Form["names"];
            contactUs.email = Request.Form["email"];
            contactUs.phone = Request.Form["phone"];
            contactUs.question = Request.Form["q"];
            if (contactUs.names.Length == 0 || contactUs.email.Length == 0 || contactUs.phone.Length == 0 || contactUs.question.Length == 0)
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
                    String sql = "INSERT INTO contactUs " +
                    "(names, email, phone, question) VALUES " +
                                 "(@names, @email, @phone, @q);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@names", contactUs.names);
                        command.Parameters.AddWithValue("@email", contactUs.email);
                        command.Parameters.AddWithValue("@phone", contactUs.phone);
                        command.Parameters.AddWithValue("@q", contactUs.question);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            contactUs.names = "";
            contactUs.email = "";
            contactUs.phone = "";
            contactUs.question = "";

            successMessage = "Your Question Was Received!";

            Response.Redirect("/");
        }

        public class ContactUs
        {
            public String? id;
            public String? names;
            public String? email;
            public String? phone;
            public String? question;
        }
    }
}
