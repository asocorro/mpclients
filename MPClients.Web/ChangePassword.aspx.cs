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
using MPClients.DataAccess.NHibernate;
using NHibernate;
using MPClients.PageControllers;

namespace MPClients
{
    public partial class ChangePassword : BasePage
    {


        protected override void PageLoad()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            uxChangePassword.ChangedPassword += new EventHandler(uxChangePassword_ChangedPassword);
            uxChangePassword.CancelButtonClick += new EventHandler(uxChangePassword_CancelButtonClick);
            base.OnInit(e);
        }

        void uxChangePassword_CancelButtonClick(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        void uxChangePassword_ChangedPassword(object sender, EventArgs e)
        {
            MembershipUser CurrentUser = Membership.GetUser(uxChangePassword.UserName);

            ISession session = MPClients.DataAccess.NHibernate.UnitOfWork.Session;
            ITransaction transaction = session.BeginTransaction();

            MPClients.DataAccess.Domain.MembershipUsers membershipUser;
            membershipUser = session.Get<MPClients.DataAccess.Domain.MembershipUsers>(new Guid(CurrentUser.ProviderUserKey.ToString()));

            membershipUser.ChangePassword = false;
            membershipUser.LastActivityDate = DateTime.Now;
            membershipUser.LastPriceQueryDate = DateTime.Now;
            session.Save(membershipUser);
            transaction.Commit();


        }

    }
}
