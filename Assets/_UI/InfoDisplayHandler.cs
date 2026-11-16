using UnityEngine;
using UnityEngine.UIElements;

namespace _UI
{
    public class InfoDisplayHandler : MonoBehaviour
    {
        private PanelRenderer _panelRenderer;
        private Label _title;
        private Label _info;
        private Button _contButton;
    
        [TextArea] public string title;
        [TextArea] public string info;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }

        private void OnDestroy()
        {
            if (_panelRenderer)
                _panelRenderer.UnregisterUIReloadCallback(OnUIReload);

            _contButton?.UnregisterCallback<ClickEvent>(OnContinueButtonClicked);
        }

        private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
        {
            _title = root.Q<Label>("Title");
            _info = root.Q<Label>("Info");
            _contButton = root.Q<Button>("Continue-Button");
        
            _title.text = title;
            _info.text = info;
        
            if (_contButton != null)
                _contButton.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked(ClickEvent e)
        {
            gameObject.SetActive(false);
        }

        async public void SetTitle(string newTitle)
        {
            await Awaitable.WaitForSecondsAsync(0.5f);
            if (_title == null)
            {
                Debug.LogWarning("Title not found!");
                return;
            }
            title = newTitle;
            _title.text = newTitle;
        }
    }
}
