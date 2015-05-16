using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace ProductStorageApp
{
    public class Products
    {
        public int code;
        public string description;
        public int quantity;
    }

    //class ConnectionClass
    //{
    //    public static SqlConnection GetProductDBConnecton()
    //    {
    //        string conn=
    //    }
    //}
}
