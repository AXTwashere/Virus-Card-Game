using UnityEngine;
using TMPro;

public class ResearchNumber : MonoBehaviour
{
    TMP_Text tmp;
    void Start() { tmp = GetComponent<TMP_Text>(); }
    public void UpdateNumber(int num) { tmp.text = num.ToString(); }
}
