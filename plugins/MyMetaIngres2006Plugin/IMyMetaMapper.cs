using System.Collections.Generic;

namespace MyMeta.Plugins
{
    interface IMyMetaMapper<T>
    {
        T Execute();
    }
}
