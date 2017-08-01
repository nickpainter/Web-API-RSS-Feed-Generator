using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPITemplate;
using WebAPITemplate.Controllers;
using System.ServiceModel.Syndication;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace WebAPITemplate.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public async Task ValidateFeed()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            HttpResponseMessage result = controller.Get();
            var isValidFeed = await TryParseFeed(result);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(isValidFeed);
        }

        public async Task<bool> TryParseFeed(HttpResponseMessage response)
        {
            try
            {

                Stream dataStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new StreamReader(dataStream);
                string strResponse = reader.ReadToEnd();
                reader.BaseStream.Position = 0;

                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(dataStream));

                foreach (SyndicationItem item in feed.Items)
                {
                    Debug.Print(item.Title.Text);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
