using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Category
    {
        public int CategoryID { get; set; }
        public String CategoryName { get; set; }
        public String CategoryDescription { get; set; }

        public Category(int categoryid, String categoryName, String description) {
            CategoryID= categoryid;
            CategoryName= categoryName;
            CategoryDescription= description;
        }

        public Category()
        {
            CategoryID = 0;
            CategoryName = "";
            CategoryDescription = "";
        }

    }
}
