using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collections.Scoped;

public class GraphResolver<T> : IDisposable
{
    private readonly ScopedList<DependencyRelation<T, T>> DependencyRelations;

    public GraphResolver(IEnumerable<DependencyRelation<T, T>> dependencies)
    {
        DependencyRelations = ScopedList<DependencyRelation<T, T>>.Get(dependencies);
    }

    public IEnumerable<T> ResolveTopologicalOrder()
    {
        // TODO: detect cycles early

        using MultiValueDictionary<T, T, ScopedList<T>> iDependOn = new(ScopedList<T>.Get,
            ScopedList<T>.Return);

        using MultiValueDictionary<T, T, ScopedList<T>> dependsOnMe = new(ScopedList<T>.Get,
            ScopedList<T>.Return);

        using var freeNodes = ScopedHashSet<T>.Get();

        foreach (var dependencyRelation in DependencyRelations)
        {
            iDependOn.AddValue(dependencyRelation.Dependent, dependencyRelation.Dependency);
            dependsOnMe.AddValue(dependencyRelation.Dependency, dependencyRelation.Dependent);
            freeNodes.Remove(dependencyRelation.Dependent);
            if (!iDependOn.ContainsKey(dependencyRelation.Dependency))
            {
                freeNodes.Add(dependencyRelation.Dependency);
            }
        }

        while (freeNodes.Count > 0)
        {
            var freeNode = freeNodes.First();
            freeNodes.Remove(freeNode);
            yield return freeNode;

            if (!dependsOnMe.TryGetValuesForKey(freeNode, out var dependents))
            {
                continue;
            }

            foreach (var dependent in dependents)
            {
                iDependOn.RemoveValue(dependent, freeNode);
                if (!iDependOn.ContainsKey(dependent))
                {
                    freeNodes.Add(dependent);
                }
            }
        }

        if (iDependOn.KeyCount > 0)
        {
            throw new Exception("Cycle detected");
        }
    }

    public void Dispose()
    {
        DependencyRelations.Dispose();
    }
}