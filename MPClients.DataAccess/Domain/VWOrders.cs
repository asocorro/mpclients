using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Order object for NHibernate mapped table SpecialOrders.
    /// </summary>
    [Serializable]
    public class VWOrders : DomainObject<System.Guid>
    {

        private System.String _ClientId;
        private System.Guid _OrderId;
        private System.String _Name;
        private System.String _Telephonenumber;
        private System.DateTime _OrderDate;
        private System.DateTime? _StatusDate;
        private System.Int32 _Status;
        private System.String _OrderNo;
        private System.Int32 _DetailsCount;
        private System.String _UserName;

        public VWOrders()
        {
        }

        public VWOrders(System.Guid id)
        {
            base.ID = id;
        }

        public virtual System.String ClientId
        {
            get { return _ClientId; }
            set { _ClientId = value; }
        }

        public virtual System.Guid OrderId
        {
            get { return _OrderId; }
            set { _OrderId = value; }
        }

        public virtual System.String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public virtual System.String Telephonenumber
        {
            get { return _Telephonenumber; }
            set { _Telephonenumber = value; }
        }

        public virtual System.DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }
        public virtual System.DateTime? StatusDate
        {
            get { return _StatusDate; }
            set { _StatusDate = value; }
        }

        public virtual System.Int32 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public virtual System.Int32 DetailsCount
        {
            get { return _DetailsCount; }
            set { _DetailsCount = value; }
        }

        public virtual System.String OrderNo
        {
            get { return _OrderNo; }
            set { _OrderNo = value; }
        }

        public virtual System.String UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }
}
