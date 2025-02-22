using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(AudioSource))]
public class HelpUI : MonoBehaviour
{
    private UIDocument doc;
    [SerializeField] GameLoop gameLoop;
    [SerializeField] TheGameUI gui;      // stands for game-UI lol

    [SerializeField] private List<VisualElement> helpSlides = new();
    AudioSource audioSource;

    private List<Button> buttons = new();

    private Label slideCountLabel;

    private int selectedSlide;

    private Button backButton;
    private Button forwardButton;

    public bool isVisible;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        doc = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();
        selectedSlide = 0;
        GetSlides();
        InitAll();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isVisible)
        {
            ToggleVisibility();
            audioSource.Play();
        }
    }

    void InitAll()
    {
        VisualElement root = doc.rootVisualElement;
        backButton = root.Q<Button>("help-back");
        forwardButton = root.Q<Button>("help-forward");

        backButton.RegisterCallback<ClickEvent>(OnBackButtonClick);
        forwardButton.RegisterCallback<ClickEvent>(OnForwardButtonClick);
        buttons = root.Query<Button>().ToList();
        foreach (Button b in buttons)
        {
            b.RegisterCallback<ClickEvent>(OnAllButtonClick);
        }

        slideCountLabel = root.Q<Label>("slide-counter");
        ChangeSlide(0);

        isVisible = true;
        UpdateVisibility();
    }

    void Onsable()
    {
        backButton.UnregisterCallback<ClickEvent>(OnBackButtonClick);
        forwardButton.UnregisterCallback<ClickEvent>(OnForwardButtonClick);
        foreach (Button b in buttons)
        {
            b.UnregisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        audioSource.Play();
    }

    private void OnBackButtonClick(ClickEvent evt)
    {
        ChangeSlide(-1);
    }

    private void OnForwardButtonClick(ClickEvent evt)
    {
        ChangeSlide(1);
    }


    private void GetSlides()
    {
        // add sldes to helpSlides
        helpSlides = doc.rootVisualElement.Query<VisualElement>().Class("help-slide").ToList();
        // sort them by name
        helpSlides.Sort((a, b) => string.Compare(a.name, b.name));
        // foreach (VisualElement slide in helpSlides)
        // {
        //     Debug.Log($"{slide.name}");
        // }
    }

    public void ChangeSlide(int amt)
    {
        // amt will always equal 0, 1 or -1
        if (amt != 0)
        {
            amt /= Mathf.Abs(amt);
        }
        selectedSlide += amt;
        if (selectedSlide < 0)
        {
            selectedSlide = helpSlides.Count - 1;
        }
        else if (selectedSlide >= helpSlides.Count())
        {
            selectedSlide = 0;
        }

        // hide all slides
        foreach (var slide in helpSlides)
        {
            slide.style.opacity = 0f;
        }
        // display new slide
        helpSlides[selectedSlide].style.opacity = 1;
        slideCountLabel.text = $"{selectedSlide + 1}/{helpSlides.Count()}";
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        if (isVisible)
        {
            doc.rootVisualElement.style.display = DisplayStyle.Flex;
            //doc.rootVisualElement.pickingMode = PickingMode.Position;
            ChangeSlide(0);
        }
        else
        {
            doc.rootVisualElement.style.display = DisplayStyle.None;
            //doc.rootVisualElement.pickingMode = PickingMode.Ignore;
        }
    }
}
