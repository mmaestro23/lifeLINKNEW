using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lifeLINK.Pages
{
    public class AdminModel : PageModel
    {
        public List<DonorInfo> listDonors = new List<DonorInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=lifeLink;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM donated";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DonorInfo donorInfo = new DonorInfo();
                                donorInfo.id = "" + reader.GetInt32(0);
                                donorInfo.nid = reader.GetString(1);
                                donorInfo.fullNames = reader.GetString(2);
                                donorInfo.email = reader.GetString(3);
                                donorInfo.phone = reader.GetString(4);
                                donorInfo.address = reader.GetString(5);
                                donorInfo.bloodGroup = reader.GetString(6);
                                donorInfo.RHfactor = reader.GetString(7);
                                donorInfo.organ = reader.GetString(8);
                                donorInfo.description = reader.GetString(9);

                                listDonors.Add(donorInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
}
