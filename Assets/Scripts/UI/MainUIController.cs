using UnityEngine;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public GameObject[] panels;
        public static MainUIController Instance;
        void Awake()
        {
            if (Instance == null)
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }


        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
            {
                var active = i == index;
                var g = panels[i];
                if (g.activeSelf != active) g.SetActive(active);
            }
        }

        void OnEnable()
        {
            SetActivePanel(0);
        }
    }
}