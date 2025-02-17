using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(GameLoop))]
[RequireComponent(typeof(AudioSource))]
public class TheGameUI : MonoBehaviour
{

    private UIDocument doc;
    private GameLoop gameLoop;

    private List<Button> buttons = new List<Button>();

    private AudioSource audioSource;


    void Awake()
    {
        doc = GetComponent<UIDocument>();
        gameLoop = GetComponent<GameLoop>();

        audioSource = GetComponent<AudioSource>();

        // references to buttons:
        // Button button = doc.rootVisualElement.Q("name_of_button") as button
        // onclick event:
        // button.RegisterCallback<ClickEvent>();

        // register callbacks
        buttons = doc.rootVisualElement.Query<Button>().ToList();
        foreach (Button b in buttons)
        {
            b.RegisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    void OnDsable()
    {
        // unresgister
        // button.UnregisterCallback<ClickEvent>(OnPlayGameClick);

        foreach (Button b in buttons)
        {
            b.UnregisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnPlayGameClick(ClickEvent ev)
    {
        Debug.Log("Test Button");
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
