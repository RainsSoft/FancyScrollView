namespace FancyScrollView {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Text = TMPro.TMP_Text;
    using InputField = TMPro.TMP_InputField;
    using Dropdown = TMPro.TMP_Dropdown;
    using ItemData = FScrollMy_ItemData;
    using Context = FScrollMy_Context;


    public class FScrollMy_Item : FancyScrollViewCell<ItemData, Context> {

        //[SerializeField] Text message;
        //[SerializeField] Image image;
        //[SerializeField] Button button;
        //public Action<int,FScrollMy_Item>


        #region mb初始
        private void Awake() {
            //Debug.Log("awake");
        }
        void OnEnable() {
            //Debug.Log("Awake-> *OnEnable->Property->Start");
            if (this.Context != null) {
                UpdatePosition(currentPosition);
            }
        }
        void Start() {
            UpdatePosition(currentPosition);
            //button.onClick.AddListener(() => {
            //    if (Context.OnCellClicked != null) Context.OnCellClicked.Invoke(Index);
            //});
        }
        #endregion
        [Serializable]
        public class OnUpdateCellContentEvent : UnityEngine.Events.UnityEvent<int, FScrollMy_ItemData, GameObject> { }

        public OnUpdateCellContentEvent OnUpdateCellItem = new OnUpdateCellContentEvent();
        public OnUpdateCellContentEvent OnCellClicked = new OnUpdateCellContentEvent();

        public bool Selected {
            get {
                return this.Context.SelectedIndex == this.Index;
            }
        }
        /// <summary>
        /// 触发内部关联Context点击事件
        /// </summary>
        public void OnNotifyClickedCell() {
            //this.Context.SelectedIndex = this.Index;
            this.OnCellClicked.Invoke(this.Index, this.ItemData, this.gameObject);

        }
        public override void UpdateContent(ItemData itemData) {

            this.ItemData = itemData;
            this.OnUpdateCellItem.Invoke(this.Index, itemData, this.gameObject);
            //message.text = itemData.Message;

            //var selected = Context.SelectedIndex == Index;
            //image.color = selected
            //    ? new Color32(0, 255, 255, 100)
            //    : new Color32(255, 255, 255, 77);
        }

        public override void UpdatePosition(float position) {
            //Todo:
            //Debug.Log(this.Index.ToString() + " pos: " + position); 
            if (this.Context == null || this.Context.scrollView == null) return;
            //
            currentPosition = position;
            RectTransform rectT = this.GetComponent<RectTransform>();
            RectTransform cellContainerRectT = this.Context.scrollView.cellContainer.GetComponent<RectTransform>();
            //
            float weight = cellContainerRectT.rect.width;
            float height = cellContainerRectT.rect.height;
            Scroller scroll = this.Context.scrollView.scroller;
            Vector3 anchorPos = rectT.anchoredPosition3D;
            //float offset = this.Context.scrollView.scrollOffset;
            if (scroll.directionOfRecognize == Scroller.ScrollDirection.Horizontal) {
                anchorPos.x = weight * (position);//preItemAnchorPos.x + preItemWeight + offset;
            }
            else {
                anchorPos.y = height * (1.0f - position);//preItemAnchorPos.y  + preItemHeight + offset;
            }
            anchorPos.z = 0;
            rectT.anchoredPosition3D = anchorPos;

        }

        // GameObject 保持当前位置以OnEnable的定时重新设定当前位置 
        float currentPosition = 0;
        public override void SetupContext(Context context) {
            Debug.Log("SetupContext");
            base.SetupContext(context);
        }


    }
    public class FScrollMy_ItemData {
        //public string Message { get; set; }

        //public FScrollMy_ItemData(string message) {
        //    Message = message;
        //}
        /// <summary>
        /// 关联对象
        /// </summary>
        public object TagObj;
        /// <summary>
        /// 标识字符
        /// </summary>
        public string TagStr;

    }
}
