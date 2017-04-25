using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Services;

namespace SitefinityWebApp
{
    public class AutoImageTranslator
    {
        public static void Action(IDataEvent e)
        {
            var action = e.Action;
            var contentType = e.ItemType;
            if (action == "New" && contentType.FullName == "Telerik.Sitefinity.Libraries.Model.Image")
            {
                var languages = SystemManager.CurrentContext.AppSettings.DefinedFrontendLanguages;
                if (languages.Length > 1)
                {
                    var culture = Thread.CurrentThread.CurrentUICulture;
                    var itemId = e.ItemId;
                    var providerName = e.ProviderName;
                    var librariesManager = LibrariesManager.GetManager(providerName);
                    var image = librariesManager.GetImage(itemId);
                    if (image.Status == ContentLifecycleStatus.Master)
                    {
                        var title = image.Title;
                        foreach (var language in languages)
                        {
                            if (!image.AvailableCultures.Any(i => i.Name == language.Name))
                            {
                                Thread.CurrentThread.CurrentCulture = language;
                                Thread.CurrentThread.CurrentUICulture = language;

                                image.Title = title;
                                librariesManager.SaveChanges();
                                librariesManager.Lifecycle.Publish(image);
                                librariesManager.SaveChanges();
                            }
                        }
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                    }
                }
            }
        }
    }
}