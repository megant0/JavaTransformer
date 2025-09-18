using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace JavaTransformer.UI.MainDesktop.Models.Extension
{

    public static class MenuItemExtensions
    {
        public static MenuItem FindByHeader(this Menu menu, string header)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header), "The header cannot be empty.");

            if (menu.Items == null || menu.Items.Count == 0)
                return null;

            return FindMenuItemByHeader(menu.Items, header);
        }
        public static MenuItem FindByNestedPath(this Menu menu, string nestedPath)
        {
            if (nestedPath == null)
                throw new ArgumentNullException(nameof(nestedPath), "The path cannot be null");

            if (string.IsNullOrWhiteSpace(nestedPath))
                throw new ArgumentException("The path cannot be empty or contain only spaces.", nameof(nestedPath));

            var pathParts = ParseNestedPath(nestedPath);
            if (pathParts.Length == 0)
                return null;

            return FindMenuItemByPath(menu.Items, pathParts, 0);
        }

        public static MenuItem FindByPathParts(this Menu menu, params string[] pathParts)
        {
            if (pathParts == null)
                throw new ArgumentNullException(nameof(pathParts), "Parts of the path cannot be null");

            if (pathParts.Length == 0)
                return null;

            if (pathParts.Any(string.IsNullOrEmpty))
                throw new ArgumentException("All parts of the path must contain non-empty values.", nameof(pathParts));

            return FindMenuItemByPath(menu.Items, pathParts, 0);
        }

    
        public static bool TryFindByNestedPath(this Menu menu, string nestedPath, out MenuItem result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(nestedPath))
                return false;

            try
            {
                result = menu.FindByNestedPath(nestedPath);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public static MenuItem FindRecursiveByHeader(this Menu menu, string header)
        {
            if (header == null || menu.Items == null)
                return null;

            return FindMenuItemRecursive(menu.Items, header);
        }

    
        public static IEnumerable<MenuItem> GetPathElements(this Menu menu, string nestedPath)
        {
            if (string.IsNullOrWhiteSpace(nestedPath))
                return Enumerable.Empty<MenuItem>();

            var pathParts = ParseNestedPath(nestedPath);
            if (pathParts.Length == 0)
                return Enumerable.Empty<MenuItem>();

            return GetMenuItemPath(menu.Items, pathParts, 0).ToList();
        }

        private static MenuItem FindMenuItemByPath(IEnumerable items, string[] pathParts, int currentIndex)
        {
            if (items == null || currentIndex >= pathParts.Length)
                return null;

            var currentHeader = pathParts[currentIndex];
            var currentItem = FindMenuItemByHeader(items, currentHeader);

            if (currentItem == null)
                return null;

            if (currentIndex == pathParts.Length - 1)
                return currentItem;

            return FindMenuItemByPath(currentItem.Items, pathParts, currentIndex + 1);
        }

        private static IEnumerable<MenuItem> GetMenuItemPath(IEnumerable items, string[] pathParts, int currentIndex)
        {
            if (items == null || currentIndex >= pathParts.Length)
                yield break;

            var currentHeader = pathParts[currentIndex];
            var currentItem = FindMenuItemByHeader(items, currentHeader);

            if (currentItem == null)
                yield break;

            yield return currentItem;

            if (currentIndex < pathParts.Length - 1)
            {
                foreach (var childItem in GetMenuItemPath(currentItem.Items, pathParts, currentIndex + 1))
                {
                    yield return childItem;
                }
            }
        }

        private static string[] ParseNestedPath(string nestedPath)
        {
            return nestedPath.Split('.')
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrEmpty(part))
                .ToArray();
        }

        private static MenuItem FindMenuItemByHeader(IEnumerable items, string header)
        {
            if (items == null)
                return null;

            return items.OfType<MenuItem>()
                .FirstOrDefault(item => item != null &&
                                      string.Equals(item.Header?.ToString(), header, StringComparison.OrdinalIgnoreCase));
        }

        private static MenuItem FindMenuItemRecursive(IEnumerable items, string header)
        {
            if (items == null)
                return null;

            foreach (MenuItem item in items.OfType<MenuItem>())
            {
                if (item == null)
                    continue;

                if (string.Equals(item.Header?.ToString(), header, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }

                if (item.Items != null && item.Items.Count > 0)
                {
                    var foundInChildren = FindMenuItemRecursive(item.Items, header);
                    if (foundInChildren != null)
                    {
                        return foundInChildren;
                    }
                }
            }

            return null;
        }


        public static bool ContainsHeader(this Menu menu, string header)
        {
            return menu.FindByHeader(header) != null;
        }

      
        public static bool ContainsNestedPath(this Menu menu, string nestedPath)
        {
            return menu.TryFindByNestedPath(nestedPath, out _);
        }

        public static int GetPathDepth(this Menu menu, string nestedPath)
        {
            if (string.IsNullOrWhiteSpace(nestedPath))
                return 0;

            var pathParts = ParseNestedPath(nestedPath);
            return pathParts.Length;
        }
    }
}
