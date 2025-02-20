using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
[RequireComponent(typeof(GameLoop))]
[RequireComponent(typeof(AudioSource))]

// TODO: MouseOverEvents for stuff!
public class TheGameUI : MonoBehaviour
{
    const float FUNDING_DELTA_AMOUNT = 0.25f;
    const float TAX_DELTA_AMOUNT = 0.01f;
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


    // Tax/Debt Labels
    private Label taxAmtLabel;              // how much is earned through taxes
    private Label taxRateLabel;             // rate of citizen income that is taxed

    private Label debtFundingLabel;
    private Label debtAmtLabel;
    void Awake()
    {
        doc = GetComponent<UIDocument>();
        gameLoop = GetComponent<GameLoop>();

        audioSource = GetComponent<AudioSource>();


        InitAll();

    }

    // Update all production/demand labels
    public void Tick()
    {
        energyProdLabel.text = $"{Round.RoundToPlaces(gameLoop.energyProduction, 2)}";
        energyDemLabel.text = $"{Round.RoundToPlaces(gameLoop.energyDemand, 2)}";

        waterProdLabel.text = $"{Round.RoundToPlaces(gameLoop.waterProduction, 2)}";
        waterDemLabel.text = $"{Round.RoundToPlaces(gameLoop.waterDemand, 2)}";

        foodProdLabel.text = $"{Round.RoundToPlaces(gameLoop.foodProduction, 2)}";
        foodDemLabel.text = $"{Round.RoundToPlaces(gameLoop.foodDemand, 2)}";

        taxAmtLabel.text = $"{Round.RoundToPlaces(gameLoop.taxRate * gameLoop.population * gameLoop.citizenIncome, 2)}";
        taxRateLabel.text = $"{Round.RoundToPlaces(gameLoop.taxRate * 100f, 2)}";

        debtAmtLabel.text = $"{Round.RoundToPlaces(gameLoop.totalDebt, 2)}";
        debtFundingLabel.text = $"{Round.RoundToPlaces(gameLoop.debtFunding, 2)}";
        // tax labels
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

        taxDecButton.UnregisterCallback<ClickEvent>(OnTaxDecClick);
        taxIncButton.UnregisterCallback<ClickEvent>(OnTaxIncClick);

        debtDecButton.UnregisterCallback<ClickEvent>(OnDebtDecClick);
        debtIncButton.UnregisterCallback<ClickEvent>(OnDebtIncClick);

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

    private void OnTaxDecClick(ClickEvent evt)
    {
        UpdateTaxRate(-TAX_DELTA_AMOUNT);
    }

    private void OnTaxIncClick(ClickEvent evt)
    {
        UpdateTaxRate(TAX_DELTA_AMOUNT);
    }

    private void OnDebtDecClick(ClickEvent evt)
    {
        UpdateDebtFunding(-FUNDING_DELTA_AMOUNT);
    }

    private void OnDebtIncClick(ClickEvent evt)
    {
        UpdateDebtFunding(FUNDING_DELTA_AMOUNT);
    }



    public void UpdateAllLabels()
    {
        // update labels
        UpdateEnergyFunding(0);
        UpdateWaterFunding(0);
        UpdateFoodFunding(0);
        UpdateTaxRate(0);

        energyProdLabel.text = $"{Round.RoundToPlaces(gameLoop.energyProduction, 2)}";
        energyDemLabel.text = $"{Round.RoundToPlaces(gameLoop.energyDemand, 2)}";

        waterProdLabel.text = $"{Round.RoundToPlaces(gameLoop.waterProduction, 2)}";
        waterDemLabel.text = $"{Round.RoundToPlaces(gameLoop.waterDemand, 2)}";

        foodProdLabel.text = $"{Round.RoundToPlaces(gameLoop.foodProduction, 2)}";
        foodDemLabel.text = $"{Round.RoundToPlaces(gameLoop.foodDemand, 2)}";


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

    private void UpdateTaxRate(float amount)
    {
        gameLoop.taxRate += amount;
        if (gameLoop.taxRate < 0f)
        {
            gameLoop.taxRate = 0f;
        }
        else if (gameLoop.taxRate > 1.0f)
        {
            gameLoop.taxRate = 1f;
        }
        taxRateLabel.text = $"{gameLoop.taxRate * 100f}%";
    }

    private void UpdateDebtFunding(float amount)
    {
        gameLoop.debtFunding += amount;
        if (gameLoop.debtFunding < 0f)
        {
            gameLoop.debtFunding = 0f;
        }
        debtFundingLabel.text = $"{Round.RoundToPlaces(gameLoop.debtFunding, 2)}";
    }


    public void InitAll()
    {
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

        taxDecButton = doc.rootVisualElement.Q<Button>("tax-dec");
        taxIncButton = doc.rootVisualElement.Q<Button>("tax-inc");
        taxDecButton.RegisterCallback<ClickEvent>(OnTaxDecClick);
        taxIncButton.RegisterCallback<ClickEvent>(OnTaxIncClick);

        debtDecButton = doc.rootVisualElement.Q<Button>("debt-dec");
        debtIncButton = doc.rootVisualElement.Q<Button>("debt-inc");
        debtDecButton.RegisterCallback<ClickEvent>(OnDebtDecClick);
        debtIncButton.RegisterCallback<ClickEvent>(OnDebtIncClick);

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

        taxRateLabel = doc.rootVisualElement.Q<Label>("tax-rate");
        taxAmtLabel = doc.rootVisualElement.Q<Label>("tax-prod");

        debtAmtLabel = doc.rootVisualElement.Q<Label>("debt-amt");
        debtFundingLabel = doc.rootVisualElement.Q<Label>("debt-funding");

        // register callbacks
        buttons = doc.rootVisualElement.Query<Button>().ToList();
        foreach (Button b in buttons)
        {
            b.RegisterCallback<ClickEvent>(OnAllButtonClick);
        }

        // update labels
        UpdateEnergyFunding(0);
        UpdateWaterFunding(0);
        UpdateFoodFunding(0);
        UpdateTaxRate(0);
        UpdateDebtFunding(0);
    }
}
