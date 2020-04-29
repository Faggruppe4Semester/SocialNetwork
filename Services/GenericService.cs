using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using SocialNetwork.Areas.Database;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class GenericService<TCollection> : IService<TCollection, string>
    where TCollection : class, IModel
    {
        private IMongoCollection<TCollection> _collection;

        public GenericService(ISocialNetworkDatabaseSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TCollection>(collectionName);
        }

        /*----- CREATE -----*/
        public TCollection Create(TCollection obj)
        {
            _collection.InsertOne(obj);
            return obj;
        }

        /*----- READ -----*/
        public List<TCollection> Read() => _collection.Find(collection => true).ToList();
        public TCollection Read(string id) => Read(model => model.Id == id); //Nested call to Read(Expression<Func<TCollection, bool>> filter)
        public TCollection Read(Expression<Func<TCollection, bool>> filter) => _collection.Find(filter).FirstOrDefault();

        /*----- UPDATE -----*/
        public TCollection Update(TCollection obj, string id) => Update(obj, model => model.Id == id); //Nested call to Update(TCollection obj, Expression<Func<TCollection, bool>> filter)
        public TCollection Update(TCollection obj, Expression<Func<TCollection, bool>> filter)
        {
            _collection.ReplaceOne(filter, obj);
            return obj;
        }

        /*----- DELETE -----*/
        public void Delete(TCollection obj) => Delete(obj.Id); //Nested call to Delete(string id)
        public void Delete(string id) => Delete(collection => collection.Id == id); //Nested call to Delete(Expression<Func<TCollection, bool>> filter)
        public void Delete(Expression<Func<TCollection, bool>> filter) => _collection.DeleteOne(filter);
    }
}