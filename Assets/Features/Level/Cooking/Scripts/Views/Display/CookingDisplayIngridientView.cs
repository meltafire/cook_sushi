using TMPro;
using UnityEngine;

public class CookingDisplayIngridientView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private TMP_Text _count;

    public void Show(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public void SetData(string name, int count)
    {
        _name.text = name;
        _count.text = count.ToString();
    }
}
