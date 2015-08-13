using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IHaveGraphics
{
    bool Rendering { get; }
    GameObject Graphics { get; }

    bool SetRendering(bool value);
}
