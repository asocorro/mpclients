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
using MPClients.PageControllers;
using NHibernate;
using MPClients.DataAccess.NHibernate;

namespace MPClients.Web
{
    public partial class NewsEdit : BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                if (!MetaSapiens.PageMethods.PageMethodsEngine.InvokeMethod(this, true))
                    throw new Exception("Page method not found!");

            }
        }
        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            base.OnInit(e);
        }

        void uxSave_Click(object sender, EventArgs e)
        {
            if (!uxFromDate.SelectedDate.HasValue
                || !uxToDate.SelectedDate.HasValue
                || String.IsNullOrEmpty(uxTitle.Text)
                || String.IsNullOrEmpty(uxDescriptionEditor.Content))
            {
                base.MasterPage.DisplayMessage("All Fields are required.");
            }
            else
            {

                Guid id = new Guid(uxID.Value);

                ISession session = UnitOfWork.Session;
                ITransaction transaction = session.BeginTransaction();
                MPClients.DataAccess.Domain.News news;
                news = session.Get<MPClients.DataAccess.Domain.News>(id);

                if (news == null)
                {
                    news = new MPClients.DataAccess.Domain.News(id);                  
                }
                news.FromDate = uxFromDate.SelectedDate.Value ;
                news.ToDate = uxToDate.SelectedDate.Value ;
                news.Title = uxTitle.Text;
                //news.Description = uxDescription.Value;
                news.Description = uxDescriptionEditor.Content;

                session.Save(news);
                transaction.Commit();
            }


        }

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void DoLoad(string id)
        {
            uxID.Value = id;

            MPClients.DataAccess.Domain.News news;
            news = UnitOfWork.Session.Get<MPClients.DataAccess.Domain.News>(new Guid(id));

            if (news == null)
            {
                base.MasterPage.DisplayMessage("Error Loading the News");
            }
            else
            {
                uxFromDate.SelectedDate = news.FromDate;
                uxToDate.SelectedDate = news.ToDate;
                uxTitle.Text = news.Title ;
                uxDescription.Value = news.Description;
                uxDescriptionEditor.Content = news.Description;
            }
        }

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void DoCreate()
        {
            uxID.Value = Guid.NewGuid().ToString();
        }
    }
}
