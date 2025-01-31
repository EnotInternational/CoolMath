using System.Collections.Generic;
using UnityEngine;

public interface IVertexVisualizer
{
    public void SetGoal();
    public void SetStart();
    public void SetSeen();
    public void SetUnseen();
    public void SetProcessed();
}
