using BcdSharp;
using System.Collections.Generic;
using System.Linq;

namespace AtlasToolbox.Services
{
    /// <summary>
    /// Registers the BCD service. 
    /// </summary>
    public class BcdService : IBcdService
    {
        private readonly BcdStore _store;

        public BcdService(BcdStore store)
        {
            _store = store;
        }

        public void DeleteElement(string objectId, uint elementType)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.DeleteElement(elementType);
            App.logger.Info($"[BCDEdit] Deleted element type {elementType} from object {objectId}");
        }

        public object GetElementValue(string objectId, uint elementType)
        {
            using BcdObject obj = _store.OpenObject(objectId);

            if (obj is null)
            {
                return null;
            }

            bool elementExists = obj.EnumerateElementTypes().Contains(elementType);

            if (!elementExists)
            {
                return null;
            }

            using BcdElement element = obj.GetElement(elementType)!;

            object value = element switch
            {
                BcdBooleanElement booleanElement => booleanElement.Boolean,
                BcdIntegerElement integerElement => integerElement.Integer,
                BcdIntegerListElement integerListElement => integerListElement.Integers,
                BcdObjectElement objectElement => objectElement.Id,
                BcdObjectListElement objectListElement => objectListElement.Ids,
                BcdStringElement stringElement => stringElement.String,
                _ => null
            };

            return value;
        }

        public void SetBooleanElement(string objectId, uint elementType, bool value)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.SetBooleanElement(elementType, value);
        }

        public void SetIntegerElement(string objectId, uint elementType, ulong value)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.SetIntegerElement(elementType, value);
        }

        public void SetIntegerListElement(string objectId, uint elementType, IEnumerable<ulong> value)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.SetIntegerListElement(elementType, value);
        }

        public void SetObjectElement(string objectId, uint elementType, string value)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.SetObjectElement(elementType, value);
        }

        public void SetObjectListElement(string objectId, uint elementType, IEnumerable<string> value)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.SetObjectListElement(elementType, value);
        }

        public void SetStringElement(string objectId, uint elementType, string value)
        {
            using BcdObject @object = _store.OpenObject(objectId);
            @object?.SetStringElement(elementType, value);
        }
    }
}
