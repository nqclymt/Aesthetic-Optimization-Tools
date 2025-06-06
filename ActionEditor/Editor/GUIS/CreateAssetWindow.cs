
using System;
using System.Data.SqlTypes;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PKC.ActionEditor
{
    public class CreateAssetWindow : PopupWindowContent
    {
        
        private static Rect _myRect ;
        private string _selectType ;
        private string _createName = string.Empty;

        public static void Show()
        {
            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            _myRect = new Rect(mousePos.x, mousePos.y, 400, 150);
            PopupWindow.Show(new Rect(_myRect.x,_myRect.y,0,0),new CreateAssetWindow());
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
            GUILayout.Label($"<size=22><b>{Lan.CreateAsset}</b></size>");
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            
            GUILayout.BeginVertical("box");
            
            _selectType = EditorTools.CleanPopup(Lan.CreateAsset, _selectType, Prefs.AssetNames);
            _createName = EditorGUILayout.TextField(new GUIContent(Lan.CreateAsset, Lan.CreateAssetFileName),
                _createName);
            GUI.backgroundColor = new Color(1, 0.5f, 0.5f);
            if (GUILayout.Button(new GUIContent(Lan.CreateAssetConfirm)))
            {
                CreateConfirm();
            }
            
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button(new GUIContent(Lan.CreateAssetReset)))
            {
                _selectType = Prefs.AssetNames[0];
                _createName = string.Empty;
            }
            GUILayout.EndVertical();
            
            GUILayout.EndVertical();
            
        }

        void CreateConfirm()
        {
            var path = $"{Prefs.savePath}/{_createName}.json";
            if (string.IsNullOrEmpty(_createName))
            {
                EditorUtility.DisplayDialog(Lan.TipsTitle, Lan.CreateAssetTipsNameNull, Lan.TipsConfirm);
            }
            else if (AssetDatabase.LoadAssetAtPath<TextAsset>(path) != null)
            {
                EditorUtility.DisplayDialog(Lan.TipsTitle, Lan.CreateAssetTipsRepetitive, Lan.TipsConfirm);
            }
            else
            {
                var t = Prefs.AssetTypes[_selectType];
                var inst = Activator.CreateInstance(t);
                if (inst != null)
                {
                    var json = Json.Serialize(inst);
                    System.IO.File.WriteAllText(path,json);
                    AssetDatabase.Refresh();
                    var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    if (textAsset != null)
                    {
                        App.OnObjectPickerConfig(textAsset);
                    }
                    editorWindow.Close();
                }
            }
        }
    }
}