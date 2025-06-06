
using System;
using PKC.ActionEditor;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PKC.ActionEditor
{
    public class ActionEditorWindow : EditorWindow
    {
        [MenuItem("PCK/Action EdtiorWindow/Open Action Window", false, 0)]
        public static void OpenWindow()
        {
            ActionEditorWindow window = GetWindow<ActionEditorWindow>();
            if (window != null) window.Show();
        }

        private WelcomeView _welcomeView;
        private TimelineView _timelineView;


        #region Init

        void InittializeAll()
        { 
            Lan.Load();
            Styles.Load();
            Prefs.InitializeAssetTypes();
            App.OnInitialize?.Invoke();
            _welcomeView = this.CreateView<WelcomeView>();
            _timelineView = this.CreateView<TimelineView>();

        }

        #endregion

        #region Lifecyle

        private void OnEnable()
        { App.Window = this;
            EditorSceneManager.sceneSaving -= OnWillSaveScene;
            EditorSceneManager.sceneSaving += OnWillSaveScene;

            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
            
            titleContent = new GUIContent(Lan.Title);
            minSize = new Vector2(500, 250);
            
            InittializeAll();
        }

        void OnDisable()
        {
            App.Window = null;
            EditorSceneManager.sceneSaving -= OnWillSaveScene;
            EditorApplication.update -= OnEditorUpdate;
            
            App.OnDisable?.Invoke();
        }

        void OnEditorUpdate()
        {
            this.UpdateViews();
            if (App.NeedForceRefresh)
            {
                this.Repaint();
            }
            App.OnUpdate();
        }
        
        void OnGUI()
        {
            if (App.AssetData == null)
            {
                // Test();
                _welcomeView.OnGUI(this.position);
                return;
            }

            if (Event.current.type == EventType.MouseMove)
            {
                Debug.Log("MouseMove===11");
                Repaint();
            }
            
            _timelineView.OnGUI(this.position);
            App.OnGUIEnd();
        }

        void OnWillSaveScene(UnityEngine.SceneManagement.Scene scene, string path)
        {
        }
        
        #endregion
        
        #region Test

        private bool isDragging = false; // 是否正在拖动
        private Vector2 dragStartPos; // 拖动起始位置
        private Rect draggableRect = new Rect(50, 50, 100, 100); // 可拖动区域

        void Test()
        {
            // 绘制一个可拖动的矩形
            EditorGUI.DrawRect(draggableRect, Color.green);

            Event e = Event.current;

            // 检测鼠标事件
            switch (e.type)
            {
                case EventType.MouseDown:
                    // 如果鼠标在可拖动矩形内，开始拖动
                    if (draggableRect.Contains(e.mousePosition) && e.button == 0)
                    {
                        isDragging = true;
                        dragStartPos = e.mousePosition;
                        Debug.Log("Begin Drag");
                        e.Use(); // 使用事件，防止事件继续传播
                    }

                    break;

                case EventType.MouseDrag:
                    // 如果正在拖动，更新矩形位置
                    if (isDragging)
                    {
                        Vector2 dragDelta = e.mousePosition - dragStartPos;
                        draggableRect.position += dragDelta; // 更新矩形位置
                        dragStartPos = e.mousePosition; // 更新起始位置
                        Debug.Log("Dragging: " + dragDelta);
                        e.Use();
                    }

                    break;

                case EventType.MouseUp:
                    // 鼠标抬起，结束拖动
                    if (isDragging && e.button == 0)
                    {
                        isDragging = false;
                        Debug.Log("End Drag");
                        e.Use();
                    }

                    break;

                case EventType.Repaint:
                    // 额外操作：实时重绘界面
                    EditorGUI.DrawRect(draggableRect, Color.green);
                    break;
            }
        }

        #endregion
    }
}
