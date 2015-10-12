﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Core
{
    public static class JaliCoreExtensions
    {
        public static void AddRange<T>(this ICollection<T> sequence, IEnumerable<T> range)
        {
            var list = sequence as List<T>;
            if (list != null)
            {
                list.AddRange(range);
            }
            else
            {
                foreach (var element in range)
                {
                    sequence.Add(element);
                }
            }
        }

        public static async Task<TryGetResult<TValue>> GetValueOrDefaultAsync<TKey, TValue>(
            this IDictionary<TKey, TValue> reference, TKey key, Func<Task<TValue>> factory)
        {
            TValue value;
            var succeeded = reference.TryGetValue(key, out value);

            if (!succeeded)
            {
                value = await factory();
            }

            return new TryGetResult<TValue>
            {
                Value = value,
                Succeeded = succeeded,
            };
        }

        public static TryGetResult<TValue> GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> reference,
            TKey key, Func<TValue> factory)
        {
            TValue value;
            var succeeded = reference.TryGetValue(key, out value);

            return new TryGetResult<TValue>
            {
                Value = value,
                Succeeded = succeeded,
            };
        }

        public static TryGetResult<TValue> GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> reference,
            TKey key)
        {
            return reference.GetValueOrDefault(key, () => default(TValue));
        }

        public static async Task<TryGetResult<TValue>> GetOrCreateValueAsync<TKey, TValue>(
            this IDictionary<TKey, TValue> reference, TKey key, Func<Task<TValue>> factory)
        {
            var result = await reference.GetValueOrDefaultAsync(key, factory);

            if (!result.Succeeded && result.Value != null)
            {
                reference[key] = result.Value;
            }

            return result;
        }

        public static TryGetResult<TValue> GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> reference,
            TKey key, Func<TValue> factory)
        {
            var result = reference.GetValueOrDefault(key, factory);

            if (!result.Succeeded && result.Value != null)
            {
                reference[key] = result.Value;
            }

            return result;
        }

        public static TryGetResult<TValue> GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> reference,
            TKey key)
        {
            return reference.GetOrCreateValue(key, () => default(TValue));
        }
    }
}