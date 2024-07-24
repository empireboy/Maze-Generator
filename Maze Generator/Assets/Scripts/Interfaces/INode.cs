using System.Collections.Generic;

public interface INode<T>
{
    List<T> Neighbours { get; }
}