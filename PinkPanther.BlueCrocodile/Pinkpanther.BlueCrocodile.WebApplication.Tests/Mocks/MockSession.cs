using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Tests.Mocks
{
    class MockSession : ISession
    {
        public IDictionary<string, byte[]> Store { get; set; } = new Dictionary<string, byte[]>();

        public bool IsAvailable 
            => true;

        public string Id 
            => Guid.NewGuid().ToString();

        public IEnumerable<string> Keys 
            => Store.Keys;

        public void Clear() 
            => Store.Clear();


        public void Remove(string key) 
            => Store.Remove(key);

        public void Set(string key, byte[] value) 
            => Store.Add(key, value);

        public bool TryGetValue(string key, out byte[] value) 
            => Store.TryGetValue(key, out value);


        public Task CommitAsync(CancellationToken cancellationToken = default) 
            => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) 
            => Task.CompletedTask;
    }
}
