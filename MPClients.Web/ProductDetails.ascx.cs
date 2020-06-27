using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MPClients.DataAccess.NHibernate;
using MPClients.DataAccess.Domain;

namespace MPClients.Web
{
    public partial class ProductDetails : System.Web.UI.UserControl
    {
        public string ProductID
        {
            get
            {
                if (ViewState["ProductID"] == null)
                {
                    return "";
                }
                return (string)ViewState["ProductID"];
            }
            set
            {
                if (this.ProductID != value)
                {

                }
                ViewState["ProductID"] = value;
                DescriptionExt.Text = value;



                //ConfigureView();
            }
        }

        public void LoadData(string data)
        {
            string[] value = data.Split(';');
            DescriptionExt.Text = value[1];
            Description3.Text = value[0];
            Category.Text = value[2];
        }

        //private void ConfigureView()
        //{
        //    Product product = UnitOfWork.GetIsolatedSession().Get<Product>(ProductID);
        //    try
        //    {
        //        //Category category = UnitOfWork.GetIsolatedSession().Get<Category>(product.ProductCategory);
        //        //ID.Text = ProductID;
        //        // ProductName.Text = product.Name;
        //        DescriptionExt.Text = product.DescriptionExt;
        //        Description3.Text = product.Description3;
        //        //ProductName.Text = product.Name;
        //        //StockquantityText.Text = product.StockquantityText;
        //        //Price.Text = product.Price.ToString("c");
        //        //Category.Text = category.ID + "-"+ category.Description ;
        //        Category.Text = "abs";

        //    }
        //    catch
        //    {

        //    }
        //}
    }
}