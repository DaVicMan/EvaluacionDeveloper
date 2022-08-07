using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EvaluacionDeveloper
{
    public class Database
    {
        private static SqlConnection GetConnection()
        {
            return new SqlConnection("Server=DESKTOP-UNDJRHE;Database=EvaluacionDeveloper;Trusted_Connection=True;");
        }
        public static string[] SelectSingleColumn(string query)
        {

            List<string> Response = new List<string>();
            using (SqlConnection sqlConn = GetConnection())
            {
                using (SqlCommand sqlCMD = new SqlCommand(query, sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlRead = sqlCMD.ExecuteReader())
                    {
                        while (sqlRead.Read())
                        {
                            Response.Add(sqlRead[0].ToString());
                        }
                    }
                    sqlConn.Close();

                }
            }
            return Response.ToArray();
        }
        public static DataTable SelectStoredProcedure(string query, Dictionary<string, string> Parameters = null)
        {
            DataTable Datos = new DataTable();

            using (SqlConnection sqlConn = GetConnection())
            {
                using (SqlCommand sqlCMD = new SqlCommand(query, sqlConn))
                {
                    if (Parameters != null)
                        foreach (KeyValuePair<string, string> Parameter in Parameters)
                            sqlCMD.Parameters.AddWithValue(Parameter.Key, Parameter.Value);


                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlConn.Open();
                    Datos.Load(sqlCMD.ExecuteReader());
                    sqlConn.Close();
                }
            }
            return Datos;
        }
        public static string ExecuteInsertStoredProcedure(string Query, Dictionary<string, string> Parametros = null)
        {
            string QueryResponse = string.Empty;
            using (SqlConnection sqlCon = GetConnection())
            {
                using (SqlCommand sqlCMD = new SqlCommand(Query, sqlCon))
                {
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    if (Parametros != null)
                        foreach (KeyValuePair<string, string> Parameter in Parametros)
                            sqlCMD.Parameters.AddWithValue(Parameter.Key, Parameter.Value);



                    sqlCon.Open();
                    QueryResponse = sqlCMD.ExecuteNonQuery().ToString();
                }
                sqlCon.Close();
            }

            return QueryResponse;

        }
        public static DataTable SelectQuery(string query)
        {
            DataTable Datos = new DataTable();
            using (SqlConnection sqlConn = GetConnection())
            {
                using (SqlCommand sqlCMD = new SqlCommand(query, sqlConn))
                {
                    sqlConn.Open();
                    //SqlDataAdapter da = new SqlDataAdapter(sqlCMD);
                    Datos.Load(sqlCMD.ExecuteReader());
                    sqlConn.Close();
                }
            }
            return Datos;
        }
        public static int ExecuteQuery(string Query)
        {
            int QueryResponse = -1;
            using (SqlConnection sqlConn = GetConnection())
            {
                using (SqlCommand sqlCMD = new SqlCommand(Query, sqlConn))
                {
                    sqlConn.Open();
                    QueryResponse = sqlCMD.ExecuteNonQuery();
                }
                sqlConn.Close();
            }
            return QueryResponse;
        }
        public static IList<SelectListItem> GetAuthors()
        {
            IList<SelectListItem> Authors = (from DataRow Row in SelectQuery("select * from Authors").Rows
                                             select new SelectListItem
                                             {
                                                 Value = Convert.ToString((int)Row["ID"]),
                                                 Text = $"{(string)Row["FirstName"]} {(string)Row["LastName"]}"
                                             }).ToList();      
            return Authors;
        }
    }
}
