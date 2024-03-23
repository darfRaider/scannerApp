﻿using MongoDB.Driver;
using scanapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanapp
{
    internal class MongoDbHandler
    {
        private MongoClient _client;
        private IMongoDatabase _db;
        private IMongoCollection<Article> articleCollection;
        private IMongoCollection<Article> containmentCollection;

        public MongoDbHandler(string host, string dbName)
        {
            this._client = new MongoClient(host);
            this._db = this._client.GetDatabase(dbName);
            this.articleCollection = this._db.GetCollection<Article>("Articles");
            this.containmentCollection = this._db.GetCollection<Article>("Containments");
        }

        public List<Article> getArticles()
        {
            return this.articleCollection.Find<Article>(_ => true).ToList();
        }

        private List<string> getStringListFromArticle(Article article)
        {
            return new List<string> { article.ArticleId.ToString(), article.ArticleName };
        }

        public void printArticles()
        {
            var header = new List<string> { "Id", "ArticleName" };
            var articleList = new List<List<string>> { };
            this.getArticles().ForEach(article => { articleList.Add(getStringListFromArticle(article)); });
            var ct = new ConsoleTable(articleList, header);
        }
    }
}
