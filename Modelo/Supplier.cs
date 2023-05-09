using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }

        public Supplier(int supplierID, string companyName)
        {
            SupplierID = supplierID;
            CompanyName = companyName;
        }
        public Supplier()
        {
            SupplierID = 0;
            CompanyName = "";
        }
    }
}
