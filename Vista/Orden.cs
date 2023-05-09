using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class Orden : Form
    {
        public static List<Product> products;

        public Orden()
        {
            InitializeComponent();
            Conexion con = new Conexion();

            products = new ProductDAO().GetOrders();

            dgvProduct.DataSource = products;
            dgvProduct.AllowUserToAddRows = false;
            dgvProduct.AllowUserToDeleteRows = false;
            dgvProduct.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvProduct.Columns["ProductName"].HeaderText = "Producto";
            dgvProduct.Columns["CompanyName"].HeaderText = "Proveedor";
            //dgvProduct.Columns["Unidades"].HeaderText = "Unidades";

        }
    }
}
