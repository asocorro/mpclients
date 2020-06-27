using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Category object for NHibernate mapped table Category.
    /// </summary>
    [Serializable]
    public class OrderStatus : DomainObject<System.Int32>
    {


        private System.String _Description;

        public OrderStatus()
        {
        }

        public OrderStatus(System.Int32 id)
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
