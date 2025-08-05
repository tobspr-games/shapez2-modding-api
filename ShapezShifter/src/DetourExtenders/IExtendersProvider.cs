using System.Collections.Generic;

internal interface IExtendersProvider
{
    IEnumerable<TExtender> ExtendersOfType<TExtender>();
}