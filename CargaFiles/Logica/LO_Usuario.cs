using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using CargaFiles.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace CargaFiles.Logica
{
    public class LO_Usuario
    {
        string cadena = "Data Source=ZIBOR-64517;Initial Catalog=DBARCHIVOS;Integrated Security=true";

        public Usuarios EncontrarUsuario(string correo, string clave)
        {


            var Clave = encrip(clave);

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                // query que se va a mandar
                string query = "select Nombre,Apellido,Correo,Clave,IdRol from Usuarios where Correo = @pcorreo and Clave = @pclave";

                SqlCommand cmd = new SqlCommand(query, conexion);
                //llenamos los valores del query con los valores de entrada establecidos en el parametro
                cmd.Parameters.AddWithValue("@pcorreo", correo);
                cmd.Parameters.AddWithValue("@pclave", Clave);
                cmd.CommandType = CommandType.Text;
                conexion.Open();
                
                //los valores que traemos con el select
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Usuarios objeto = new Usuarios();
                    while (dr.Read())
                    {
                        //se llevan los atrubitos de la clase usuarios con lo que este en la bd
                        objeto = new Usuarios()
                        {
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            IdRol = (Rol)dr["IdRol"],

                        };
                    }
                    return objeto;
                }
            }
            

        }

      
        public Usuarios Guardar(Usuarios user)
        {
            var crypto = encrip(user.Clave);
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("insert into Usuarios(Nombre,Apellido,Correo,Clave,IdRol) values(@nombre,@apellido,@correo,@clave,@IdRol)", oconexion);
                //cmd.Parameters.AddWithValue("@nombre", name);
                cmd.Parameters.AddWithValue("@nombre", user.Nombre);
                cmd.Parameters.AddWithValue("@apellido", user.Apellido);
                cmd.Parameters.AddWithValue("@correo", user.Correo);
                cmd.Parameters.AddWithValue("@clave", crypto);
                cmd.Parameters.AddWithValue("@IdRol", 2);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return user;
            
        }


        public static string encrip(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;

            //byte[] bytes = Encoding.Unicode.GetBytes(text);
            //SHA256Managed hashstring = new SHA256Managed();
            //byte[] hash = hashstring.ComputeHash(bytes);
            //return BitConverter.ToString(hash);

            //string finalKey = string.Empty;
            //byte[] encode = new byte[text.Length];
            //encode = Encoding.UTF8.GetBytes(text);
            //finalKey = Convert.ToBase64String(encode);
            //return finalKey;
        }
    }
}