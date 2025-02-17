using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(GameLoop))]
[RequireComponent(typeof(AudioSource))]
public class TheGameUI : MonoBehaviour
{

    private UIDocument doc;
    private GameLoop gameLoop;

    private List<Button> buttons = new();

    private AudioSource audioSource;


    // INCREMENT/DECREMENT BUTTONS
    private Button energyDecButton;
    private Button energyIncButton;

    private Button foodDecButton;
    private Button foodIncButton;

    private Button waterDecButton;
    private Button waterIncButton;

    private Button taxDecButton;
    private Button taxIncButton;

    private Button debtDecButton;
    private Button debtIncButton;

    // PRODUCTION/DEMAND LABELS
    private Label energyProdLabel;
    private Label energyDemLabel;

    private Label waterProdLabel;
    private Label waterDemLabel;

    private Label foodProdLabel;
    private Label foodDemLabel;


    // FUDING LABELS

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        gameLoop = GetComponent<GameLoop>();

        audioSource = GetComponent<AudioSource>();

        // references to buttons:
        // Button button = doc.rootVisualElement.Q("name_of_button") as button
        // onclick event:
        // button.RegisterCallback<ClickEvent>();

        // REGISTERING BUTTONS


        // registering labels
        energyProdLabel = doc.rootVisualElement.Q<Label>("energy-prod");
        energyDemLabel = doc.rootVisualElement.Q<Label>("energy-dem");

        // register callbacks
        buttons = doc.rootVisualElement.Query<Button>().ToList();
        foreach (Button b in buttons)
        {
            b.RegisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    // Update all production/demand labels
    public void Tick()
    {
        energyProdLabel.text = $"{gameLoop.energyProduction}mwh";
        energyDemLabel.text = $"{gameLoop.energyDemand}mwh";
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

    private void OnAllButtonClick(ClickEvent evt)
    {
        audioSource.Play();
    }


    // Increment/decrement buttons

    // "Energy Decrement Click"
    private void OnEnergyDecClick(ClickEvent evt)
    {
        // decrement funding for Energy
    }

    private void OnEnergyIncClick(ClickEvent evt)
    {
        // increment funding for Energy

    }
}
