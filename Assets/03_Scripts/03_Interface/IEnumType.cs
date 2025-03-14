using System;

public interface IEnumType<E> where E : Enum
{
    E Type { get; }
}
