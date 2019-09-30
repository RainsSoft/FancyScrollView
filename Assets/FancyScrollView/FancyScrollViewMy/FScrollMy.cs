namespace FancyScrollView {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using EasingCore;
    using Text = TMPro.TMP_Text;
    using InputField = TMPro.TMP_InputField;
    using Dropdown = TMPro.TMP_Dropdown;
    using ItemData = FScrollMy_ItemData;
    using Context = FScrollMy_Context;



    public class FScrollMy : FancyScrollView<ItemData, Context> {

        //
        [SerializeField] internal RectTransform Viewport;
        [SerializeField] internal Scroller scroller;
        [SerializeField] GameObject cellPrefab;
        [SerializeField] bool AutoCalculCellInterval = false;
        [SerializeField] float AutoCellIntervalSpacing = 4;
        [SerializeField] Button prevCellButton;
        [SerializeField] Button nextCellButton;
        Action<int> onSelectionChanged;
        //[Header("--Debug-------------")]
        //public bool Debug_UseTestData=false;
        //public int Debug_TestDataCount=20;
        //public void Debug_AddTestData() {
        //    var items = Enumerable.Range(0, 20)
        //    .Select(i => new ItemData() { TagStr="Cell " + i})
        //    .ToArray();

        //    this.UpdateData(items);
        //    this.SelectCell(0);
        //}
        protected override GameObject CellPrefab {
            get {
                return cellPrefab;
            }
        }

        void Awake() {
            #region
            GameObject go;
            if (this.cellContainer != null) {
                //保证和UGUI的ScrollView结构一致，不直接销毁，内部存放 cellView对象
                //DestroyImmediate(_scrollRect.content.gameObject);
                var content = this.cellContainer;
                content.gameObject.SetActive(false);
            }
            // Create a new active cell view container with a layout group
            go = new GameObject("Container", typeof(RectTransform));
            var containerRect = go.GetComponent<RectTransform>();
            containerRect.SetParent(this.Viewport, false);
            containerRect.pivot = new Vector2(0, 1);
            containerRect.anchorMax = new Vector2(1f, 1f);
            containerRect.anchorMin = new Vector2(0f, 0f);
            containerRect.anchoredPosition = new Vector2(0f, 0f);
            containerRect.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            containerRect.offsetMax = new Vector2(0f, 0f);
            containerRect.offsetMin = new Vector2(0f, 0f);
            //
            if (this.cellContainer != null) {
                go.transform.SetSiblingIndex(this.cellContainer.GetSiblingIndex());
            }
            this.cellContainer = go.transform;
            #endregion
            //
            Context.OnCellSelect = SelectCell;
            Context.scrollView = this;
            //把滚动器，容器绑在同一个panel上
            if (scroller == null) {
                scroller = this.GetComponent<Scroller>();
            }
            scroller.viewport = containerRect;
            //
            if (this.scroller != null&&this.cellPrefab!=null) {
                if (this.scroller.directionOfRecognize == Scroller.ScrollDirection.Horizontal) {
                    //水平
                    var rtf=this.cellPrefab.GetComponent<RectTransform>();
                    rtf.anchorMin = new Vector2(0f, 0.5f);
                    rtf.anchorMax = new Vector2(0f,0.5f);
                    rtf.pivot = new Vector2(0.5f,0.5f);
                    rtf.localPosition = Vector3.zero;
                    rtf.anchoredPosition3D = Vector3.zero;
                }
                else if (this.scroller.directionOfRecognize == Scroller.ScrollDirection.Vertical) {
                    //垂直
                    var rtf = this.cellPrefab.GetComponent<RectTransform>();
                    rtf.anchorMin = new Vector2(0.5f, 0f);
                    rtf.anchorMax = new Vector2(0.5f, 0f);
                    rtf.pivot = new Vector2(0.5f, 0.5f);
                    rtf.localPosition = Vector3.zero;
                    rtf.anchoredPosition3D = Vector3.zero;
                }
            }
            //
            if (this.AutoCalculCellInterval && this.cellPrefab != null && this.scroller != null) {
                float ci = this.cellInterval;
                if (scroller.directionOfRecognize == Scroller.ScrollDirection.Horizontal) {
                    ci = (this.cellPrefab.GetComponent<RectTransform>().rect.width + this.AutoCellIntervalSpacing) / this.cellContainer.GetComponent<RectTransform>().rect.width;
                }
                else if (scroller.directionOfRecognize == Scroller.ScrollDirection.Vertical) {
                    ci = (this.cellPrefab.GetComponent<RectTransform>().rect.height + this.AutoCellIntervalSpacing) / this.cellContainer.GetComponent<RectTransform>().rect.height;
                }
                this.cellInterval = Mathf.Max(ci, cellInterval);
                this.scrollOffset = ci*0.5f;
            }
            if (prevCellButton != null) prevCellButton.onClick.AddListener(SelectPrevCell);
            if (nextCellButton != null) nextCellButton.onClick.AddListener(SelectNextCell);
            //提前
            doStart();
        }

        void doStart() {
            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
            //

            //#if DEBUG&&UNITY_EDITOR
            //            this.Debug_AddTestData();
            //#endif
        }

        void UpdateSelection(int index) {
            Debug.Log("OnSelectionChanged index:" + index);
            if (Context.SelectedIndex == index) {
                return;
            }

            Context.SelectedIndex = index;
            Refresh();

            if (onSelectionChanged != null) onSelectionChanged.Invoke(index);
        }

        public void UpdateData(IList<ItemData> items) {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        public void OnSelectionChanged(Action<int> callback) {
            onSelectionChanged = callback;
        }

        public void SelectNextCell() {
            SelectCell(Context.SelectedIndex + 1);
        }

        public void SelectPrevCell() {
            SelectCell(Context.SelectedIndex - 1);
        }

        public void SelectCell(int index) {
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex) {
                return;
            }

            UpdateSelection(index);
            scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
        }
    }

    public class FScrollMy_Context {
        public Action<int, FScrollMy_ItemData, GameObject> OnUpdateCellContent;
        public Action<int, FScrollMy_ItemData, GameObject> OnCellClicked;
        public Action<int> OnCellSelect;
        public FScrollMy scrollView;
        public int SelectedIndex = -1;
    }
}
