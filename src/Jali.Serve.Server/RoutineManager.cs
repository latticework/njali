using System;
using Jali.Serve.Definition;

namespace Jali.Serve.Server
{
    public class RoutineManager
    {
        public RoutineManager(ResourceManager resourceManager, Routine routine)
        {
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (routine == null) throw new ArgumentNullException(nameof(routine));


            this.ResourceManager = resourceManager;
            this.Routine = routine;
        }

        public Routine Routine { get; }

        public ResourceManager ResourceManager { get; }
    }
}
