using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using wasp.WebApi.Data.Dto.Base;

namespace wasp.WebApi.Controllers.Base
{
    public class EnvelopeResult
    {
        private readonly ControllerBase _controllerBase;

        public EnvelopeResult(ControllerBase controllerBase)
        {
            _controllerBase = controllerBase;
        }

        public OkObjectResult Ok<T>() => _okArray(Array.Empty<T>());

        public OkObjectResult Ok<T>(IEnumerable<T> items)
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }

        public OkObjectResult Ok<T>(IList<T> items)
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }

        public OkObjectResult Ok<T>(List<T> items)
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }
        
        public OkObjectResult Ok<T>(T item)
        {
            return _okArray(new []{item});
        }
        
        private OkObjectResult _okArray<T>(IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type? itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];
          
            return _controllerBase.Ok(new ResponseBodyDto<T>
            {
                Data = new ResponseBodyDataDto<T>
                {
                    Type =  itemType?.Name ?? "<item-type not found>",
                    Items = items,
                    Count = null
                }
            });
        }

        public CreatedResult Created<T>(string url, IEnumerable<T> items)
        {
            if (items is null)
                return _createdArray(url, Array.Empty<T>());

            return _createdArray(url, items);
        }
        
        public CreatedResult Created<T>(string url, T item) where T:class
        {
            return _createdArray(url, new[] {item});
        }
        
        private CreatedResult _createdArray<T>(string url, IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();

            return _controllerBase.Created(url, new ResponseBodyDto<T>
            {
                Data = new ResponseBodyDataDto<T>
                {
                    Type = itemType?.Name,
                    Items = enumerable.Cast<T>(),
                    Count = enumerable.LongCount()
                }
            });
        }

        public OkObjectResult Updated<T>(T item) where T:class
        {
            return _updatedArray(new[] {item});
        }

        public OkObjectResult Updated<T>(IEnumerable<T> items)
        {
            if (items is null)
                return _updatedArray(Array.Empty<T>());

            return _updatedArray(items);
        }

        private OkObjectResult _updatedArray<T>(IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();

            return _controllerBase.Ok(new ResponseBodyDto<T>
            {
                Data = new ResponseBodyDataDto<T>
                {
                    Type = itemType?.Name,
                    Items = enumerable.Cast<T>(),
                    Count = enumerable.LongCount()
                }
            });
        }
    }
}