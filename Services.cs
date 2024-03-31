using System.Text.Json.Nodes;
using MongoDB.Bson.IO;
using scanapp.Models;
using System.Net.Http;

namespace scanapp {

    class Service {
        private HttpClient sharedClient;
        
        public Service(String uri) {
            
            this.sharedClient = new()
            {
                BaseAddress = new Uri(uri),
            };
        }

        public void DuplicateArticle(int articleId, Article newArticleInfo){
            // HttpContent cnt = new HttpContent(JsonConvert. newArticleInfo);
            // this.sharedClient?.PostAsync(String.Format("/api/duplicate/{0}", articleId), newArticleInfo);
        }
    }
}