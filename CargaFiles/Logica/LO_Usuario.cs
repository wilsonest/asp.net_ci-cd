using CargaFiles.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Configuration;

namespace CargaFiles.Logica
{
    public class Lo_Usuario
    {
        //string cadena = "Data Source=ZIBOR-64517;Initial Catalog=DBARCHIVOS;Integrated Security=true";  cadena vieja ya se mejoro en el web.config
        string cadena = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;


        public List<Products> GetAllProducts()
        {
            List<Products> listaProductos = new List<Products>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select * from Productos", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Products archivoE = new Products();
                        archivoE.IdProducto = Convert.ToInt32(dr["IdProducto"]);
                        archivoE.Nombre = dr["Nombre"].ToString();
                        archivoE.Descripcion = dr["Descripcion"].ToString();
                        archivoE.Id = Convert.ToInt32(dr["Id"]);
                        listaProductos.Add(archivoE);
                    }
                }
            }
            return listaProductos;
        }

        public Usuarios EncontrarUsuario(string correo, string clave)
        {


            var Clave = encrip(clave);
            //var Clave = EncryptStringAES(clave);

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                // query que se va a mandar
                string query = "select * from Usuarios where Correo = @pcorreo and Clave = @pclave";

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
                        //var clavedes = DecryptStringFromBytes_Aes(Convert.FromBase64String(dr["Clave"].ToString()));  //prueba para zibor
                        //se llevan los atrubitos de la clase usuarios con lo que este en la bd
                        objeto = new Usuarios()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            //Clave = clavedes,
                            IdRol = (Rol)dr["IdRol"],

                        };
                    }
                    return objeto;
                }
            }


        }

        public Products GetProductos(int a)
        {

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                //string query = "SELECT TOP 1 * FROM Productos INNER JOIN Usuarios ON Productos.Id = Usuarios.Id WHERE Usuarios.Id ="+ a +"ORDER BY Productos.IdProducto DESC";
                string query = "SELECT TOP 1 * FROM Productos INNER JOIN Usuarios ON Productos.Id = Usuarios.Id WHERE Usuarios.Id = @id ORDER BY Productos.IdProducto DESC";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", a);
                cmd.CommandType = CommandType.Text;
                conexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Products objeto = new Products();
                    while (dr.Read())
                    {
                        //se llevan los atrubitos de la clase usuarios con lo que este en la bd
                        objeto = new Products()
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            Id = Convert.ToInt32(dr["Id"]),
                        };
                    }
                    return objeto;
                }
            }


        }

        public DataTable GetImagenes()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string query = "select * from Fotos";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                conexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Add columns to the DataTable
                    dt.Columns.Add("Id", typeof(int));
                    dt.Columns.Add("NombreImg", typeof(string));
                    dt.Columns.Add("Imagen", typeof(byte[]));
                    dt.Columns.Add("IdProducto", typeof(int));

                    // Loop through the SqlDataReader and add rows to the DataTable
                    while (dr.Read())
                    {
                        DataRow row = dt.NewRow();
                        row["Id"] = Convert.ToInt32(dr["Id"]);
                        row["NombreImg"] = dr["NombreImg"].ToString();
                        row["Imagen"] = (byte[])dr["Imagen"];
                        row["IdProducto"] = Convert.ToInt32(dr["IdProducto"]);
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

        public Usuarios Guardar(Usuarios user)
        {
            var crypto = encrip(user.Clave);
            //var crypto = EncryptStringAES(user.Clave);

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

        public Products CrearProducto(Products prod, int a)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("insert into Productos(Nombre,Descripcion,Id) values(@nombre,@descripcion,@id)", oconexion);
                cmd.Parameters.AddWithValue("@nombre", prod.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", prod.Descripcion);
                cmd.Parameters.AddWithValue("@id", a);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return prod;

        }

        public void SubirImagen(int idProducto, List<HttpPostedFileBase> archivos)
        {
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                oconexion.Open();

                foreach (var archivo in archivos)
                {
                    if (archivo != null)
                    {
                        string nombre = Path.GetFileName(archivo.FileName);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            archivo.InputStream.CopyTo(ms);
                            byte[] imagen = ms.ToArray();
                            SqlCommand cmd = new SqlCommand("insert into Fotos(NombreImg,Imagen,IdProducto) values(@nombreimg,@imagen,@idproducto)", oconexion);
                            cmd.Parameters.AddWithValue("@nombreimg", nombre);
                            cmd.Parameters.AddWithValue("@imagen", imagen);
                            cmd.Parameters.AddWithValue("@idproducto", idProducto);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
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

        public static string EncryptStringAES(string plainText)
        {
            var Clave_AES_Vector = "3Mi9RqPe2vd5npXAFNWj5w==";
            byte[] IV = Convert.FromBase64String(Clave_AES_Vector);
            byte[] Key = Convert.FromBase64String("u8bjuHDfPs3zDv57doBIgiio28coN2LH");

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }



        string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            var Clave_AES_Vector = "3Mi9RqPe2vd5npXAFNWj5w==";
            byte[] IV = Convert.FromBase64String(Clave_AES_Vector);
            byte[] Key = Convert.FromBase64String("u8bjuHDfPs3zDv57doBIgiio28coN2LH");
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

    }
}