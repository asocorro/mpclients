using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Category object for NHibernate mapped table Category.
    /// </summary>
    [Serializable]
    public class Category : DomainObject<System.String>
    {


        private System.String _Description;

        public Category()
        {
        }

        public Category(System.String id)
        {
            base.ID = id;
        }

         public virtual System.String Description {
             get { return _Description; }
             set { _Description = value;}
         }
 


        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
