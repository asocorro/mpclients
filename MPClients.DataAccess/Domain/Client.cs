using System;
using System.Collections.Generic;

namespace MPClients.DataAccess.Domain
{
    /// <summary>
    /// Client object for NHibernate mapped table Client.
    /// </summary>
    [Serializable]
    public class Client : DomainObject<System.String>
    {


        private System.String _Name;
        private System.String _Contactperson;
        private System.String _Address1;
        private System.String _Address2;
        private System.String _Town;
        private System.String _Country;
        private System.String _Telephonenumber;
        private System.Boolean? _Active;

        public Client()
        {
        }

        public Client(System.String id)
        {
            base.ID = id;
        }

         public virtual System.String Name {
             get { return _Name; }
             set { _Name = value;}
         }

         public virtual System.String Contactperson {
             get { return _Contactperson; }
             set { _Contactperson = value;}
         }

         public virtual System.String Address1 {
             get { return _Address1; }
             set { _Address1 = value;}
         }

         public virtual System.String Address2 {
             get { return _Address2; }
             set { _Address2 = value;}
         }

         public virtual System.String Town {
             get { return _Town; }
             set { _Town = value;}
         }

         public virtual System.String Country {
             get { return _Country; }
             set { _Country = value;}
         }

         public virtual System.String Telephonenumber {
             get { return _Telephonenumber; }
             set { _Telephonenumber = value;}
         }

         public virtual System.Boolean? Active {
             get { return _Active; }
             set { _Active = value;}
         }

         public virtual System.String Territory
         {
             get;
             set;
         }

        public virtual System.String Fullname
        {
            get { return string.Format("{0}-{1}", ID.Trim(), Name ); }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

     }


    [Serializable]
    public class ClientHoldDate : DomainObject<System.String>
    {

        public ClientHoldDate()
        {
        }

        public ClientHoldDate(System.String id)
        {
            base.ID = id;
        }

      
        public virtual System.DateTime? HoldFromDate
        {
            get;
            set;
        }


        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }
}
