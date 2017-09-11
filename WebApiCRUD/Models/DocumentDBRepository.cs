using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using WebApiCRUD.Configuration;

namespace WebApiCRUD.Models
{
    public class DocumentDBRepository<T> where T : class
    {
        private string _databaseId;
        private string _collectionId;

        private DocumentClient _client;

        public DocumentDBRepository(IOptions<DocumentDbSettings> settings)
        {
            _collectionId = settings.Value.Collection;
            _databaseId = settings.Value.Database;

            _client = new DocumentClient(new Uri(settings.Value.Endpoint), settings.Value.AuthKey);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<T> GetItemAsync(int id)
        {
            try
            {
                Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id.ToString()));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
        }

        public async Task<Document> UpdateItemAsync(int id, T item)
        {
            return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id.ToString()), item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id.ToString()));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Database { Id = _databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new DocumentCollection { Id = _collectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
