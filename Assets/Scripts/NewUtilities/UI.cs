using UnityEngine.UIElements;

namespace _Scripts.Utilities {
    public static class UI {
        public static VisualElement Create(string classes)
        {
            return Create<VisualElement>(classes);
        }
        public static VisualElement Create(params string[] classNames)
        {
            return Create<VisualElement>(classNames);
        }

        public static T Create<T>(string classes) where T : VisualElement, new()
        {
            var ele = new T();
            foreach (var className in classes.Split(' '))
            {
                ele.AddToClassList(className);
            }

            return ele;
        }

        public static T Create<T>(params string[] classNames) where T : VisualElement, new()
        {
            var ele = new T();
            foreach (var className in classNames)
            {
                foreach(var clss in className.Split(' '))
                {
                    ele.AddToClassList(clss);
                }
            }

            return ele;
        }
        public static T AddHoverClass<T> (this T ele, string hoverClass) where T : VisualElement
        {
            ele.RegisterCallback<MouseEnterEvent>(evt => {
                foreach (var className in hoverClass.Split(' '))
                {
                    ele.AddToClassList(className);
                }
            });
            ele.RegisterCallback<MouseLeaveEvent>(evt => {
                foreach (var className in hoverClass.Split(' '))
                {
                    ele.RemoveFromClassList(className);
                }
            });
            return ele;
        }
        
        
        public static T AddHoverClasses<T> (this T ele, params string[] hoverClasses) where T : VisualElement
        {
            foreach (var hoverClass in hoverClasses)
            {
                ele.AddHoverClass(hoverClass);
            }
            return ele;
        }
        
        // AddHoverClass as extension method to VisualElement
        public static VisualElement AddHoverClass (this VisualElement ele, string hoverClass)
        {
            ele.RegisterCallback<MouseEnterEvent>(evt => {
                foreach (var className in hoverClass.Split(' '))
                {
                    ele.AddToClassList(className);
                }
            });
            ele.RegisterCallback<MouseLeaveEvent>(evt => {
                foreach (var className in hoverClass.Split(' '))
                {
                    ele.RemoveFromClassList(className);
                }
            });
            return ele;
        }
        
        // AddHoverClasses as extension method to VisualElement
        public static VisualElement AddHoverClasses (this VisualElement ele, params string[] hoverClasses)
        {
            foreach (var hoverClass in hoverClasses)
            {
                ele.AddHoverClass(hoverClass);
            }
            return ele;
        }
        
        // set .text as extension method to Label, Button, etc.
        public static T Text<T> (this T ele, string text) where T : VisualElement
        {
            if (ele is TextElement textElement)
            {
                textElement.text = text;
            }
            
            return ele;
        }
        
        // Click as extension method to Button
        public static T Click<T> (this T ele, EventCallback<MouseUpEvent> callback) where T : VisualElement
        {
            if (ele is Button button)
            {
                button.RegisterCallback<MouseUpEvent>(callback);
            }
            
            return ele;
        }
        
        public static T Style<T> (this T ele, string classes) where T : VisualElement
        {
            foreach (var className in classes.Split(' '))
            {
                ele.AddToClassList(className);
            }
            
            return ele;
        }
        
        // Push as extension method to VisualElement
        public static T Push<T> (this T ele, params VisualElement[] children) where T : VisualElement
        {
            foreach (var child in children)
            {
                ele.Add(child);
            }
            
            return ele;
        }
        
        
        public static T RecursiveBuild<T> (this T ele, out T output) where T : VisualElement {
            foreach (var child in ele.Children())
            {
                child.RecursiveBuild(out _);
            }
            output = ele;
            return ele;
        }
        
        // Output as extension method to VisualElement
        public static T Build<T> (this T ele, out T output) where T : VisualElement {
            output = ele;
            return ele;
        }

        public static Button MenuButton() {
            return Create<Button>("rounded-lg bg-purple-500 text-white p-1 mb-4")
                .AddHoverClass("bg-purple-600");
        }
        
    }
}