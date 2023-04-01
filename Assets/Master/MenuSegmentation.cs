using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSegmentation : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private ActionDatabase database;

    private Button onScene = null;

    private void CreateButtonWithFunc(ActionNode node)
    {
        var rectTransform = GetComponent<RectTransform>();
        if (onScene != null)
        {
            Destroy(onScene);
        }
        var obj = button.GetComponentInChildren<Text>();
        button.name = node.name;
        obj.text = node.name;
        onScene = Instantiate(button, rectTransform);
        onScene.onClick.AddListener(
               () => node.Action()
        );
    }
    public void FindLabel(string label)
    {
        foreach (var node in database.actions)
        {
            if (node.Label == label)
            {
                CreateButtonWithFunc(node.actionType);
            }
        }
    }
}
