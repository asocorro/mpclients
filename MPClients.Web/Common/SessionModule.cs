using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace MPClients.Web.Common
{
    public class GuidSessionIDManager : SessionIDManager
    {

        public override string CreateSessionID(HttpContext context)
        {
            return Guid.NewGuid().ToString();
        }


        public override bool Validate(string id)
        {
            try
            {
                Guid testGuid = new Guid(id);

                if (id == testGuid.ToString())
                    return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
