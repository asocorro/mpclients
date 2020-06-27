using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Order object for NHibernate mapped table Orders.
    /// </summary>
    [Serializable]
    public class Orders : DomainObject<System.Guid>
    {


        private System.Guid _UserId;
        private System.String _OrderNo;
        private System.String _PONumber;
        private System.String _ContactPerson;
        private System.Boolean? _Delivery;
        private System.DateTime _OrderDate;
        private System.String _Note;
        private System.Int32? _Status;
        private System.Int32 _AutoNum;

        private System.DateTime? _StatusDate;
        private System.String _ConfirmationNumber;
        private OrderStatus _StatusOfOrder;
        private IList<OrderDetail> _OrderDetails;
        private MembershipUsers _UserIdLookup;

        public virtual MembershipUsers UserIdLookup
        {
            get { return _UserIdLookup; }
            set { _UserIdLookup = value; }
        }

        public Orders()
        {
        }

        public Orders(System.Guid id)
        {
            base.ID = id;
        }

        public virtual System.String OnBehalfOf
        {
            get;
            set;
        }
        public virtual System.Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public virtual System.String OrderNo
        {
            get { return _OrderNo; }
            set { _OrderNo = value; }
        }


        public virtual System.String PONumber
        {
            get { return _PONumber; }
            set { _PONumber = value; }
        }

        public virtual System.String ContactPerson
        {
            get { return _ContactPerson; }
            set { _ContactPerson = value; }
        }

        public virtual System.Boolean? Delivery
        {
            get { return _Delivery; }
            set { _Delivery = value; }
        }

        public virtual System.DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }

        public virtual System.String Note
        {
            get { return _Note; }
            set { _Note = value; }
        }

        public virtual System.Int32? Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public virtual System.DateTime? StatusDate
        {
            get { return _StatusDate; }
            set { _StatusDate = value; }
        }
        public virtual System.String ConfirmationNumber
        {
            get { return _ConfirmationNumber; }
            set { _ConfirmationNumber = value; }
        }

        public virtual System.Int32 AutoNum
        {
            get { return _AutoNum; }
            set { _AutoNum = value; }
        }
        public virtual decimal OrderTotal
        {
            get
            {
                decimal total = 0;
                foreach (OrderDetail detail in _OrderDetails)
                {
                    total += detail.ExtendedPrice;
                }
                return total;
            }
        }

        public virtual OrderStatus StatusOfOrder
        {
            get { return _StatusOfOrder; }
            set { _StatusOfOrder = value; }
        }

        public virtual IList<OrderDetail> OrderDetails
        {
            get { return _OrderDetails; }
            set { _OrderDetails = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }
}
