using System;
using System.Collections.Generic;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Events
{
    public interface ILifetimeScope
    {
        TService Resolve<TService>();
    }
}
