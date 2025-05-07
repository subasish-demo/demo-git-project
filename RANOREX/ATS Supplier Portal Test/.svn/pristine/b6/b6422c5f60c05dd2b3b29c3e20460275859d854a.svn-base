using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.Hierarchy
{
    public static class HierarchicalDataHelper
    {
        /// <summary>
        /// Generates tree of items from item list
        /// Example usage IEnumerable<Category> mlc = categories.GenerateTree(c => c.Id, c => c.ParentId, (c, ci) => new Category { Id = c.Id, Name = c.Name, ParentId = c.ParentId , Subcategories = ci });
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
                this IEnumerable<T> collection,
                Func<T, K> id_selector,
                Func<T, K> parent_id_selector,
                K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }

        /// <summary>
        /// Get all children in this tree branch with children displayed before siblings
        /// Example usage 
        /// var flat = root.AsDepthFirstEnumerable(x => x.Children);
        /// </summary>
        /// <typeparam name="T">Type being passed in</typeparam>
        /// <param name="head">Root node to search</param>
        /// <param name="childrenFunc">funtion indicating which property in T represents children</param>
        /// <returns></returns>
        public static IEnumerable<T> AsDepthFirstEnumerable<T>(this T head, Func<T, IEnumerable<T>> childrenFunc)
        {
            yield return head;

            if (childrenFunc(head) != null)
            {
                foreach (var node in childrenFunc(head))
                {
                    foreach (var child in AsDepthFirstEnumerable(node, childrenFunc))
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Get all children in this tree branch with siblings displayed before children
        /// Example usage 
        /// var flat = root.AsBreadthFirstEnumerable(x => x.Children);
        /// </summary>
        /// <typeparam name="T">Type being passed in</typeparam>
        /// <param name="head">Root node to search</param>
        /// <param name="childrenFunc">funtion indicating which property in T represents children</param>
        public static IEnumerable<T> AsBreadthFirstEnumerable<T>(this T head, Func<T, IEnumerable<T>> childrenFunc)
        {
            yield return head;

            var last = head;
            if (childrenFunc(head) != null)
            {
                foreach (var node in AsBreadthFirstEnumerable(head, childrenFunc))
                {
                    if (childrenFunc(node) != null)
                    {
                        foreach (var child in childrenFunc(node))
                        {
                            yield return child;
                            last = child;
                        }
                    }
                    if (last.Equals(node)) yield break;
                }
            }
        }
    }
}
