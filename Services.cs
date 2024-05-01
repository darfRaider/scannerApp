using System.Text.Json;
using System.Net.Http.Headers;
using MongoDB.Bson.IO;
using scanapp.Models;
using System.Text;
using scanapp;

namespace scanapp {

    class Service {
        private HttpClient sharedClient;
        
        public Service(String uri) {
            // TODO: remove following if 
            if(uri.Substring(uri.Length-1) != "/"){
                throw new Exception();
            }
            this.sharedClient = new()
            {
                BaseAddress = new Uri(uri),
            };
        }

        public async Task DuplicateArticle(int articleId, Article? newArticleInfo){
            StringContent strContent;
            // newArticleInfo.ArticleId = Constants.UNASSIGNED;
            if(newArticleInfo != null) 
                strContent = new StringContent(JsonSerializer.Serialize(newArticleInfo), Encoding.UTF8, "application/json");
            else
                strContent = new StringContent("");
            var resp = await this.sharedClient.PostAsync(
                String.Format("api/articles/duplicate/{0}", articleId), strContent);
            if(resp.StatusCode == System.Net.HttpStatusCode.Created){
                Console.Write("StringContent: ");
                Console.WriteLine(strContent);
                Console.WriteLine("Request OK");
            }
            else {
                Console.WriteLine(resp.StatusCode);
                Console.WriteLine(resp.Content);
                Console.WriteLine("Request Error");
            }
        }

        public async Task InsertArticle(Article? newArticleInfo){
            StringContent strContent = new StringContent(
                JsonSerializer.Serialize(newArticleInfo),
                Encoding.UTF8,
                "application/json");

            var resp = await this.sharedClient.PostAsync("api/articles", strContent);
            if(resp.StatusCode == System.Net.HttpStatusCode.Created){
                Console.Write("StringContent: ");
                Console.WriteLine(strContent);
                Console.WriteLine("Request OK");
            }
            else {
                Console.WriteLine(resp.StatusCode);
                Console.WriteLine(resp.Content);
                Console.WriteLine("Request Error");
            }
        }
    }
}