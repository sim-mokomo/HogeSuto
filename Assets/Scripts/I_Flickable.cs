
using UnityEngine;

public interface I_Flickable
{
    bool IsFlickable { get; }
    void OnStartFlick(Vector3 startPos);
    void OnFlicking(Vector3 flickPos);
    void OnEndFlick(FlickData flickData);
}
