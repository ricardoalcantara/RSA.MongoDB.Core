using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace RSA.MongoDB.Core
{
    public class UpdateDocument<T> where T : IMongoDocument
    {
        private readonly UpdateDefinitionBuilder<T> _updateBuilder;
        private UpdateDefinition<T> _updateDefinition;

        public UpdateDocument(UpdateDefinitionBuilder<T> updateBuilder)
        {
            _updateBuilder = updateBuilder;
        }

        public UpdateDefinition<T> UpdateDefinition
        {
            get
            {
                return _updateDefinition;
            }
        }

        public UpdateDocument<T> Set<TField>(Expression<Func<T, TField>> field, TField value, bool setIfNull = false)
        {
            if (!setIfNull && value == null)
            {
                return this;
            }

            if (_updateDefinition == null)
                _updateDefinition = _updateBuilder.Set(field, value);
            else
                _updateDefinition = _updateDefinition.Set(field, value);

            return this;
        }

        public UpdateDocument<T> Push<TField>(Expression<Func<T, IEnumerable<TField>>> field, TField value, bool setIfNull = false)
        {
            if (!setIfNull && value == null)
            {
                return this;
            }

            if (_updateDefinition == null)
                _updateDefinition = _updateBuilder.Push(field, value);
            else
                _updateDefinition = _updateDefinition.Push(field, value);

            return this;
        }
    }
}
