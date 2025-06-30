public struct DependencyRelation<TDependent, TDependency>
{
    public readonly TDependent Dependent;
    public readonly TDependency Dependency;

    public DependencyRelation(TDependent dependent, TDependency dependency)
    {
        Dependent = dependent;
        Dependency = dependency;
    }
}