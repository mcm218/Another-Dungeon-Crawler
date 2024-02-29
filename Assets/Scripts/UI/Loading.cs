using _Scripts.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

    
[RequireComponent(typeof(UIDocument))]
public class Loading : MonoBehaviour {
    [SerializeField]
    private UIDocument document;
    
    [SerializeField]
    private StyleSheet styleSheet;

    [SerializeField]
    private UnityEvent onStart;

    private void Awake() {
        StartCoroutine(GenerateUI());
    }

    private void OnEnable() {
        StartCoroutine(GenerateUI());
    }

    private void OnValidate() {
        if (Application.isPlaying) return;

        StartCoroutine(GenerateUI());
    }

    private IEnumerator GenerateUI() {
        yield return null;

        var root = document.rootVisualElement;
        root.Clear();
        root.styleSheets.Add(styleSheet);

        root.Add(
                 UI.Create("flex justify-center items-center flex-col h-full w-full")
                   .Push(
                         UI.Create("rounded-lg bg-gray-900 p-6 text-sm")
                           .Push(
                                 // title
                                 UI.Create<Label>("text-center text-2xl font-bold text-purple-200 mb-4")
                                   .Text("Loading...")
                                    )
                        )
                   .RecursiveBuild(out var mainMenuContainer)
                );
    }
}