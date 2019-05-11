using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/*
Converting the nice, simple implementations of tree traversal algorithms written in Python at https://eddmann.com/posts/depth-first-search-and-breadth-first-search-in-python/ into C#, as a learning exercise
*/

namespace tree_search
{
    class Program
    {
        Dictionary<char, HashSet<char>> graph = new Dictionary<char, HashSet<char>>
        {
            {'A', new HashSet<char>(new[]{'B', 'C'})},
            {'B', new HashSet<char>(new[]{'A', 'D', 'E'})},
            {'C', new HashSet<char>(new[]{'A', 'F'})},
            {'D', new HashSet<char>(new[]{'B'})},
            {'E', new HashSet<char>(new[]{'B', 'F'})},
            {'F', new HashSet<char>(new[]{'C', 'E'})}
         };

        /* with a stack
         using the extension method Except isn't optimized for hashsets. But the HashSet method ExceptWith requires that
         you clone the collection first as it modifies in place, and I'm aiming for simplicity here.
         
        Python version:
        
        def dfs(graph, start):
            visited, stack = set(), [start]
            while stack:
                vertex = stack.pop()
                if vertex not in visited:
                    visited.add(vertex)
                    stack.extend(graph[vertex] - visited)
            return visited
                
         */
        static HashSet<char> DepthFirstSearch1(IReadOnlyDictionary<char, HashSet<char>> graph, char start)
        {
            var visited = new HashSet<char>();
            var stack = new Stack<char>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();
                if (visited.Contains(vertex)) continue;
                
                visited.Add(vertex);
                var newNodes = new HashSet<char>(graph[vertex].Except(visited));
                
                foreach (var node in newNodes)
                    stack.Push(node);
            }

            return visited;
        }

        /* recursive
        
        Same comment on use of Except() applies as in the previous version
        
        For real use it would likely also be better to use an overload without a visited parameter, rather than
        using a default value that gets tested for null in every loop
        
        Python version:
        
        def dfs(graph, start, visited=None):
            if visited is None:
                visited = set()
            visited.add(start)
            for next in graph[start] - visited:
                dfs(graph, next, visited)
            return visited
            
         */
        static HashSet<char> DepthFirstSearch2(
            IReadOnlyDictionary<char, HashSet<char>> graph, char start, HashSet<char> visited = null)
        {
            if (visited == null)
                visited = new HashSet<char>();

            visited.Add(start);

            foreach (var next in graph[start].Except(visited))
                DepthFirstSearch2(graph, next, visited);
            
            return visited;
        }
        
        /*
        This one has two Python versions, one using yield from and recursion, one without:
        
        def dfs_paths(graph, start, goal):
            stack = [(start, [start])]
            while stack:
                (vertex, path) = stack.pop()
                for next in graph[vertex] - set(path):
                    if next == goal:
                        yield path + [next]
                    else:
                        stack.append((next, path + [next]))
                        
        def dfs_paths(graph, start, goal, path=None):
            if path is None:
                path = [start]
            if start == goal:
                yield path
            for next in graph[start] - set(path):
                yield from dfs_paths(graph, next, goal, path + [next])
        
        In C# I've only implemented the non-recursive version. Unfortunately this is an example of where C#'s type syntax
        so verbose it gets in the way of the logic.
        
        Again, using LINQ Except is not very efficient
        
         */
        static IEnumerable<List<char>> DepthFirstSearchShortestPaths(
            IReadOnlyDictionary<char, HashSet<char>> graph, char start, char goal)
        {
            var stack = new Stack<ValueTuple<char, List<char>>>();
            
            stack.Push((start, new List<char>{start}));

            while (stack.Count > 0)
            {
                var (vertex, path) = stack.Pop();

                foreach (var next in graph[vertex].Except(path))
                {
                    var combined = path.Concat(new[] {next}).ToList();
                    
                    if (next == goal)
                        yield return combined;
                    else
                        stack.Push((next, combined));
                }
            }
        }

        /*
         Again, the same comment on Except as the above functions

        def bfs(graph, start):
            visited, queue = set(), [start]
            while queue:
                vertex = queue.pop(0)
                if vertex not in visited:
                    visited.add(vertex)
                    queue.extend(graph[vertex] - visited)
            return visited

         */
        static HashSet<char> BreadthFirstSearch1(IReadOnlyDictionary<char, HashSet<char>> graph, char start)
        {
            var visited = new HashSet<char>();
            var queue = new Queue<char>();
            queue.Enqueue(start);
            
            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                if (visited.Contains(vertex)) continue;
                
                visited.Add(vertex);

                foreach (var item in graph[vertex].Except(visited))
                {
                    queue.Enqueue(item);
                }
            }

            return visited;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
