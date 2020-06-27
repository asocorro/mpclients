using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Category object for NHibernate mapped table Category.
    /// </summary>
    [Serializable]
    public class SubCategory : DomainObject<System.String>
    {

        private System.String _CategoryID;
        private System.String _SubCategoryName;

        public SubCategory()
        {
        }

        public SubCategory(System.String id)
        {
            base.ID = id;
        }
        public virtual System.String CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        public virtual System.String SubCategoryName
        {
             get { return _SubCategoryName; }
             set { _SubCategoryName = value;}
        }
 


        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
