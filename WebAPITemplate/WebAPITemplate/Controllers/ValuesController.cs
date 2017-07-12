using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using System.Text;
using WebAPITemplate.Models;
using System.Collections.ObjectModel;

namespace WebAPITemplate.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        //public IEnumerable<string> Get()
        public HttpResponseMessage Get()
        {
            string feedTitle = "News article feed title";
            string feedDescription = "Recent news articles";
            Uri feedAlternateLink = new System.Uri("https://www.google.com");
            SyndicationFeed feed = new SyndicationFeed(feedTitle, feedDescription, feedAlternateLink);

            // Add feed items
            Collection<SyndicationItem> items = new Collection<SyndicationItem>();

            foreach (Models.NewsArticleViewModel article in this.GetArticles())
            {
                // Add article to feed item
                SyndicationItem item = new SyndicationItem(article.Headline, article.ArticleBodyText.ToString(), new Uri(article.FullUrl));
                SyndicationPerson person = new SyndicationPerson();

                // Add author information to feed item
                person.Name = article.OriginalAuthor;
                item.Authors.Add(person);
                item.Id = article.FullUrl;

                items.Add(item);
            }
            feed.Items = items;

            // save the feed in memory and then return it to the client
            System.IO.Stream stream = new System.IO.MemoryStream();
            XmlWriter rssWriter = XmlWriter.Create(stream);
            Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
            rssFormatter.WriteTo(rssWriter);
            rssWriter.Close();
            stream.Position = 0;  //reset stream position
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                return new HttpResponseMessage() { Content = new StringContent(sr.ReadToEnd(), Encoding.UTF8, "application/xml") };
            }
            

            

            

            // save to atom feed
            //XmlWriter atomWriter = XmlWriter.Create("atom.xml");
            //Atom10FeedFormatter atomFormatter = new Atom10FeedFormatter(feed);
            //atomFormatter.WriteTo(atomWriter);
            //atomWriter.Close();

            // save to RSS feed
            //XmlWriter rssWriter = XmlWriter.Create("rss.xml");
            //Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
            //rssFormatter.WriteTo(rssWriter);
            //rssWriter.Close();

            //return new string[] { "value1", "value2" };
        }

        private IEnumerable<NewsArticleViewModel> GetArticles()
        {
            NewsArticleViewModel article1 = new NewsArticleViewModel();
            article1.ArticleBodyText = "Sample article 1";
            article1.FullUrl = "http://www.google.com/article1";
            article1.Headline = "Sensational headline";
            article1.OriginalAuthor = "Nick Painter";

            NewsArticleViewModel article2 = new NewsArticleViewModel();
            article2.OriginalAuthor = "Nicholas Charles Painter";
            article2.FullUrl = "http://www.google.com/article2";
            article2.ArticleBodyText = "sample article body 2";
            article2.Headline = "not so sensational headline";

            List<NewsArticleViewModel> retVal = new List<NewsArticleViewModel>();
            retVal.Add(article1);
            retVal.Add(article2);
            return retVal;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
