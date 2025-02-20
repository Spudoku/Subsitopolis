using System;
using System.Collections.Generic;
using UnityEditor.Search;
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

    // icon textures
    [SerializeField] private List<Texture2D> approvalIcons;

    // Vitals stuff
    private VisualElement approvalIcon;
    private Label approvalAmtLabel;
    public List<Color32> approvalLabelColors = new();

    private Label popAmtLabel;
    private Label TreasuryAmtLabel;

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

        UpdateApproval();
        UpdatePopulation();
        UpdateTreasury();
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

    private void UpdateApproval()
    {
        approvalAmtLabel.text = $"{Round.RoundToPlaces(gameLoop.approval * 100, 1)}%";
        if (gameLoop.approval > 0.75f)
        {
            // happy
            approvalAmtLabel.style.color = new StyleColor(approvalLabelColors[0]);
            approvalIcon.style.backgroundImage = approvalIcons[0];
        }
        else if (gameLoop.approval > 0.5f)
        {
            // mid
            approvalAmtLabel.style.color = new StyleColor(approvalLabelColors[1]);
            approvalIcon.style.backgroundImage = approvalIcons[1];
        }
        else if (gameLoop.approval > 0.4f)
        {
            // mildly angry
            approvalAmtLabel.style.color = new StyleColor(approvalLabelColors[2]);
            approvalIcon.style.backgroundImage = approvalIcons[2];
        }
        else if (gameLoop.approval > 0.33f)
        {
            // pissed
            approvalAmtLabel.style.color = new StyleColor(approvalLabelColors[3]);
            approvalIcon.style.backgroundImage = approvalIcons[3];
        }
        else
        {
            // you're fucked
            approvalAmtLabel.style.color = new StyleColor(approvalLabelColors[4]);
            approvalIcon.style.backgroundImage = approvalIcons[4];
        }
    }
    private void UpdateTreasury()
    {
        TreasuryAmtLabel.text = $"${Round.RoundToPlaces(gameLoop.treasury, 2)}M";
        if (gameLoop.treasury > gameLoop.totalDebt * 5)
        {
            // happy
            TreasuryAmtLabel.style.color = new StyleColor(approvalLabelColors[0]);

        }
        else if (gameLoop.treasury > gameLoop.totalDebt * 2)
        {
            // mid
            TreasuryAmtLabel.style.color = new StyleColor(approvalLabelColors[1]);

        }
        else if (gameLoop.treasury > gameLoop.totalDebt)
        {
            // mildly angry
            TreasuryAmtLabel.style.color = new StyleColor(approvalLabelColors[2]);

        }
        else if (gameLoop.treasury > gameLoop.totalDebt * 0.5)
        {
            // pissed
            TreasuryAmtLabel.style.color = new StyleColor(approvalLabelColors[3]);

        }
        else
        {
            // you're fucked
            TreasuryAmtLabel.style.color = new StyleColor(approvalLabelColors[4]);

        }
    }

    private void UpdatePopulation()
    {
        if (gameLoop.population > 1000000)
        {
            popAmtLabel.text = $"{Round.RoundToPlaces(gameLoop.population / 1000000, 2)}M";
        }
        else if (gameLoop.population > 1000)
        {
            popAmtLabel.text = $"{Round.RoundToPlaces(gameLoop.population / 1000, 2)}K";
        }
        else
        {
            popAmtLabel.text = $"{Round.RoundToPlaces(gameLoop.population, 2)}";
        }

        if (gameLoop.population > GameLoop.STARTING_POPULATION * 2)
        {
            // happy
            popAmtLabel.style.color = new StyleColor(approvalLabelColors[0]);

        }
        else if (gameLoop.population > GameLoop.STARTING_POPULATION)
        {
            // mid
            popAmtLabel.style.color = new StyleColor(approvalLabelColors[1]);

        }
        else if (gameLoop.population > GameLoop.STARTING_POPULATION * 0.75)
        {
            // mildly angry
            popAmtLabel.style.color = new StyleColor(approvalLabelColors[2]);

        }
        else if (gameLoop.population > GameLoop.STARTING_POPULATION * 0.33)
        {
            // pissed
            popAmtLabel.style.color = new StyleColor(approvalLabelColors[3]);

        }
        else
        {
            // you're fucked
            popAmtLabel.style.color = new StyleColor(approvalLabelColors[4]);

        }
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

        // Approval Stuff
        approvalIcon = doc.rootVisualElement.Q<VisualElement>("approval-icon");
        approvalAmtLabel = doc.rootVisualElement.Q<Label>("approval-perc");

        popAmtLabel = doc.rootVisualElement.Q<Label>("pop-amt");
        TreasuryAmtLabel = doc.rootVisualElement.Q<Label>("treasury-amt");

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
        UpdateApproval();
        UpdatePopulation();
        UpdateTreasury();
    }
}
