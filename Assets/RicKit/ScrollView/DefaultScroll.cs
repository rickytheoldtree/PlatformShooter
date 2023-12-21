using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.ScrollView
{
    public abstract class DefaultScroll<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private T itemPrefab;
        private float itemHeight;
        protected readonly List<T> items = new List<T>();
        private ScrollRect scrollRect;
        private RectTransform content;
        protected int totalCount;

        protected virtual void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            content = scrollRect.content;
            itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        }
        public void InitItems(int totalCount)
        {
            this.totalCount = totalCount;
            for (var i = 0; i < totalCount; i++)
            {
                var item = Instantiate(itemPrefab, content);
                items.Add(item);
            }
        }
        public void SetContentPos(int row, float spacing)
        {
            var y = row * itemHeight + row * spacing;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, y);
        }
        public void Clear()
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
        }
    }
}

