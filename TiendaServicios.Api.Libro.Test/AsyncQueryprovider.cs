using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libro.Test
{
    public class AsyncQueryprovider<IEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public AsyncQueryprovider(IQueryProvider queryProvider)
        {
            this._inner = queryProvider;
        }
        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<IEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var resultadoType = typeof(TResult).GetGenericArguments()[0];
            var ejecucionResultado = typeof(IQueryProvider).GetMethod(
                name: nameof(IQueryProvider.Execute),
                genericParameterCount: 1,
                types: new[] { typeof(Exception) }
                )
                .MakeGenericMethod(resultadoType)
                .Invoke(this, new[] { expression });

            var result = (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))?
                .MakeGenericMethod(resultadoType).Invoke(null, new[] { ejecucionResultado});

            return result;



            
        }
    }
}
