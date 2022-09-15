using System;

namespace ZzzLab
{
    public interface ICopyable: ICloneable
    {
        object CopyTo(object target);

        object CopyFrom(object source);
    }
}