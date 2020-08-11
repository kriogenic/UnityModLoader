///NEXT: Figure out the RectTransforms correctly so that everything lines up and looks right.
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ModGUI
    {
        private GameObject can = null;
        private Dictionary<string, GameObject> panelsOnCanvas = new Dictionary<string, GameObject>();
        private Dictionary<string, Text> textsOnCanvas = new Dictionary<string, Text>();
        private Dictionary<string, Button> buttonsOnCanvas = new Dictionary<string, Button>();
        private Dictionary<string, Dropdown> dropdownsOnCanvas = new Dictionary<string, Dropdown>();
        private Dictionary<string, Image> imagesOnCanvas = new Dictionary<string, Image>();


        public Dictionary<string, Dropdown> DropdownDictionary
        {
            get { return dropdownsOnCanvas; }
        }

        public Dictionary<string, Button> ButtonDictionary
        {
            get { return buttonsOnCanvas; }
        }
        public Dictionary<string, Text> TextDictionary
        {
            get { return textsOnCanvas; }
        }

        public Dictionary<string, Image> ImageDictionary
        {
            get { return imagesOnCanvas; }
        }
        public Dictionary<string, GameObject> PanelDictionary
        {
            get { return panelsOnCanvas; }
        }
        public GameObject Canvas
        {
            get { return can; }
        }

        public ModGUI(string canName)
        {
            createCanvas(canName);
        }


        public GameObject getPanelWithName(string name)
        {
            if (panelsOnCanvas.ContainsKey(name))
            {
                return panelsOnCanvas[name];
            }

            return null;
        }

        public Text getLabelWithName(string name)
        {
            if (textsOnCanvas.ContainsKey(name))
            {
                return textsOnCanvas[name];
            }

            return null;
        }

        public Button getButtonWithName(string name)
        {
            if (buttonsOnCanvas.ContainsKey(name))
            {
                return buttonsOnCanvas[name];
            }

            return null;
        }

        public Dropdown getDropdownWithName(string name)
        {
            if (dropdownsOnCanvas.ContainsKey(name))
            {
                return dropdownsOnCanvas[name];
            }

            return null;
        }

        public Image getImageWithName(string name)
        {
            if (imagesOnCanvas.ContainsKey(name))
            {
                return imagesOnCanvas[name];
            }
            return null;
        }

        private void createCanvas(string cname)
        {
            if (can != null)
            {
                can.name = cname;
                return;
            }



            Canvas[] canvass = GameObject.FindObjectsOfType<Canvas>();
            EventSystem[] eventsystems = GameObject.FindObjectsOfType<EventSystem>();
            foreach (Canvas c in canvass)
            {
                c.gameObject.SetActive(false);
            }
            foreach (EventSystem ES in eventsystems)
            {
                ES.gameObject.SetActive(false);
            }

            GameObject eventsystem = new GameObject();
            eventsystem.name = "EventSystemL";
            EventSystem e = eventsystem.AddComponent<EventSystem>();
            e.firstSelectedGameObject = null;
            e.sendNavigationEvents = true;
            e.pixelDragThreshold = 10;
            StandaloneInputModule SIM = eventsystem.AddComponent<StandaloneInputModule>();
            SIM.horizontalAxis = "Horizontal";
            SIM.verticalAxis = "Vertical";
            SIM.submitButton = "Submit";
            SIM.cancelButton = "Cancel";
            SIM.inputActionsPerSecond = 10;
            SIM.repeatDelay = 0.5f;
            SIM.forceModuleActive = false;


            can = new GameObject();
            can.name = cname;
            can.layer = 5;
            Canvas cans = can.AddComponent<Canvas>();
            cans.renderMode = RenderMode.ScreenSpaceOverlay;
            cans.pixelPerfect = false;
            cans.sortingOrder = 20000;
            cans.additionalShaderChannels = AdditionalCanvasShaderChannels.Tangent | AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal;
            CanvasScaler canScale = can.AddComponent<CanvasScaler>();
            canScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canScale.referenceResolution = new Vector2(1920, 1080);
            canScale.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canScale.matchWidthOrHeight = 1;
            canScale.referencePixelsPerUnit = 100;
            GraphicRaycaster graycast = can.AddComponent<GraphicRaycaster>();
            graycast.ignoreReversedGraphics = true;
            graycast.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        }

        public Text createLabel(GameObject labelParent, string labelName, string labelText, int fontSize = 24, Vector2? labelPosition = null, Vector2? labelWidthHeight = null)
        {

            GameObject textObj = new GameObject();
            textObj.name = labelName;
            textObj.transform.parent = labelParent.transform;
            textObj.layer = 5;
            RectTransform RT = textObj.AddComponent<RectTransform>();
            RT.pivot = new Vector2(0, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(0, 1);
            RT.sizeDelta = labelWidthHeight ?? new Vector2(640, 30);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = labelPosition ?? Vector2.zero;
            CanvasRenderer canR = textObj.AddComponent<CanvasRenderer>();
            //  canR.cullTransparentMesh = false;
            Text textText = textObj.AddComponent<Text>();
            textText.text = labelText;
            textText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            textText.fontSize = fontSize;
            textText.verticalOverflow = VerticalWrapMode.Truncate;
            textText.horizontalOverflow = HorizontalWrapMode.Wrap;
            textText.alignment = TextAnchor.UpperLeft;
            textText.color = Color.white;
            textText.raycastTarget = true;
            textText.maskable = true;

            textsOnCanvas.Add(labelName, textText);

            return textText;
        }

        public GameObject createPanel(string panelName, GameObject parent = null, Color? backColor = null, Vector2? Position = null, Vector2? WidthHeight = null)
        {
            if (parent == null)
                parent = can;



            GameObject panelObj = new GameObject();
            panelObj.name = panelName;
            panelObj.transform.parent = parent.transform;
            panelObj.layer = 5;
            RectTransform RT = panelObj.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(0, 1);
            RT.sizeDelta = WidthHeight ?? new Vector2(640, 1080);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = Position ?? new Vector2(0, 0);

            CanvasRenderer canR = panelObj.AddComponent<CanvasRenderer>();
            //  canR.cullTransparentMesh = false;

            Image img = panelObj.AddComponent<Image>();
            img.sprite = null;
            img.color = backColor ?? Color.black;
            img.material = null;
            img.raycastTarget = true;
            //   img.raycastPadding = Vector4.zero;
            img.maskable = true;

            panelsOnCanvas.Add(panelName, panelObj);


            return panelObj;
        }

        public Button createButton(GameObject parent, string name, string content, Vector2? Position = null, Vector2? WidthHeight = null)
        {
            GameObject mainButton = new GameObject();
            mainButton.name = name;
            mainButton.transform.SetParent(parent.transform);
            mainButton.layer = 5;

            GameObject textButton = new GameObject();
            textButton.name = "Text";
            textButton.transform.SetParent(mainButton.transform);
            textButton.layer = 5;



            RectTransform RT = mainButton.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(0, 1);
            RT.sizeDelta = WidthHeight ?? new Vector2(80, 40);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = Position ?? new Vector2(0, 0);
            CanvasRenderer CR = mainButton.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image dropdownImage = mainButton.AddComponent<Image>();
            dropdownImage.sprite = null;
            dropdownImage.color = Color.white;
            dropdownImage.material = null;
            dropdownImage.raycastTarget = true;
            dropdownImage.maskable = true;
            Button theButton = mainButton.AddComponent<Button>();
            theButton.interactable = true;
            theButton.targetGraphic = theButton.GetComponent<Image>();
            ColorBlock buttonColors = new ColorBlock();
            buttonColors.normalColor = Color.white;
            buttonColors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            buttonColors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            //  buttonColors.selectedColor = Color.white;
            buttonColors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            buttonColors.colorMultiplier = 1;
            buttonColors.fadeDuration = 0.1f;
            theButton.colors = buttonColors;
            Navigation theButtonNav = new Navigation();
            theButtonNav.mode = Navigation.Mode.Automatic | Navigation.Mode.Horizontal | Navigation.Mode.Vertical;
            theButton.navigation = theButtonNav;



            RT = textButton.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = Vector2.zero;
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = Vector2.zero;
            CR = textButton.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Text buttonText = textButton.AddComponent<Text>();
            buttonText.text = content;
            buttonText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            buttonText.fontSize = 14;
            buttonText.resizeTextForBestFit = true;
            buttonText.verticalOverflow = VerticalWrapMode.Truncate;
            buttonText.horizontalOverflow = HorizontalWrapMode.Wrap;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.black;
            buttonText.raycastTarget = true;
            buttonText.maskable = true;

            buttonsOnCanvas.Add(name, theButton);

            return theButton;
        }

        public Dropdown createDropdown(GameObject parent, string name, Vector2? Position = null, Vector2? WidthHeight = null)
        {

            //Create correct object heirarchy for a dropdown menu.
            GameObject mainDropdown = new GameObject();
            mainDropdown.name = name;
            mainDropdown.transform.SetParent(parent.transform);
            mainDropdown.layer = 5;
            mainDropdown.SetActive(false);

            GameObject labelDropdown = new GameObject();
            labelDropdown.name = "Label";
            labelDropdown.transform.SetParent(mainDropdown.transform);
            labelDropdown.layer = 5;

            GameObject templateDropdown = new GameObject();
            templateDropdown.name = "Template";
            templateDropdown.transform.SetParent(mainDropdown.transform);
            templateDropdown.layer = 5;
            GameObject viewportTemplate = new GameObject();
            viewportTemplate.name = "Viewport";
            viewportTemplate.transform.SetParent(templateDropdown.transform);
            viewportTemplate.layer = 5;
            GameObject contentViewport = new GameObject();
            contentViewport.name = "Content";
            contentViewport.transform.SetParent(viewportTemplate.transform);
            contentViewport.layer = 5;
            GameObject itemContent = new GameObject();
            itemContent.name = "Item";
            itemContent.transform.SetParent(contentViewport.transform);
            itemContent.layer = 5;
            GameObject backgroundItem = new GameObject();
            backgroundItem.name = "Item Background";
            backgroundItem.transform.SetParent(itemContent.transform);
            backgroundItem.layer = 5;
            GameObject labelItem = new GameObject();
            labelItem.name = "Item Label";
            labelItem.transform.SetParent(itemContent.transform);
            labelItem.layer = 5;

            GameObject scrollbarTemplate = new GameObject();
            scrollbarTemplate.name = "Scrollbar";
            scrollbarTemplate.transform.SetParent(templateDropdown.transform);
            scrollbarTemplate.layer = 5;
            GameObject slidingScrollbar = new GameObject();
            slidingScrollbar.name = "Sliding Area";
            slidingScrollbar.transform.SetParent(scrollbarTemplate.transform);
            slidingScrollbar.layer = 5;
            GameObject handleSliding = new GameObject();
            handleSliding.name = "Handle";
            handleSliding.transform.SetParent(slidingScrollbar.transform);
            handleSliding.layer = 5;
            ///////////////////////////////////////////////////
            ///

            //////////Setup all RectTransform
            RectTransform RT = mainDropdown.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(0, 1);
            RT.sizeDelta = WidthHeight ?? new Vector2(160, 30);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = Position ?? new Vector2(0, 0);
            CanvasRenderer CR = mainDropdown.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image dropdownImage = mainDropdown.AddComponent<Image>();
            dropdownImage.sprite = null;
            dropdownImage.color = Color.white;
            dropdownImage.material = null;
            dropdownImage.raycastTarget = true;
            dropdownImage.maskable = true;


            RT = labelDropdown.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(0, 0);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 0);
            CR = labelDropdown.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Text labelText = labelDropdown.AddComponent<Text>();
            labelText.text = "Option A";
            labelText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            labelText.fontSize = 14;
            labelText.resizeTextForBestFit = true;
            labelText.verticalOverflow = VerticalWrapMode.Truncate;
            labelText.horizontalOverflow = HorizontalWrapMode.Wrap;
            labelText.alignment = TextAnchor.UpperLeft;
            labelText.color = Color.black;
            labelText.raycastTarget = true;
            labelText.maskable = true;


            RT = templateDropdown.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 1f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 0);
            RT.sizeDelta = new Vector2(0, 150);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 2);
            CR = templateDropdown.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image templateImage = templateDropdown.AddComponent<Image>();
            templateImage.sprite = null;
            templateImage.color = Color.white;
            templateImage.material = null;
            templateImage.raycastTarget = true;
            templateImage.maskable = true;
            templateDropdown.SetActive(false);
            Canvas c = templateDropdown.AddComponent<Canvas>();
            c.overrideSorting = true;
            c.sortingOrder = 30000;
            GraphicRaycaster GRC = templateDropdown.AddComponent<GraphicRaycaster>();
            GRC.ignoreReversedGraphics = true;
            GRC.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            CanvasGroup CG = templateDropdown.AddComponent<CanvasGroup>();
            CG.alpha = 1;
            CG.interactable = true;
            CG.blocksRaycasts = true;
            CG.ignoreParentGroups = false;

            RT = viewportTemplate.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0, 1f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(18, 0);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 0);
            CR = viewportTemplate.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image viewportImage = viewportTemplate.AddComponent<Image>();
            viewportImage.sprite = null;
            viewportImage.color = Color.white;
            viewportImage.material = null;
            viewportImage.raycastTarget = true;
            viewportImage.maskable = true;
            Mask viewportMask = viewportTemplate.AddComponent<Mask>();

            RT = contentViewport.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 1f);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(0, 28);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 0);

            RT = itemContent.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0.5f);
            RT.anchorMax = new Vector2(1, 0.5f);
            RT.sizeDelta = new Vector2(0, 20);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 0);


            RT = backgroundItem.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(0, 0);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 0);
            CR = backgroundItem.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image backgroundImage = backgroundItem.AddComponent<Image>();
            viewportImage.sprite = null;
            viewportImage.color = Color.white;
            viewportImage.material = null;
            viewportImage.raycastTarget = true;
            viewportImage.maskable = true;

            RT = labelItem.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(10, 1);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(5, 2);
            CR = labelItem.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Text labelItemText = labelItem.AddComponent<Text>();
            labelItemText.text = "Option A";
            labelItemText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            labelItemText.fontSize = 14;
            labelItemText.verticalOverflow = VerticalWrapMode.Truncate;
            labelItemText.horizontalOverflow = HorizontalWrapMode.Wrap;
            labelItemText.alignment = TextAnchor.MiddleLeft;
            labelItemText.color = new Color(0.1960784f, 0.1960784f, 0.1960784f);
            labelItemText.raycastTarget = true;
            labelItemText.maskable = true;


            RT = scrollbarTemplate.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(1f, 1f);
            RT.anchorMin = new Vector2(1, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(20, 0);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(0, 0);
            CR = scrollbarTemplate.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image scrollbarImage = scrollbarTemplate.AddComponent<Image>();
            viewportImage.sprite = null;
            viewportImage.color = Color.white;
            viewportImage.material = null;
            viewportImage.raycastTarget = true;
            viewportImage.maskable = true;

            RT = slidingScrollbar.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(10, 10);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(10, 10);

            RT = handleSliding.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0.5f, 0.5f);
            RT.anchorMin = new Vector2(0, 0);
            RT.anchorMax = new Vector2(1, 1);
            RT.sizeDelta = new Vector2(-10, -10);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = new Vector2(-10, -10);
            CR = handleSliding.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image handleImage = handleSliding.AddComponent<Image>();
            viewportImage.sprite = null;
            viewportImage.color = Color.white;
            viewportImage.material = null;
            viewportImage.raycastTarget = true;
            viewportImage.maskable = true;


            Scrollbar scrollbarScrollbar = scrollbarTemplate.AddComponent<Scrollbar>();
            scrollbarScrollbar.interactable = true;
            scrollbarScrollbar.transition = Selectable.Transition.ColorTint;
            scrollbarScrollbar.targetGraphic = handleSliding.GetComponent<Image>();
            ColorBlock scrollbarColors = new ColorBlock();
            scrollbarColors.normalColor = Color.white;
            scrollbarColors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            scrollbarColors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            //    scrollbarColors.selectedColor = Color.white;
            scrollbarColors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            scrollbarColors.colorMultiplier = 1;
            scrollbarColors.fadeDuration = 0.1f;
            scrollbarScrollbar.colors = scrollbarColors;
            Navigation scrollbarNav = new Navigation();
            scrollbarNav.mode = Navigation.Mode.Automatic | Navigation.Mode.Horizontal | Navigation.Mode.Vertical;
            scrollbarScrollbar.navigation = scrollbarNav;
            scrollbarScrollbar.handleRect = handleSliding.GetComponent<RectTransform>();
            scrollbarScrollbar.direction = Scrollbar.Direction.BottomToTop;
            scrollbarScrollbar.value = 1;
            scrollbarScrollbar.size = 1;
            scrollbarScrollbar.numberOfSteps = 0;

            Toggle itemToggle = itemContent.AddComponent<Toggle>();
            itemToggle.interactable = true;
            itemToggle.transition = Selectable.Transition.ColorTint;
            itemToggle.targetGraphic = backgroundItem.GetComponent<Image>();
            ColorBlock toggleColors = new ColorBlock();
            toggleColors.normalColor = Color.white;
            toggleColors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            toggleColors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            // toggleColors.selectedColor = Color.white;
            toggleColors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            toggleColors.colorMultiplier = 1;
            toggleColors.fadeDuration = 0.1f;
            itemToggle.colors = toggleColors;
            Navigation toggleNav = new Navigation();
            toggleNav.mode = Navigation.Mode.Horizontal | Navigation.Mode.Vertical | Navigation.Mode.Automatic;
            itemToggle.navigation = toggleNav;
            itemToggle.isOn = true;
            itemToggle.toggleTransition = Toggle.ToggleTransition.Fade;
            itemToggle.graphic = null;
            itemToggle.group = null;

            //Add Scroll Rect
            ScrollRect SR = templateDropdown.AddComponent<ScrollRect>();
            SR.content = contentViewport.GetComponent<RectTransform>();
            SR.horizontal = false;
            SR.vertical = true;
            SR.movementType = ScrollRect.MovementType.Clamped;
            SR.inertia = true;
            SR.decelerationRate = 0.135f;
            SR.scrollSensitivity = 1;
            SR.viewport = viewportTemplate.GetComponent<RectTransform>();
            SR.horizontalScrollbar = null;
            SR.verticalScrollbar = scrollbarTemplate.GetComponent<Scrollbar>();
            SR.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            SR.verticalScrollbarSpacing = -3;

            ///Add Dropdown Component Last
            Dropdown dropdown = mainDropdown.AddComponent<Dropdown>();
            dropdown.interactable = true;
            dropdown.transition = Selectable.Transition.ColorTint;
            dropdown.targetGraphic = dropdownImage;
            ColorBlock dropdownColors = new ColorBlock();
            dropdownColors.normalColor = Color.white;
            dropdownColors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            dropdownColors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            //   dropdownColors.selectedColor = Color.white;
            dropdownColors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            dropdownColors.colorMultiplier = 1;
            dropdownColors.fadeDuration = 0.1f;
            dropdown.colors = dropdownColors;
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Automatic | Navigation.Mode.Horizontal | Navigation.Mode.Vertical;
            dropdown.navigation = nav;
            dropdown.template = templateDropdown.GetComponent<RectTransform>();
            dropdown.captionText = labelDropdown.GetComponent<Text>();
            dropdown.captionImage = null;
            dropdown.itemText = labelItem.GetComponent<Text>();
            dropdown.itemImage = null;
            dropdown.value = 0;
            //   dropdown.alphaFadeSpeed = 0.15f;
            mainDropdown.SetActive(true);

            dropdownsOnCanvas.Add(name, dropdown);
            return dropdown;
        }

        public Image createImageFromPath(GameObject parent, string name, string spritePath = null, Vector2? Position = null, Vector2? WidthHeight = null)
        {
            Sprite graphic = null;
            if (spritePath != null)
            {
                graphic = getSpriteFromFile(spritePath);
            }
            GameObject imgObj = new GameObject();
            imgObj.name = name;
            imgObj.transform.SetParent(parent.transform);
            imgObj.layer = 5;
            RectTransform RT = imgObj.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(0, 1);
            RT.sizeDelta = WidthHeight ?? new Vector2(100, 100);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = Position ?? new Vector2(0, 0);
            CanvasRenderer CR = imgObj.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image theImage = imgObj.AddComponent<Image>();
            theImage.sprite = graphic;
            theImage.color = Color.white;
            theImage.material = null;
            theImage.raycastTarget = true;
            theImage.maskable = true;


            imagesOnCanvas.Add(name, theImage);
            return theImage;
        }
        public Image createImageFromSprite(GameObject parent, string name, Sprite graphic = null, Vector2? Position = null, Vector2? WidthHeight = null)
        {
            GameObject imgObj = new GameObject();
            imgObj.name = name;
            imgObj.transform.SetParent(parent.transform);
            imgObj.layer = 5;
            RectTransform RT = imgObj.AddComponent<RectTransform>();
            RT.position = new Vector3(0, 0, 0);
            RT.pivot = new Vector2(0, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchorMax = new Vector2(0, 1);
            RT.sizeDelta = WidthHeight ?? new Vector2(100, 100);
            RT.localScale = new Vector3(1, 1, 1);
            RT.anchoredPosition = Position ?? new Vector2(0, 0);
            CanvasRenderer CR = imgObj.AddComponent<CanvasRenderer>();
            CR.cull = false;
            Image theImage = imgObj.AddComponent<Image>();
            theImage.sprite = graphic;
            theImage.color = Color.white;
            theImage.material = null;
            theImage.raycastTarget = true;
            theImage.maskable = true;


            imagesOnCanvas.Add(name, theImage);
            return theImage;
        }

        private Sprite getSpriteFromFile(string fileName)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(fileName))
            {
                FileData = File.ReadAllBytes(fileName);
                Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                {
                    Sprite NewSprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0), 100, 0, SpriteMeshType.FullRect);
                    return NewSprite;
                }
            }
            return null;
        }
    }

}
