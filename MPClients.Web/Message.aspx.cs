using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MPClients
{
    public partial class Message : MPClients.PageControllers.BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                if (!MetaSapiens.PageMethods.PageMethodsEngine.InvokeMethod(this, true))
                    throw new Exception("Page method not found!");

            }
        }
        [MetaSapiens.PageMethods.PageMethod(true)]
        public void MsgLoad(string message)
        {
            uxMessage.Text = message;
        }

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void LoadById(int messageId)
        {
            switch (messageId)
            {
                case 1:
                    uxMessage.Text = "Your Shopping Cart is Empty";
                    break;
                default:
                    uxMessage.Text = "";
                    break;
            }
        }
    }
}
