using System;
using System.Collections.Generic;
using UnityEngine;

namespace BiggerDemo.Extensions
{
    public static class ExtensionMethods
    {
        public static float GetCameraOrthographicSize(Vector2Int boardSize, float percentOfOrthographicSize = 0.5f, float cellSize = 1f)
        {
            int targetDir;
            if (Screen.width > Screen.height)
            {
                targetDir = boardSize.y;
            }
            else
            {
                targetDir = boardSize.x;
            }

            float orthographicSize = (targetDir * (cellSize + cellSize * (1 - percentOfOrthographicSize)));
            return orthographicSize;
        }

        public static TextMesh CreateWorldTextTemplate(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 5000)
        {
            return ExtensionMethods.CreateWorldText(text, parent, localPosition, fontSize, color ?? Color.white, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }

        public static void Shuffle<T>(this T[,] array)
        {
            int m = array.GetLength(0);
            int n = array.GetLength(1);
            for (int i = 0; i < array.Length; i++)
            {
                int rm = UnityEngine.Random.Range(0, m--);
                int rn = UnityEngine.Random.Range(0, n--);
                T temp = array[m, n];
                array[m, n] = array[rm, rn];
                array[rm, rn] = temp;
                if (m == 0)
                {
                    m = array.GetLength(0);
                }

                if (n == 0)
                {
                    n = array.GetLength(1);
                }
            }
        }

        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return list[UnityEngine.Random.Range(0, list.Count - 1)];
        }

        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static string ToTimeString(this float seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds);
            return answer;
        }
    }
}
