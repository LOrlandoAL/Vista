using System;
using System.Windows.Forms;
using Datos;
using Modelo;

namespace Vista
{
    public partial class AgregarEditar : Form
    {
        private Product product;
        public AgregarEditar(Product p)
        {
            InitializeComponent();
            this.product = p;
            LoadControls();
        }
        /// <summary>
        /// Carga los datos del objeto seleccionado para su edición
        /// </summary>
        private void LoadControls()
        {
            cboCategories.DataSource = App.categories;
            cboSupplier.DataSource = App.suppliers;
            cboCategories.DisplayMember = "CategoryName";
            cboCategories.SelectedIndex = product.CategoryID;
            cboSupplier.DisplayMember = "CompanyName";
            cboSupplier.SelectedIndex = product.SupplierID;
            txtName.Text = product.ProductName;
            txtQuantityPerUnit.Text = product.QuantityPerUnit;
            txtUnitPrice.Text = product.ProductID == 0 ? "" : product.UnitPrice.ToString();
            txtUnitsInStock.Text = product.ProductID == 0? "" : product.UnitsInStock.ToString();
            txtUnitsOnOrder.Text = product.ProductID == 0 ? "" : product.UnitsOnOrder.ToString();
            txtReorderLevel.Text = product.ProductID == 0 ? "" : product.ReorderLevel.ToString();
            chbDiscontinued.Checked = product.Discontinued;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Equals("") || cboSupplier.SelectedIndex < 1) { 
                MessageBox.Show("Por favor ingrese el nombre del producto");
                return;
            }
            if (cboCategories.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor indique la categoría del producto");
                return;
            }
            if (cboSupplier.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor indique el proveedor del producto");
                return;
            }

            ProductDAO productDAO = new ProductDAO();
            product.ProductName = txtName.Text;
            product.CategoryID = ((Category)cboCategories.SelectedItem).CategoryID;
            product.SupplierID = ((Supplier)cboSupplier.SelectedItem).SupplierID;
            product.QuantityPerUnit = txtQuantityPerUnit.Text;
            product.UnitPrice = Convert.ToInt32(txtUnitPrice.Text);
            product.UnitsInStock = Convert.ToInt32(txtUnitsInStock.Text);
            product.UnitsOnOrder = Convert.ToInt32(txtUnitsOnOrder.Text);
            product.ReorderLevel = Convert.ToInt32(txtReorderLevel.Text);
            product.Discontinued = chbDiscontinued.Checked;

            if (product.ProductID == 0) productDAO.Insert(product);
            else productDAO.Update(product);
            this.Dispose();
        }

    }
}
