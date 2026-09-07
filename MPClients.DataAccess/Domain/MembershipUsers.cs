using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// MembershipUsers object for NHibernate mapped table MembershipUsers.
    /// </summary>
    [Serializable]
    public class MembershipUsers : DomainObject<System.Guid>
    {

        private Guid _UserId;
        private System.String _Email;
        private System.String _UserName;
        private System.String _ClientID;
        private System.String _ClientName;
        private System.String _ClientNameOnly;
        private System.Int32 _PriceQuerys;
        private System.Boolean _ChangePassword; 
        private System.Boolean? _IsActive;
        private System.Boolean? _IsLockedOut;
        private System.Boolean? _AllowDelivery;
        private System.Boolean? _AllowPickup;

        public MembershipUsers()
        {
        }

        public MembershipUsers(System.Guid id)
        {
            base.ID = id;
        }

        public virtual System.DateTime? LastPriceQueryDate
        {
            get;
            set;
        }

        public virtual System.String CurrentClientID
        {
            get;
            set;
        }

        public virtual System.String CurrentClientName
        {
            get;
            set;
        }

        public virtual System.Guid ShoppingCartId
        {
            get;
            set;
        }


     
        public virtual System.Int32 PriceQuerys
        {
            get { return _PriceQuerys; }
            set { _PriceQuerys = value; }
        }

        public virtual System.String ClientID
        {
            get { return _ClientID; }
            set { _ClientID = value; }
        }

        public virtual System.Boolean ChangePassword
        {
            get { return _ChangePassword; }
            set { _ChangePassword = value; }
        }

        public virtual System.DateTime? LastActivityDate
        {
            get;
            set;
        }

        public virtual Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public virtual System.String Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public virtual System.String UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public virtual System.String ClientName
        {
            get { return _ClientName; }
            set { _ClientName = value; }
        }

        public virtual System.String Territory
        {
            get;
            set;
        }
        public virtual System.String ClientNameOnly
        {
            get
            { 
                string[] value = _ClientName.ToString().Split('-');
                if (value.Length == 2)
                {
                    return value[1];
                }
                else if (value.Length == 3)
                {
                    return value[1] + '-' + value[2];
                }
                else
                {
                    return _ClientName;
                }
            }
            set { _ClientNameOnly = value; }
        }

        public virtual System.Boolean? IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public virtual System.Boolean? IsLockedOut
        {
            get { return _IsLockedOut; }
            set { _IsLockedOut = value; }
        }

        public virtual System.Boolean? AllowDelivery
        {
            get { return _AllowDelivery; }
            set { _AllowDelivery = value; }
        }

        public virtual System.Boolean? AllowPickup
        {
            get { return _AllowPickup; }
            set { _AllowPickup = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }

    [Serializable]
    public class UserCategory : DomainObject<System.Guid>
    {
        private Guid _UserId;
        private System.String _Category;


        public UserCategory()
        {
        }

        public UserCategory(System.Guid id)
        {
            base.ID = id;
        }

        public virtual System.Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public virtual System.String Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }
}
