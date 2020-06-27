using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Product object for NHibernate mapped table Product.
    /// </summary>
    [Serializable]
    public class Product : DomainObject<System.String>
    {


        private System.String _Name;
        private System.String _Category;
        private System.String _Description1;
        private System.String _Description2;
        private System.String _Description3;
        private System.Int32 _Stockquantity;
        private System.Decimal _Price;
        private System.String _DescriptionExt; 

     
        private System.Int32 _QuantityDesired;

        public Product()
        {
        }

        public Product(System.String id)
        {
            base.ID = id;
        }

         public virtual System.String Name {
             get { return _Name; }
             set { _Name = value;}
         }

        public virtual System.String ProductCategory
        {
             get { return _Category; }
             set { _Category = value;}
         }

         public virtual System.String Description1 {
             get { return _Description1; }
             set { _Description1 = value;}
         }

         public virtual System.String Description2 {
             get { return _Description2; }
             set { _Description2 = value;}
         }

         public virtual System.String Description3 {
             get { return _Description3; }
             set { _Description3 = value;}
         }

         public virtual System.Int32 Stockquantity {
             get { return _Stockquantity; }
             set { _Stockquantity = value;}
         }

        public virtual System.Int32 QuantityDesired
        {
            get { return _QuantityDesired; }
            set { _QuantityDesired = value; }
        }
         public virtual System.Decimal Price {
             get { return _Price; }
             set { _Price = value;}
         }

        public virtual System.String DescriptionExt
        {
            get
            {
                return _DescriptionExt == "" ? "-" : _DescriptionExt;
            }
            set { _DescriptionExt = value; }
        }

        public virtual System.String StockquantityText
        {
            get { return _Stockquantity > 10 ? "Over 10" : Stockquantity.ToString(); }
        }


        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }

    [Serializable]
    public class ProductFriends : DomainObject<System.Int32>
    {

        private System.String _ProductID;
        private System.String _FriendID;

        public ProductFriends()
        {
        }

        public ProductFriends(System.Int32 id)
        {
            base.ID = id;
        }

        public virtual System.String ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }

        public virtual System.String FriendID
        {
            get { return _FriendID; }
            set { _FriendID = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }

}
