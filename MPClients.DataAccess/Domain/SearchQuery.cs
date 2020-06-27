using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// New object for NHibernate mapped table SearchQuery.
    /// </summary>
    [Serializable]
    public class SearchQuery : DomainObject<System.Int32>
    {

        private System.String _QueryText;
        private System.DateTime _QueryDateTime;
        private System.Guid _UserId;
        private System.Int32 _ResultsCount;

        public SearchQuery()
        {
        }

        //public SearchQuery(System.Int32 id)
        //{
        //    base.ID = id;
        //}

        public virtual System.String QueryText
        {
            get { return _QueryText; }
            set { _QueryText = value; }
        }

        public virtual System.DateTime QueryDateTime
        {
            get { return _QueryDateTime; }
            set { _QueryDateTime = value; }
        }

        public virtual System.Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public virtual System.Int32 ResultsCount
        {
            get { return _ResultsCount; }
            set { _ResultsCount = value; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }
}
