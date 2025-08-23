using UnityEngine;
using UnityEngine.Events;

public class XButton : MonoBehaviour
{
    public UnityEvent Exit;
    public void ButtonEnd() {Exit.Invoke();}
}
