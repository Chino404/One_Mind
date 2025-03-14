using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransparency
{
    /// <summary>
    /// Desvanecer el material del objeto
    /// </summary>
    /// <param name="t"></param>
    public void Fade(float t);

    /// <summary>
    /// Para que aparezca el material
    /// </summary>
    /// <param name="t"></param>
    public void Appear(float t);
}
