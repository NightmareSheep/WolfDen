using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wolfden.Server.Other
{
    public class ConcurrencyObjects
    {
        private static readonly ConcurrentDictionary<Guid, object> Locks = new ConcurrentDictionary<Guid, object>();
        private static readonly ConcurrentDictionary<Guid, object> Objects = new ConcurrentDictionary<Guid, object>();

        public static void ConcurentOperation(Guid objectId, Action<object> action) => ConcurentOperation<object>(objectId, action);

        public static void ConcurentOperation<T>(Guid objectId, Action<T> action)
        {
            Locks.TryGetValue(objectId, out object lockObject);
            Objects.TryGetValue(objectId, out object concurrentObject);
            if (!(concurrentObject is T))
                return;

            var concurrentObjectWithCorrectType = (T)concurrentObject;

            if (lockObject != null && concurrentObject != null && concurrentObjectWithCorrectType != null)
                lock (lockObject)
                    action(concurrentObjectWithCorrectType);
        }

        public static void RemoveObject(Guid objectId)
        {
            ConcurentOperation(objectId, (obj) =>
            {
                RemoveObjectWithoutLocking(objectId);
            });
        }

        public static void RemoveObjectWithoutLocking(Guid objectId)
        {
            Locks.TryRemove(objectId, out _);
            Objects.TryRemove(objectId, out _);
        }

        public static bool AddObject(Guid id, object obj)
        {
            if (Locks.TryAdd(id, new object()) && Objects.TryAdd(id, obj))
                return true;
            Locks.TryRemove(id, out _);
            Objects.TryRemove(id, out var _);
            return false;
        }

        public static IEnumerable<T> GetObjectsOfType<T>()
        {
            return ConcurrencyObjects.Objects.Values.OfType<T>();
        }
    }
}
