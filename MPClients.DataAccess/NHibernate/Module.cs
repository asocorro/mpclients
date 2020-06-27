using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MPClients.DataAccess.NHibernate
{
    /// <summary> 
    ///        HttpModule to automatically close a session at the end of a request 
    /// </summary> 
    public sealed class Module : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(EndRequest);
        }

        public void Dispose() { }

        public void EndRequest(Object sender, EventArgs e)
        {
            UnitOfWork.CloseSession();
        }
    } 
}
