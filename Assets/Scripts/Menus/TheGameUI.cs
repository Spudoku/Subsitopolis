using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(GameLoop))]
[RequireComponent(typeof(AudioSource))]
public class TheGameUI : MonoBehaviour
{
    const float FUNDING_DELTA_AMOUNT = 0.25f;
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

    // TOTAL FUNDING LABELS
    private Label energyFundingLabel;

    private Label foodFundingLabel;

    private Label waterFundingLabel;


    // FUDING LABELS

    void Start()
    {
        doc = GetComponent<UIDocument>();
        gameLoop = GetComponent<GameLoop>();

        audioSource = GetComponent<AudioSource>();

        // references to buttons:
        // Button button = doc.rootVisualElement.Q("name_of_button") as button
        // onclick event:
        // button.RegisterCallback<ClickEvent>();

        // REGISTERING BUTTONS
        energyDecButton = doc.rootVisualElement.Q<Button>("energy-dec");
        energyIncButton = doc.rootVisualElement.Q<Button>("energy-inc");
        energyDecButton.RegisterCallback<ClickEvent>(OnEnergyDecClick);
        energyIncButton.RegisterCallback<ClickEvent>(OnEnergyIncClick);

        waterDecButton = doc.rootVisualElement.Q<Button>("water-dec");
        waterIncButton = doc.rootVisualElement.Q<Button>("water-inc");
        waterDecButton.RegisterCallback<ClickEvent>(OnWaterDecClick);
        waterIncButton.RegisterCallback<ClickEvent>(OnWaterIncClick);

        foodDecButton = doc.rootVisualElement.Q<Button>("food-dec");
        foodIncButton = doc.rootVisualElement.Q<Button>("food-inc");
        foodDecButton.RegisterCallback<ClickEvent>(OnFoodDecClick);
        foodIncButton.RegisterCallback<ClickEvent>(OnFoodIncClick);

        // registering labels
        energyProdLabel = doc.rootVisualElement.Q<Label>("energy-prod");
        energyDemLabel = doc.rootVisualElement.Q<Label>("energy-dem");
        energyFundingLabel = doc.rootVisualElement.Q<Label>("energy-funding");

        waterProdLabel = doc.rootVisualElement.Q<Label>("water-prod");
        waterDemLabel = doc.rootVisualElement.Q<Label>("water-dem");
        waterFundingLabel = doc.rootVisualElement.Q<Label>("water-funding");

        foodProdLabel = doc.rootVisualElement.Q<Label>("food-prod");
        foodDemLabel = doc.rootVisualElement.Q<Label>("food-dem");
        foodFundingLabel = doc.rootVisualElement.Q<Label>("food-funding");

        // register callbacks
        buttons = doc.rootVisualElement.Query<Button>().ToList();
        foreach (Button b in buttons)
        {
            b.RegisterCallback<ClickEvent>(OnAllButtonClick);
        }

        // update labels
        UpdateEnergyFunding(0);
    }

    // Update all production/demand labels
    public void Tick()
    {
        energyProdLabel.text = $"{gameLoop.energyProduction}";
        energyDemLabel.text = $"{Round.RoundToPlaces(gameLoop.energyDemand, 2)}";
    }

    void OnDsable()
    {
        // unresgister
        // button.UnregisterCallback<ClickEvent>(OnPlayGameClick);

        // INCREMENT/DECREMENT buttons
        energyDecButton.UnregisterCallback<ClickEvent>(OnEnergyDecClick);
        energyIncButton.UnregisterCallback<ClickEvent>(OnEnergyIncClick);

        waterDecButton.UnregisterCallback<ClickEvent>(OnWaterDecClick);
        waterIncButton.UnregisterCallback<ClickEvent>(OnWaterIncClick);

        waterDecButton.UnregisterCallback<ClickEvent>(OnWaterDecClick);
        waterIncButton.UnregisterCallback<ClickEvent>(OnWaterIncClick);

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
        UpdateEnergyFunding(-FUNDING_DELTA_AMOUNT);
    }

    private void OnEnergyIncClick(ClickEvent evt)
    {
        // increment funding for Energy
        UpdateEnergyFunding(FUNDING_DELTA_AMOUNT);
    }

    private void OnWaterDecClick(ClickEvent evt)
    {
        UpdateWaterFunding(-FUNDING_DELTA_AMOUNT);
    }

    private void OnWaterIncClick(ClickEvent evt)
    {
        UpdateWaterFunding(FUNDING_DELTA_AMOUNT);
    }

    private void OnFoodDecClick(ClickEvent evt)
    {
        UpdateFoodFunding(-FUNDING_DELTA_AMOUNT);
    }

    private void OnFoodIncClick(ClickEvent evt)
    {
        UpdateFoodFunding(FUNDING_DELTA_AMOUNT);
    }


    private void UpdateEnergyFunding(float amount)
    {
        gameLoop.energyFunding += amount;
        if (gameLoop.energyFunding < 0f)
        {
            gameLoop.energyFunding = 0f;
        }
        energyFundingLabel.text = $"{Round.RoundToPlaces(gameLoop.energyFunding, 2)}";
    }

    private void UpdateWaterFunding(float amount)
    {
        gameLoop.waterFunding += amount;
        if (gameLoop.waterFunding < 0f)
        {
            gameLoop.waterFunding = 0f;
        }
        waterFundingLabel.text = $"{Round.RoundToPlaces(gameLoop.waterFunding, 2)}";
    }

    private void UpdateFoodFunding(float amount)
    {
        gameLoop.foodFunding += amount;
        if (gameLoop.foodFunding < 0f)
        {
            gameLoop.foodFunding = 0f;
        }
        foodFundingLabel.text = $"{Round.RoundToPlaces(gameLoop.foodFunding, 2)}";
    }

}
