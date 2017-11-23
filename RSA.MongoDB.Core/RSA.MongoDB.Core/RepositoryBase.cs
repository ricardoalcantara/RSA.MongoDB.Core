using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.Extensions.Options;

namespace RSA.MongoDB.Core
{
    public abstract class RepositoryBase<T> where T : IMongoDocument
    {
        protected readonly MongoConnectionHandler<T> MongoConnectionHandler;

        public RepositoryBase(IOptions<RepositoryOptions> repositoryConfiguration, string collectionName = null)
            : this(repositoryConfiguration.Value.ConnectionString, repositoryConfiguration.Value.DatabaseName, collectionName)
        {
        }
        protected RepositoryBase(string mongoDBConnectionString, string databaseName, string collectionName = null)
        {
            MongoConnectionHandler = new MongoConnectionHandler<T>(mongoDBConnectionString, databaseName, collectionName);
        }

        public IMongoCollection<T> GetCollection()
        {
            return MongoConnectionHandler.MongoCollection;
        }

        public IMongoQueryable<T> AsQueryacble()
        {
            return GetCollection().AsQueryable();
        }

        public FilterDefinitionBuilder<T> GetFilter()
        {
            return Builders<T>.Filter;
        }

        public UpdateDocument<T> GetUpdate()
        {
            return new UpdateDocument<T>(Builders<T>.Update);
        }

        public virtual T GetById(string id)
        {
            var filter = GetFilter().Eq(f => f.Id, new ObjectId(id));
            return GetCollection().FindSync(filter).FirstOrDefault();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = GetFilter().Eq(f => f.Id, new ObjectId(id));
            var find = await GetCollection().FindAsync(filter);
            return await find.FirstOrDefaultAsync();
        }

        public async Task<IAsyncCursor<T>> FindAsync(FilterDefinition<T> filter)
        {
            return await GetCollection().FindAsync<T>(filter);
        }

        public IAsyncCursor<T> FindSync(FilterDefinition<T> filter)
        {
            return GetCollection().FindSync<T>(filter);
        }

        public async Task<IAsyncCursor<T>> FindAllAsync()
        {
            var filter = new BsonDocument();
            return await GetCollection().FindAsync<T>(filter);
        }

        public IAsyncCursor<T> FindAllSync()
        {
            var filter = new BsonDocument();
            return GetCollection().FindSync<T>(filter);
        }

        public void InsertOne(T entity)
        {
            GetCollection().InsertOne(entity);
        }

        public async Task InsertOneAsync(T entity)
        {
            await GetCollection().InsertOneAsync(entity);
        }

        public void InsertMany(IEnumerable<T> entities)
        {
            GetCollection().InsertMany(entities);
        }

        public async Task InsertManyAsync(IEnumerable<T> entities)
        {
            await GetCollection().InsertManyAsync(entities);
        }

        public void ReplaceOne(T entity)
        {
            var filter = GetFilter().Eq(f => f.Id, entity.Id);
            GetCollection().ReplaceOne(filter, entity);
        }

        public async Task ReplaceOneAsync(T entity)
        {
            var filter = GetFilter().Eq(f => f.Id, entity.Id);
            await GetCollection().ReplaceOneAsync(filter, entity);
        }
        
        public void UpdateOne(T entity, UpdateDocument<T> updateDocument)
        {
            UpdateDefinition<T> updateDef = updateDocument.UpdateDefinition;
            if (updateDef == null) return;

            var filter = GetFilter().Eq(f => f.Id, entity.Id);
            GetCollection().UpdateOne(filter, updateDef);
        }

        public async Task UpdateOneAsync(T entity, UpdateDocument<T> updateDocument)
        {
            UpdateDefinition<T> updateDef = updateDocument.UpdateDefinition;
            if (updateDef == null) return;

            var filter = GetFilter().Eq(f => f.Id, entity.Id);
            await GetCollection().UpdateOneAsync(filter, updateDef);
        }

        public void DeleteOne(T entity)
        {
            var filter = GetFilter().Eq(f => f.Id, entity.Id);
            GetCollection().DeleteOne(filter);
        }

        public void DeleteOne(FilterDefinition<T> filter)
        {
            GetCollection().DeleteOne(filter);
        }

        public void DeleteById(string id)
        {
            var filter = GetFilter().Eq(f => f.Id, new ObjectId(id));
            GetCollection().DeleteOne(filter);
        }

        public async Task DeleteOneAsync(T entity)
        {
            var filter = GetFilter().Eq(f => f.Id, entity.Id);
            await GetCollection().DeleteOneAsync(filter);
        }

        public async Task DeleteOneAsync(FilterDefinition<T> filter)
        {
            await GetCollection().DeleteOneAsync(filter);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var filter = GetFilter().Eq(f => f.Id, new ObjectId(id));
            await GetCollection().DeleteOneAsync(filter);
        }

        //public void UpdateMany(T entity, bool includeNull = false)
        //{
        //    UpdateMany(entity, entity, includeNull);
        //}

        //public async Task UpdateManyAsync(T entity, bool includeNull = false)
        //{
        //    await UpdateManyAsync(entity, entity, includeNull);
        //}

        //public void UpdateMany(T entity, T newEntity, bool includeNull = false)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task UpdateManyAsync(T entity, T newEntity, bool includeNull = false)
        //{
        //    throw new NotImplementedException();
        //}        
    }
}
