using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Accreditation object for NHibernate mapped table Accreditation.
    /// </summary>
    [Serializable]
    public class Accreditation : DomainObject<System.Int32>
    {


        private System.String _CreditID;
        private System.String _InvoiceID;
        private System.DateTime _Applicationdate;
        private System.Decimal _Amount;

        public Accreditation()
        {
        }

        public Accreditation(System.Int32 id)
        {
            base.ID = id;
        }

         public virtual System.String CreditID {
             get { return _CreditID; }
             set { _CreditID = value;}
         }

         public virtual System.String InvoiceID {
             get { return _InvoiceID; }
             set { _InvoiceID = value;}
         }

         public virtual System.DateTime Applicationdate {
             get { return _Applicationdate; }
             set { _Applicationdate = value;}
         }

         public virtual System.Decimal Amount {
             get { return _Amount; }
             set { _Amount = value;}
         }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }
}
