using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _UI
{
    public class InformationDisplay : MonoBehaviour
    {
        [Header("UI Refs")]
        public TextMeshProUGUI title;
        public TextMeshProUGUI body;
        public Button contButton;
    
        [Header("Data")]
        [TextArea] public string startTitleText;
        [TextArea] public string startBodyText;

        private void Start()
        {
            if (title != null)
            {
                title.text = startTitleText;
            }
            else
            {
                Debug.LogWarning("Title Ref not set!");
            }

            if (body != null)
            {
                body.text = startBodyText;
            }
            else
            {
                Debug.LogWarning("Body Ref not set!");
            }

            if (contButton != null)
            {
                contButton.onClick.AddListener(OnButtonClick);
            }
            else
            {
                Debug.LogWarning("Continue Button Ref not set!");
            }
        }

        private void OnButtonClick()
        {
            Destroy(transform.root.gameObject);
        }
        
        public void SetTitle(string newTitle)
        {
            if (title == null) return;
            
            startTitleText = newTitle;
            title.text = startTitleText;
        }
        
        public void SetBody(string newBody)
        {
            if (body == null) return;
            
            startBodyText = newBody;
            body.text = startBodyText;
        }
    }
}
