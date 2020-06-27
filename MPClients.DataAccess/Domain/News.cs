using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// New object for NHibernate mapped table News.
    /// </summary>
    [Serializable]
    public class News : DomainObject<System.Guid>
    {


        private System.DateTime _FromDate;
        private System.DateTime _ToDate;
        private System.String  _Title;
        private System.String _Description;

        public News()
        {
        }

        public News(System.Guid id)
        {
            base.ID = id;
        }

         public virtual System.DateTime FromDate {
             get { return _FromDate; }
             set { _FromDate = value;}
         }

         public virtual System.DateTime ToDate {
             get { return _ToDate; }
             set { _ToDate = value;}
         }

        public virtual System.String Title
        {
             get { return _Title; }
             set { _Title = value;}
         }
        public virtual System.String Description
         {
             get { return _Description; }
             set { _Description = value; }
         }



        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
