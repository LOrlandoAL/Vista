using Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class SupplierDAO
    {
        public List<Supplier> GetAll()
        {
            List<Supplier> lista = new List<Supplier>();
            //Conectarme
            if (Conexion.Conectar())
            {
                try
                {
                    //Crear la sentencia a ejecutar (SELECT)
                    String select = "SELECT supplierid, companyname FROM suppliers;";
                    //Definir un datatable para que sea llenado
                    DataTable dt = new DataTable();
                    //Crear el dataadapter
                    MySqlCommand sentencia = new MySqlCommand();
                    sentencia.CommandText=select;
                    sentencia.Connection = Conexion.conexion;
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = sentencia;
                    //Llenar el datatable
                    da.Fill(dt);
                    //Objeto Supplier auxiliar para asignar valores nulos
                    //lista.Add(new Supplier());
                    //Crear un objeto Supplier por cada fila de la tabla y añadirlo a la lista
                    foreach (DataRow fila in dt.Rows)
                    {
                        Supplier s = new Supplier(
                            //int.Parse(fila["Clave"].ToString())
                            Convert.ToInt32(fila["supplierid"]),
                            fila["companyname"].ToString()
                            );
                        lista.Add(s);
                    }

                    return lista;
                }
                finally
                {
                    Conexion.Desconectar();
                }
            }
            else
            {
                return null;
            }

        }
    }
}
