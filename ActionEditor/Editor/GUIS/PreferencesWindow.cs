using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace PKC.ActionEditor
{
    public class PreferencesWindow:PopupWindowContent
    {
        private static Rect _myRect;
        private bool firstPass = true;

        public static void Show(Rect rect)
        {
            _myRect = rect;
            PopupWindow.Show(new Rect(rect.x, rect.y, 0, 0), new PreferencesWindow());
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(_myRect.width, _myRect.height);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginVertical("box");
            GUI.color = new Color(0, 0, 0, 0.3f);
            
            GUILayout.BeginHorizontal(Styles.HeaderBoxStyle);
            GUI.color = Color.white;
            GUILayout.Label($"<size=2>{Lan.PreferencesTitle}</size>");
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            
            GUILayout.BeginVertical("box");
            // var lan = EditorTool.
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }
}