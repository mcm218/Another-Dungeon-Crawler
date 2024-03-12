using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts.Utilities {
    public class LoadingSceneController : Singleton<LoadingSceneController> {
        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private StyleSheet styleSheet;

        protected override void Awake() {
            base.Awake();

            if (document == null) {
                document = GetComponent<UIDocument>();
                if (document == null) { document = gameObject.AddComponent<UIDocument>(); }
            }

            if (styleSheet == null) {
                // styleSheet = Resources.Load<StyleSheet>("");
            }
        }

        public void Disable() {
            var root = document.rootVisualElement;
            root.Clear();
        }

        public void Enable() {
            StartCoroutine(GenerateUI());
        }

        public void Toggle() {
            if (document.rootVisualElement.childCount > 0) { Disable(); }
            else { Enable(); }
        }

        private IEnumerator GenerateUI() {
            yield return null;

            var root = document.rootVisualElement;
            root.Clear();
            if (styleSheet != null) root.styleSheets.Add(styleSheet);

            root.Add(
                     UI.Create()
                       .Style("flex justify-center items-center flex-col h-full w-full bg-black")
                       .Push(
                             UI.Create()
                               .Style("rounded-lg bg-gray-900 p-6 text-sm")
                               .Push(
                                     // title
                                     UI.Create<Label>()
                                       .Style("text-center text-2xl font-bold text-purple-200 mb-4")
                                       .Text("Loading...")
                                    )
                            )
                       .RecursiveBuild(out var loadingMenuContainer)
                    );

        }
    }
}