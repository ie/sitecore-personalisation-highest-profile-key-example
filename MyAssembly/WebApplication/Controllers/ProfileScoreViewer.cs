using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class ProfileScoreViewerController : SitecoreController
    {
        // GET: Brochures
        private string cookieKey = "psv";

        public override ActionResult Index()
        {
            Dictionary<string, string> collection = new Dictionary<string, string>();
            bool cookieJustDestroyed = false;
            if (!String.IsNullOrWhiteSpace(Request.QueryString[cookieKey]) && (Request.QueryString[cookieKey] == "false" || Request.QueryString[cookieKey] == "0"))
            {
                Response.Cookies[cookieKey].Expires = DateTime.Now.AddDays(-1);
                cookieJustDestroyed = true;
            }

            if ((!String.IsNullOrWhiteSpace(Request.QueryString[cookieKey]) && (Request.QueryString[cookieKey] == "1" || Request.QueryString[cookieKey] == "true")) || (Request.Cookies[cookieKey] != null && Request.Cookies[cookieKey].Value == "true" && !cookieJustDestroyed))
            {
                var cookie = new HttpCookie(cookieKey, "true");
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(cookie);

                foreach (var profileName in Tracker.Current.Interaction.Profiles.GetProfileNames())
                {
                    var profile = Tracker.Current.Interaction.Profiles[profileName]; //If profile does not exist, it will create that one.

                    foreach (KeyValuePair<string, float> keyValuePair in profile)
                    {
                        string key = String.Format("{0} - {1}", profileName, keyValuePair.Key);
                        if (!collection.ContainsKey(key))
                        {
                            collection.Add(key, String.Format("{0}", keyValuePair.Value));
                        }


                    }


                }
            }


            return View("~/Views/ProfileScoreViewer.cshtml", collection);
        }
    }
}