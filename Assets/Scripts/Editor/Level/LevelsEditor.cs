using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System;
using BiggerDemo.Data;
using BiggerDemo.Creation;

namespace BiggerDemo.BEditor
{
    [CustomEditor(typeof(Levels))]
    public class LevelsEditor : Editor
    {
        private SerializedProperty _property;
        private ReorderableList _list;
        private void OnEnable()
        {
            _property = serializedObject.FindProperty("LevelDatas");
            _list = new ReorderableList(serializedObject, _property, true, true, true, true)
            {
                drawHeaderCallback = DrawListHeader,
                drawElementCallback = DrawListElement
            };
        }

        private void DrawListHeader(Rect rect)
        {
            GUI.Label(rect, "");
        }

        private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var item = _property.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, item);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            _list.DoLayoutList();

            if (GUILayout.Button("Generate Levels"))
            {
                Levels levels = (Levels)serializedObject.targetObject;
                levels.LevelDatas = new List<LevelData>()
            {
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Easy),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Easy),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Easy),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Medium),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Medium),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Medium),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Hard),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Hard),
                LevelFactory.Instance.CreateRandomLevelCreator().GenerateRandomLevel(LevelDifficult.Hard),
            };

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}