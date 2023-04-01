using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController: MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI output;

    private void Awake()
    {
        backButton.onClick.AddListener(SceneSelector.GoBack);
    }

    public void SetOutputText(string outputText)
    {
        output.text = outputText;
    }

}
