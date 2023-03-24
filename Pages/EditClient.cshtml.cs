using ASPNET_CORE_CRUD_APP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ASPNET_CORE_CRUD_APP.Pages
{
    public class EditClientModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";
        public Client client = new Client();
        public void OnGet()
        {
            var builder = WebApplication.CreateBuilder();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            string ClientID = Request.Query["id"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM App.Clients WHERE ClientID = @ClientID";

                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@ClientID", ClientID);

                    connection.Open();
                using(SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.Read()) 
                    { 
                        client.ClientID = reader.GetInt32(0).ToString();
                        client.Name = reader.GetString(1);
                        client.Email = reader.GetString(2);
                        client.Phone = reader.GetString(3); 

                    }
                }
                    connection.Close();
            }


        }

        public void OnPost()
        {
            var builder = WebApplication.CreateBuilder();
            client.ClientID = Request.Query["id"].ToString();
            client.Name = Request.Form["name"];
            client.Email = Request.Form["email"];
            client.Phone = Request.Form["phone"];


            if (client.ClientID.Length == 0 || client.Name.Length == 0 || 
                client.Email.Length == 0 || client.Phone.Length == 0)
            {
                errorMessage = "All feilds are required";
                return;
            }
            //Save to db
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            string query = "UPDATE App.Clients SET Name=@name, Email=@email, Phone=@phone WHERE ClientID = @clientID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@clientID", client.ClientID);
                        command.Parameters.AddWithValue("@name", client.Name);
                        command.Parameters.AddWithValue("@email", client.Email);
                        command.Parameters.AddWithValue("@phone", client.Phone);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }

            client.Name = "";
            client.Email = "";
            client.Phone = "";

            successMessage = "Client Edit successful!";
            Response.Redirect("/Clients");
        }

    }
}
