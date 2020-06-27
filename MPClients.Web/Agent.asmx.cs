using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.ComponentModel;

namespace MPClients.Web
{
    public class UpdateInfo
    {
        // Fields
        public bool IsAvailable;
        public string newVersion;
        public string Url;
    }

    /// <summary>
    [WebService(Namespace = "http://tempuri.org/"), ToolboxItem(false), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Agent : WebService
    {
        // Methods
        [WebMethod]
        public UpdateInfo GetUpdateDBInfo(string userID)
        {
            UpdateInfo info = new UpdateInfo();
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(base.Context.Request.MapPath("updatedbcfg.xml"));
            }
            catch (Exception)
            {
                info.IsAvailable = false;
                return info;
            }
            string xpath = string.Format("//downloadmodule[@name=\"mpmobile{0}.sdf\"]", userID);
            XmlElement element = (XmlElement)document["updateinfo"].SelectSingleNode(xpath);
            if (element == null)
            {
                info.IsAvailable = false;
                return info;
            }
            info.IsAvailable = true;
            string relativeUri = element.InnerText.Trim();
            info.Url = new Uri(base.Context.Request.Url, relativeUri).ToString();
            return info;
        }

        [WebMethod]
        public UpdateInfo GetUpdateInfo(string name, string platform, string arch, int maj, int min, int bld)
        {
            UpdateInfo info = new UpdateInfo();
            Version version = new Version(maj, min, bld);
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(base.Context.Request.MapPath("updatecfg.xml"));
            }
            catch (Exception)
            {
                info.IsAvailable = false;
                return info;
            }
            string xpath = string.Format("//downloadmodule[@arch=\"{0}\" and @name=\"{4}\" and ( version/@maj>{1} or version/@maj={1}  and (version/@min > {2} or version/@min = {2} and version/@bld > {3}))]", new object[] { arch, maj, min, bld, name });
            XmlElement element = (XmlElement)document["updateinfo"].SelectSingleNode(xpath);
            if (element == null)
            {
                info.IsAvailable = false;
                return info;
            }
            info.IsAvailable = true;
            info.newVersion = new Version(string.Format("{0}.{1}.{2}", element["version"].Attributes["maj"].Value, int.Parse(element["version"].Attributes["min"].Value), int.Parse(element["version"].Attributes["bld"].Value))).ToString();
            string relativeUri = element.InnerText.Trim();
            info.Url = new Uri(base.Context.Request.Url, relativeUri).ToString();
            return info;
        }
    }


}
