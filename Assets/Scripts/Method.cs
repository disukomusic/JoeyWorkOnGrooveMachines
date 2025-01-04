using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Method", menuName = "Method", order = 0)]
public class Method : ScriptableObject
{

    public string methodName;
    [TextArea]
    public string methodDescription;
    public float pointValue;
}
