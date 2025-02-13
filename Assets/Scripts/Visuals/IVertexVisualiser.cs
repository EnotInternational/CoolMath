using System.Collections.Generic;
using UnityEngine;

public interface IVertexVisualizer<T>
{
    public T vertex{ get; set; }
    public void UpdatePosition();
}
