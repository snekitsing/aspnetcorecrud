using ASPNET_CORE_CRUD_APP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace ASPNET_CORE_CRUD_APP.Pages
{
    public class ClientsModel : PageModel
    {
        public List<Client> Client = new List<Client>();
        public void OnGet()
        {
            var builder = WebApplication.CreateBuilder();
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM App.Clients";
                SqlCommand command = new SqlCommand(query, connection);   
                
                using (SqlDataReader reader = command.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        //Client client = new Client(
                        //    reader.GetInt32(0).ToString(),
                        //    reader.GetString(1).ToString(),
                        //    reader.GetString(2).ToString(),
                        //    reader.GetString(3).ToString());
                        Client client = new Client();
                        client.ClientID = reader.GetInt32(0).ToString();
                        client.Name = reader.GetString(1).ToString();
                        client.Email = reader.GetString(2).ToString();
                        client.Phone = reader.GetString(3).ToString();

                        Client.Add(client);
                    }                
                
                }

            }
        }
    }
}
