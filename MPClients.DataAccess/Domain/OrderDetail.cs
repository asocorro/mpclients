using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// OrderDetail object for NHibernate mapped table OrderDetail.
    /// </summary>
    [Serializable]
    public class OrderDetail : DomainObject<System.Guid>
    {

        private System.Guid _OrderID;
        private System.Int32 _Quantity;
        private System.String _ProductID;
        private System.String _ShortDescription;
        private System.String _ProductName;
        private System.Boolean  _IsFriend;
        private System.Decimal? _NetPrice;
        private Orders _Orders;
        private Product _Product;

        public OrderDetail()
        {
            _IsFriend = false;
        }

        public OrderDetail(System.Guid id)
        {
            base.ID = id;
            _IsFriend = false;
        }

         public virtual System.Guid OrderID {
             get { return _OrderID; }
             set { _OrderID = value;}
         }

         public virtual System.Int32 Quantity {
             get { return _Quantity; }
             set { _Quantity = value;}
         }

         public virtual System.String ProductID {
             get { return _ProductID; }
             set { _ProductID = value;}
         }

         public virtual System.String ShortDescription {
             get { return _ShortDescription; }
             set { _ShortDescription = value;}
         }

         public virtual System.String ProductName {
             get { return _ProductName; }
             set { _ProductName = value;}
         }

         public virtual System.Decimal? NetPrice {
             get { return _NetPrice; }
             set { _NetPrice = value;}
         }

         public virtual System.Boolean IsFriend
         {
             get { return _IsFriend; }
             set { _IsFriend = value; }
         }
        public virtual System.Decimal ExtendedPrice
        {
            get { return _NetPrice.Value  * _Quantity; }
        }

        public virtual Product Product
        {
            get { return _Product; }
            set { _Product = value; }
        }

        public virtual Orders Orders
        {
            get { return _Orders; }
            set { _Orders = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
