using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;
using Vendor_Portal.App_Code.DAL;

namespace Canopy_Billing.BDM
{
    public partial class ClientMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static void SaveClient(string clientName, string Address, List<ContactModel> contacts)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Clients(ClientName, Address) OUTPUT INSERTED.ClientID VALUES(@ClientName, @Address)", con);
                cmd.Parameters.AddWithValue("@ClientName", clientName);
                cmd.Parameters.AddWithValue("@Address", Address);

                int clientId = (int)cmd.ExecuteScalar();

                foreach (var c in contacts)
                {
                    SqlCommand cmd2 = new SqlCommand(@"
                    INSERT INTO ClientContacts
                    (ClientID, ContactPerson, ContactNo, Email, IsPrimary)
                    VALUES (@ClientID, @Person, @No, @Email, 1)", con);

                    cmd2.Parameters.AddWithValue("@ClientID", clientId);
                    cmd2.Parameters.AddWithValue("@Person", c.ContactPerson);
                    cmd2.Parameters.AddWithValue("@No", c.ContactNo);
                    cmd2.Parameters.AddWithValue("@Email", c.Email);

                    cmd2.ExecuteNonQuery();
                }
            }
        }
        [WebMethod]
        public static void SaveSingleContact(ContactModel model)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO ClientContacts
            (ClientID, ContactPerson, ContactNo, Email, IsPrimary)
            VALUES (@ClientID, @Person, @No, @Email, 0)", con);

                cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
                cmd.Parameters.AddWithValue("@Person", model.ContactPerson);
                cmd.Parameters.AddWithValue("@No", model.ContactNo);
                cmd.Parameters.AddWithValue("@Email", model.Email);

                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public static List<ClientDTO> GetClients()
        {
            List<ClientDTO> list = new List<ClientDTO>();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT 
                c.ClientID,
                c.ClientName,
                cc.ContactPerson,
                cc.ContactNo,
                cc.Email,
             cc.IsPrimary
            FROM Clients c
            LEFT JOIN ClientContacts cc 
                ON c.ClientID = cc.ClientID
            ORDER BY c.ClientID DESC", con))
                {
                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new ClientDTO
                        {
                            ClientID = Convert.ToInt32(dr["ClientID"]),
                            ClientName = dr["ClientName"].ToString(),
                            ContactPerson = dr["ContactPerson"].ToString(),
                            ContactNo = dr["ContactNo"].ToString(),
                            Email = dr["Email"].ToString(),
                            IsPrimary = Convert.ToString(dr["IsPrimary"]) != "" ? Convert.ToBoolean(dr["IsPrimary"]) : false
                        });
                    }
                }
            }

            return list;
        }

        [WebMethod]
        public static List<ClientVM> GetClientsGrouped()
        {
            List<ClientVM> list = new List<ClientVM>();

            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT c.ClientID, c.ClientName, c.Address,cc.ContactID,
                   cc.ContactPerson, cc.ContactNo, cc.Email, cc.IsPrimary
            FROM Clients c
            LEFT JOIN ClientContacts cc ON c.ClientID = cc.ClientID", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var dict = new Dictionary<int, ClientVM>();

                while (dr.Read())
                {
                    int clientId = Convert.ToInt32(dr["ClientID"]);

                    if (!dict.ContainsKey(clientId))
                    {
                        dict[clientId] = new ClientVM
                        {
                            ClientID = clientId,
                            ClientName = dr["ClientName"].ToString(),
                            Address = dr["Address"].ToString(),
                            OtherContacts = new List<ContactModel>()
                        };
                    }

                    if (dr["ContactPerson"] != DBNull.Value)
                    {
                        var contact = new ContactModel
                        {
                            ContactPerson = dr["ContactPerson"].ToString(),
                            ContactNo = dr["ContactNo"].ToString(),
                            Email = dr["Email"].ToString(),
                            ContactID = Convert.ToInt32(dr["ContactID"])
                        };

                        bool isPrimary = Convert.ToString(dr["IsPrimary"]) == "" ? false : Convert.ToBoolean(dr["IsPrimary"]);

                        if (isPrimary)
                            dict[clientId].PrimaryContact = contact;
                        else
                            dict[clientId].OtherContacts.Add(contact);
                    }
                }

                list = dict.Values.ToList();
            }

            return list;
        }

        [WebMethod]
        public static void SetPrimaryContact(int clientId, int contactId)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                // Remove existing primary
                SqlCommand cmd1 = new SqlCommand(
                    "UPDATE ClientContacts SET IsPrimary = 0 WHERE ClientID = @ClientID", con);
                cmd1.Parameters.AddWithValue("@ClientID", clientId);
                cmd1.ExecuteNonQuery();

                // Set new primary
                SqlCommand cmd2 = new SqlCommand(
                    "UPDATE ClientContacts SET IsPrimary = 1 WHERE ContactID = @ContactID", con);
                cmd2.Parameters.AddWithValue("@ContactID", contactId);
                cmd2.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public static void UpdateContact(ContactModel model)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
            UPDATE ClientContacts
            SET ContactPerson = @Person,
                ContactNo = @No,
                Email = @Email
            WHERE ContactID = @ContactID", con);

                cmd.Parameters.AddWithValue("@Person", model.ContactPerson);
                cmd.Parameters.AddWithValue("@No", model.ContactNo);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@ContactID", model.ContactID);

                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public static void DeleteContact(int contactId)
        {
            using (SqlConnection con = new SqlConnection(SQLHelper.ConnectionString2))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM ClientContacts WHERE ContactID = @ID", con);

                cmd.Parameters.AddWithValue("@ID", contactId);
                cmd.ExecuteNonQuery();
            }
        }

        public class ContactModel
        {
            public int ClientID { get; set; }
            public string ContactPerson { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
            public int ContactID { get; set; }
        }
        public class ClientDTO
        {
            public int ClientID { get; set; }
            public string ClientName { get; set; }
            public string Address { get; set; }
            public string ContactPerson { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
            public bool IsPrimary { get; set; }
        }

        public class ClientVM
        {
            public int ClientID { get; set; }
            public string ClientName { get; set; }
            public string Address { get; set; }
            public ContactModel PrimaryContact { get; set; }
            public List<ContactModel> OtherContacts { get; set; }
        }


    }
}