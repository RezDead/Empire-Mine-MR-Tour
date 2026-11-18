using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace _UI
{
    public class StatusDisplay : MonoBehaviour
    {
        public PanelRenderer panelRenderer;
        [TextArea] public string title;
        
        private Label _statusLabel;

        private void Awake()
        {
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }

        private void OnDestroy()
        {
            if (panelRenderer)
                panelRenderer.UnregisterUIReloadCallback(OnUIReload);

        }

        private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
        {
            _statusLabel = root.Q<Label>("Title");
            _statusLabel.text = title;
        }
        
        async public void SetTitle(string newTitle)
        {
            await Awaitable.WaitForSecondsAsync(0.5f);
            if (_statusLabel == null)
            {
                Debug.LogWarning("Status label not found!");
                return;
            }
            title = newTitle;
            _statusLabel.text = newTitle;
        }
    }
}
