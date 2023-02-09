using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using CargaFiles.Models;
using System.Data.SqlClient;
using System.Data;

namespace CargaFiles.Logica
{
    public class LO_Usuario
    {
        public Usuarios EncontrarUsuario(string correo, string clave)
        {

            Usuarios objeto = new Usuarios();


            using (SqlConnection conexion = new SqlConnection("Data Source=ZIBOR-64517;Initial Catalog=DBARCHIVOS;Integrated Security=true"))
            {
                // query que se va a mandar
                string query = "select Nombres,Correo,Clave,IdRol from USUARIOS where Correo = @pcorreo and Clave = @pclave";

                SqlCommand cmd = new SqlCommand(query, conexion);
                //llenamos los valores del query con los valores de entrada establecidos en el parametro
                cmd.Parameters.AddWithValue("@pcorreo", correo);
                cmd.Parameters.AddWithValue("@pclave", clave);
                cmd.CommandType = CommandType.Text;


                conexion.Open();

                //los valores que traemos con el select
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        //se llevan los atrubitos de la clase usuarios con lo que este en la bd
                        objeto = new Usuarios()
                        {
                            Nombres = dr["Nombres"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            IdRol = (Rol)dr["IdRol"],

                        };
                    }

                }
            }
            return objeto;

        }
    }
}