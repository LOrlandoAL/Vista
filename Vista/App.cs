using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos;
using Modelo;

namespace Vista
{
    public partial class App : Form
    {
        public static List<Category> categories;
        public static List<Product> products;
        public static List<Supplier> suppliers;

        private AgregarEditar agregarEditar;

        public App()
        {
            InitializeComponent();
            Conexion con = new Conexion();

            categories = new CategoryDAO().GetAll();
            products = new ProductDAO().GetAll();
            suppliers = new SupplierDAO().GetAll();

            dgvProduct.DataSource = products;
            dgvProduct.AllowUserToAddRows = false;
            dgvProduct.AllowUserToDeleteRows = false;
            dgvProduct.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvProduct.Columns["ProductName"].HeaderText = "Producto";
            dgvProduct.Columns["CompanyName"].HeaderText = "Proveedor";
            dgvProduct.Columns["CategoryName"].HeaderText = "Categoría";
            dgvProduct.Columns["QuantityPerUnit"].HeaderText = "Cantidad por Unidad";
            dgvProduct.Columns["UnitPrice"].HeaderText = "Precio Unitario";
            dgvProduct.Columns["UnitsInStock"].HeaderText = "Unidades en Stock";
            dgvProduct.Columns["UnitsOnOrder"].HeaderText = "Unidades por Encargo";
            dgvProduct.Columns["ReorderLevel"].HeaderText = "Nivel de Reorden";
            dgvProduct.Columns["Discontinued"].HeaderText = "Descontinuado";

            dgvProduct.Columns["CategoryID"].Visible = false ;
            dgvProduct.Columns["SupplierID"].Visible = false;
            dgvProduct.Columns["ProductID"].Visible = false;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            agregarEditar = new AgregarEditar(new Product());
            agregarEditar.FormClosed += new FormClosedEventHandler(AgregarEditarFormClosed);
            agregarEditar.ShowDialog();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                int index = dgvProduct.CurrentRow.Index;
                Product product = products[index];
                agregarEditar = new AgregarEditar(product);
                agregarEditar.FormClosed += new FormClosedEventHandler(AgregarEditarFormClosed);
                agregarEditar.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione el registro que desea editar.");
            }
        }

        private void AgregarEditarFormClosed(object sender, FormClosedEventArgs e)
        {
            Conexion con = new Conexion();

            categories = new CategoryDAO().GetAll();
            products = new ProductDAO().GetAll();
            suppliers = new SupplierDAO().GetAll();

            dgvProduct.DataSource = products;
            dgvProduct.AllowUserToAddRows = false;
            dgvProduct.AllowUserToDeleteRows = false;
            dgvProduct.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvProduct.Columns["ProductName"].HeaderText = "Producto";
            dgvProduct.Columns["CompanyName"].HeaderText = "Proveedor";
            dgvProduct.Columns["CategoryName"].HeaderText = "Categoría";
            dgvProduct.Columns["QuantityPerUnit"].HeaderText = "Cantidad por Unidad";
            dgvProduct.Columns["UnitPrice"].HeaderText = "Precio Unitario";
            dgvProduct.Columns["UnitsInStock"].HeaderText = "Unidades en Stock";
            dgvProduct.Columns["UnitsOnOrder"].HeaderText = "Unidades por Encargo";
            dgvProduct.Columns["ReorderLevel"].HeaderText = "Nivel de Reorden";
            dgvProduct.Columns["Discontinued"].HeaderText = "Descontinuado";

            dgvProduct.Columns["CategoryID"].Visible = false;
            dgvProduct.Columns["SupplierID"].Visible = false;
            dgvProduct.Columns["ProductID"].Visible = false;
        }
        private void ActualizarTabla()
        {
            Conexion con = new Conexion();

            categories = new CategoryDAO().GetAll();
            products = new ProductDAO().GetAll();
            suppliers = new SupplierDAO().GetAll();

            dgvProduct.DataSource = products;
            dgvProduct.AllowUserToAddRows = false;
            dgvProduct.AllowUserToDeleteRows = false;
            dgvProduct.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvProduct.Columns["ProductName"].HeaderText = "Producto";
            dgvProduct.Columns["CompanyName"].HeaderText = "Proveedor";
            dgvProduct.Columns["CategoryName"].HeaderText = "Categoría";
            dgvProduct.Columns["QuantityPerUnit"].HeaderText = "Cantidad por Unidad";
            dgvProduct.Columns["UnitPrice"].HeaderText = "Precio Unitario";
            dgvProduct.Columns["UnitsInStock"].HeaderText = "Unidades en Stock";
            dgvProduct.Columns["UnitsOnOrder"].HeaderText = "Unidades por Encargo";
            dgvProduct.Columns["ReorderLevel"].HeaderText = "Nivel de Reorden";
            dgvProduct.Columns["Discontinued"].HeaderText = "Descontinuado";

            dgvProduct.Columns["CategoryID"].Visible = false;
            dgvProduct.Columns["SupplierID"].Visible = false;
            dgvProduct.Columns["ProductID"].Visible = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                int index = dgvProduct.CurrentRow.Index;
                Product product = products[index];
                DialogResult d = MessageBox.Show("Estas seguro de lo que vas a hacer" + product.ProductName, "SI O NO ", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    ProductDAO productsDAO = new ProductDAO();
                    int codigo = new ProductDAO().Delete(index);
                    String nombre = product.ProductName;
                    if( codigo != 0)
                    {
                        MessageBox.Show("Se ha eliminado correctamente el producto " +nombre);
                    } 
                    ActualizarTabla();
                }
                else 
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Seleccione el registro que desea editar.");
            }
        }
    }
}
