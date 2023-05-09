using Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ProductDAO
    {

        public List<Product> GetAll()
        {
            List<Product> lista = new List<Product>();
            //Conectarme
            if (Conexion.Conectar())
            {
                try
                {
                    //Crear la sentencia a ejecutar (SELECT)
                    String select = @"SELECT p.PRODUCTID, p.PRODUCTNAME, p.SUPPLIERID, s.COMPANYNAME, 
                        p.CATEGORYID, c.CATEGORYNAME, p.QUANTITYPERUNIT, p.UNITPRICE, p.UNITSINSTOCK, 
                        p.UNITSONORDER, p.REORDERLEVEL, p.DISCONTINUED 
                        FROM PRODUCTS p JOIN SUPPLIERS s
                        ON p.SUPPLIERID = s.SUPPLIERID
                        JOIN CATEGORIES c
                        ON p.CATEGORYID = c.CATEGORYID;";
                    /*String select = @"SELECT p.PRODUCTID, p.PRODUCTNAME, p.SUPPLIERID, 
                        p.CATEGORYID, p.QUANTITYPERUNIT, p.UNITPRICE, p.UNITSINSTOCK, 
                        p.UNITSONORDER, p.REORDERLEVEL, p.DISCONTINUED 
                        FROM PRODUCTS p;";*/
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
                    //Crear un objeto categoría por cada fila de la tabla y añadirlo a la lista
                    foreach (DataRow fila in dt.Rows)
                    {
                        Product producto = new Product(
                            //int.Parse(fila["Clave"].ToString())
                            Convert.ToInt32(fila["PRODUCTID"]),
                            fila["PRODUCTNAME"].ToString(),
                            Convert.ToInt32(fila["SUPPLIERID"]),
                            fila["COMPANYNAME"].ToString(),
                            Convert.ToInt32(fila["CATEGORYID"]),
                            fila["CATEGORYNAME"].ToString(),
                            fila["QUANTITYPERUNIT"].ToString(),
                            Convert.ToDouble(fila["UNITPRICE"]),
                            Convert.ToInt32(fila["UNITSINSTOCK"]),
                            Convert.ToInt32(fila["UNITSONORDER"]),
                            Convert.ToInt32(fila["REORDERLEVEL"]),
                            Convert.ToBoolean(fila["DISCONTINUED"])
                            );
                        lista.Add(producto);
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

        public List<Product> GetOrders()
        {
            List<Product> lista = new List<Product>();
            //Conectarme
            if (Conexion.Conectar())
            {
                try
                {
                    //Crear la sentencia a ejecutar (SELECT)
                    String select = @"select products.productName, 
                                    suppliers.Companyname, 
                                    if(products.ReorderLevel > 
                                    products.UnitsInStock, 
                                    products.ReorderLevel - products.UnitsInStock, 0) as Unidades 
                                    from products
                                    JOIN SUPPLIERS ON products.supplierid = suppliers.supplierId
                                    where discontinued = false;";
                    /*String select = @"SELECT p.PRODUCTID, p.PRODUCTNAME, p.SUPPLIERID, 
                        p.CATEGORYID, p.QUANTITYPERUNIT, p.UNITPRICE, p.UNITSINSTOCK, 
                        p.UNITSONORDER, p.REORDERLEVEL, p.DISCONTINUED 
                        FROM PRODUCTS p;";*/
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
                    //Crear un objeto categoría por cada fila de la tabla y añadirlo a la lista
                    foreach (DataRow fila in dt.Rows)
                    {
                        Product producto = new Product(
                            //int.Parse(fila["Clave"].ToString())
                            0,
                            fila["PRODUCTNAME"].ToString(),
                            0,
                            fila["COMPANYNAME"].ToString(),
                            0,
                            "",
                            "",
                            Convert.ToDouble(fila["Unidades"]),//UnitPrice
                            Convert.ToInt32(fila["UNITSINSTOCK"]),
                            0,
                            Convert.ToInt32(fila["REORDERLEVEL"]),
                            false
                            );
                        lista.Add(producto);
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

        //-------------------------------------------- AGREGAR -----------------------------------------------------

        public int Insert(Product producto)
        {
            //Conectarme
            if (Conexion.Conectar())
            {
                try
                {
                    //Crear la sentencia a ejecutar (INSERT)
                    String select = @"INSERT INTO PRODUCTS (PRODUCTNAME,SUPPLIERID,
                                        CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK, 
                                        UNITSONORDER,REORDERLEVEL,DISCONTINUED) 
                                    VALUES(
                                        @productname,   
                                        @supplierid,
                                        @categoryid,    
                                        @quantityperunit,   
                                        @unitprice,
                                        @unitsinstock,
                                        @unitsonorder,
                                        @reorderlevel,
                                        @discontinued ); 
                                    select last_insert_id();";

                    // Se modifico la manera de crear la sentencia de ejecución porque por alguna razón
                    // generaba una excepción al signar nulos
                    MySqlCommand sentencia = new MySqlCommand(select, Conexion.conexion);
                    //sentencia.CommandText = select;

                    sentencia.Parameters.AddWithValue("@productname", producto.ProductName);
                    if (producto.SupplierID == -1) sentencia.Parameters.AddWithValue("@supplierid", null);
                    else sentencia.Parameters.AddWithValue("@supplierid", producto.SupplierID);
                    if (producto.CategoryID == -1) sentencia.Parameters.AddWithValue("@categoryid", null);
                    else sentencia.Parameters.AddWithValue("@categoryid", producto.CategoryID);
                    //sentencia.Parameters.AddWithValue("@supplierid", producto.SupplierID);
                    //sentencia.Parameters.AddWithValue("@categoryid", producto.CategoryID);
                    sentencia.Parameters.AddWithValue("@quantityperunit", producto.QuantityPerUnit);
                    sentencia.Parameters.AddWithValue("@unitprice", producto.UnitPrice);
                    sentencia.Parameters.AddWithValue("@unitsinstock", producto.UnitsInStock);
                    sentencia.Parameters.AddWithValue("@unitsonorder", producto.UnitsOnOrder);
                    sentencia.Parameters.AddWithValue("@reorderlevel", producto.ReorderLevel);
                    sentencia.Parameters.AddWithValue("@discontinued", producto.Discontinued);
                    sentencia.Connection = Conexion.conexion;

                    //Ejercutar el comando 
                    //Cuando nos interesa obtener un valor adicional en el comando (como en el ejemplo de arriba que obtiene el último id generado por autoincrement podemos usar ExecuteScalar
                    int claveNuevoProducto = Convert.ToInt32(sentencia.ExecuteScalar());

                    //O de lo contrario podríamos usar ExecuteNonQuery que simplemente ejecuta la sentencia y nos permite recuperar (solo si nos interesa) el número de filas afectadas (si es un insert nos regresa cuantas filas agregó, en un update cuantas filas editó y en un delete igual cuantas filas eliminó, por ejemplo:
                    //int filasAfectadas = Convert.ToInt32(sentencia.ExecuteNonQuery());


                    return claveNuevoProducto;
                }
                finally
                {
                    Conexion.Desconectar();
                }
            }
            else
            {
                //Devolvemos un cero indicando que no se insertó nada
                return 0;
            }
        }
        public int Update(Product producto)
        {
            //Conectarme
            if (Conexion.Conectar())
            {
                try
                {
                    //Crear la sentencia a ejecutar (UPDATE)
                    String select = @"UPDATE PRODUCTS SET 
                                        PRODUCTNAME=@productname, 
                                        SUPPLIERID=@supplierid, 
                                        CATEGORYID=@categoryid,
                                        QUANTITYPERUNIT=@quantityperunit,   
                                        UNITPRICE=@unitprice, 
                                        UNITSINSTOCK=@unitsinstock,
                                        UNITSONORDER=@unitsonorder,
                                        REORDERLEVEL=@reorderlevel,
                                        DISCONTINUED=@discontinued
                                    where PRODUCTID = @productid;";

                    MySqlCommand sentencia = new MySqlCommand(select, Conexion.conexion);
                    //sentencia.CommandText = select;
                    sentencia.Parameters.AddWithValue("@productid", producto.ProductID);
                    sentencia.Parameters.AddWithValue("@productname", producto.ProductName);
                    if(producto.SupplierID == 0) sentencia.Parameters.AddWithValue("@supplierid", null);
                    else sentencia.Parameters.AddWithValue("@supplierid", producto.SupplierID);
                    if (producto.CategoryID == 0) sentencia.Parameters.AddWithValue("@categoryid", null);
                    else sentencia.Parameters.AddWithValue("@categoryid", producto.CategoryID);
                    //sentencia.Parameters.AddWithValue("@categoryid", producto.CategoryID);
                    sentencia.Parameters.AddWithValue("@quantityperunit", producto.QuantityPerUnit);
                    sentencia.Parameters.AddWithValue("@unitprice", producto.UnitPrice);
                    sentencia.Parameters.AddWithValue("@unitsinstock", producto.UnitsInStock);
                    sentencia.Parameters.AddWithValue("@unitsonorder", producto.UnitsOnOrder);
                    sentencia.Parameters.AddWithValue("@reorderlevel", producto.ReorderLevel);
                    sentencia.Parameters.AddWithValue("@discontinued", producto.Discontinued);

                    //Ejercutar el comando 
                    //Cuando nos interesa obtener un valor adicional en el comando (como en el ejemplo de arriba que obtiene el último id generado por autoincrement podemos usar ExecuteScalar
                    int claveNuevoProducto = Convert.ToInt32(sentencia.ExecuteScalar());

                    //O de lo contrario podríamos usar ExecuteNonQuery que simplemente ejecuta la sentencia y nos permite recuperar (solo si nos interesa) el número de filas afectadas (si es un insert nos regresa cuantas filas agregó, en un update cuantas filas editó y en un delete igual cuantas filas eliminó, por ejemplo:
                    //int filasAfectadas = Convert.ToInt32(sentencia.ExecuteNonQuery());


                    return claveNuevoProducto;
                }
                finally
                {
                    Conexion.Desconectar();
                }
            }
            else
            {
                //Devolvemos un cero indicando que no se insertó nada
                return 0;
            }
        }

        public int Delete(int productid)
        {
            //Conectarme
            if (Conexion.Conectar())
            {
                try
                {
                    //Crear la sentencia a ejecutar (UPDATE)
                    String select = @"delete from products
                        where productid = @productid;";

                    MySqlCommand sentencia = new MySqlCommand(select, Conexion.conexion);
                    //sentencia.CommandText = select;
                    sentencia.Parameters.AddWithValue("@productid", productid);

                    //Ejercutar el comando 
                    //Cuando nos interesa obtener un valor adicional en el comando (como en el ejemplo de arriba que obtiene el último id generado por autoincrement podemos usar ExecuteScalar
                    //int claveNuevoProducto = Convert.ToInt32(sentencia.ExecuteScalar());

                    //O de lo contrario podríamos usar ExecuteNonQuery que simplemente ejecuta la sentencia y nos permite recuperar (solo si nos interesa) el número de filas afectadas (si es un insert nos regresa cuantas filas agregó, en un update cuantas filas editó y en un delete igual cuantas filas eliminó, por ejemplo:
                    int filasAfectadas = Convert.ToInt32(sentencia.ExecuteNonQuery());
                }

                catch (MySqlException e)
                {
                    return (int)e.Code;
                }

                finally
                {

                    Conexion.Desconectar();
                }
                return 0;
            }
            else
            {
                //Devolvemos un cero indicando que no se insertó nada
                return 0;
            }
        }


    }
}
