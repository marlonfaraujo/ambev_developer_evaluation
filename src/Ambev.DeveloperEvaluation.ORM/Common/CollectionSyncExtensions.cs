namespace Ambev.DeveloperEvaluation.ORM.Common
{
    public static class CollectionSyncExtensions
    {
        public static void SyncCollection<T>(
            this ICollection<T> existingItems,
            IEnumerable<T> updatedItems,
            Func<T, object> getKey,
            Action<T, T> updateItem,
            Action<T> onAdd,
            Action<T> onRemove)
        {
            var updatedLookup = updatedItems.ToDictionary(getKey);
            var existingLookup = existingItems.ToDictionary(getKey);

            foreach (var updated in updatedItems)
            {
                var key = getKey(updated);
                if (existingLookup.TryGetValue(key, out var existing))
                {
                    updateItem(existing, updated);
                }
                else
                {
                    onAdd(updated);
                    existingItems.Add(updated);
                }
            }

            var keysToRemove = existingItems
                .Where(item => !updatedLookup.ContainsKey(getKey(item)))
                .ToList();

            foreach (var itemToRemove in keysToRemove)
            {
                onRemove(itemToRemove);
                existingItems.Remove(itemToRemove);
            }
        }
    }
}
