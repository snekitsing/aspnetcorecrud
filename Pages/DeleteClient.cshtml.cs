using ASPNET_CORE_CRUD_APP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ASPNET_CORE_CRUD_APP.Pages
{
    public class DeleteClientModel : PageModel
    {
        public void OnGet()
        {
            var builder = WebApplication.CreateBuilder();
            string ClientID = Request.Query["id"].ToString();
            
            //Save to db
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            string query = "DELETE FROM App.Clients WHERE ClientID = @clientID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@clientID", ClientID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Response.Redirect("/Clients");
        }
    }
}
