using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPITemplate.Models
{
    public class NewsArticleViewModel
    {
        public string OriginalAuthor { get; set; }
        public string Headline { get; set; }
        public string ArticleBodyText { get; set; }
        public string FullUrl { get; set; }
    }
}