using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Product object for NHibernate mapped table Product.
    /// </summary>
    [Serializable]
    public class VWProducts : DomainObject<System.String>
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
        private System.Decimal _PriceMultiplier;
        private System.Decimal _ClientProductPrice;
        private System.String _ClientID;
        private System.String _FromQuery;
        private System.String _Product_ID1;
        private System.String _AddToCart;
        private System.String _CategoryDescription;
        private System.String _MoreInfoURL;

        private System.String _NameClean;
        private System.String _Description1Clean;
        private System.String _DescriptionExtClean;

        public virtual System.String FromQuery
        {
            get { return _FromQuery; }
            set { _FromQuery = value; }
        }

        public virtual System.String NameClean
        {
            get { return _NameClean; }
            set { _NameClean = value; }
        }

        public virtual System.String Description1Clean
        {
            get { return _Description1Clean; }
            set { _Description1Clean = value; }
        }

        public virtual System.String DescriptionExtClean
        {
            get { return _DescriptionExtClean; }
            set { _DescriptionExtClean = value; }
        }

  
        public VWProducts()
        {
        }

        public VWProducts(System.String id)
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

         public virtual System.String DescriptionExt {
             get { return _DescriptionExt; }
             set { _DescriptionExt = value;}
         }

        public virtual System.String StockquantityText
        {
            get { return _Stockquantity > 10 ? "Over 10" : Stockquantity.ToString(); }
        }

        public virtual System.Decimal PriceMultiplier
        {
            get { return _PriceMultiplier; }
            set { _PriceMultiplier = value; }
        }

        public virtual System.Decimal ExtendedPrice
        {
            get { return _PriceMultiplier * _QuantityDesired; }
        }

        public virtual System.Decimal ClientProductPrice
        {
            get { return _ClientProductPrice; }
            set { _ClientProductPrice = value; }
        }

        public virtual System.String ClientID
        {
            get { return _ClientID; }
            set { _ClientID = value; }
        }

        public virtual System.String Product_ID1
        {
            get { return _Product_ID1; }
            set { _Product_ID1 = value; }
        }

        public virtual System.String MoreInfoURL
        {
            get { return _MoreInfoURL; }
            set { _MoreInfoURL = value; }
        }

        public virtual System.String CategoryDescription
        {
            get { return _CategoryDescription; }
            set { _CategoryDescription = value; }
        }
        public virtual System.String AddToCart
        {
            get { return _AddToCart; }
            set { _AddToCart = value; }
        }


        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
