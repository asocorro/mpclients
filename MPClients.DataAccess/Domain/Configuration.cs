using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// New object for NHibernate mapped table News.
    /// </summary>
    [Serializable]
    public class Configuration : DomainObject<System.Guid>
    {



        private System.Int32 _MaxOrderProductsForPickup;
        private System.Int32 _RestrictOrderProductsForPickup;

        public Configuration()
        {
        }

        public Configuration(System.Guid id)
        {
            base.ID = id;
        }
         
        public virtual System.Int32 RestrictOrderProductsForPickup
        {
             get { return _RestrictOrderProductsForPickup; }
             set { _RestrictOrderProductsForPickup = value; }
        }


        public virtual System.Int32 MaxOrderProductsForPickup
        {
            get { return _MaxOrderProductsForPickup; }
            set { _MaxOrderProductsForPickup = value; }
        }
 

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
