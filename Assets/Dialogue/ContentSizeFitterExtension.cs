using UnityEngine.EventSystems;
 
namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/Content Size Fitter With Max", 141)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class ContentSizeFitterExtension : UIBehaviour, ILayoutSelfController
    {
        /// <summary>
        /// The size fit modes avaliable to use.
        /// </summary>
        public enum FitMode
        {
            /// <summary>
            /// Don't perform any resizing.
            /// </summary>
            Unconstrained,
 
            /// <summary>
            /// Resize to the minimum size of the content.
            /// </summary>
            MinSize,
 
            /// <summary>
            /// Resize to the preferred size of the content.
            /// </summary>
            PreferredSize
        }
 
        [SerializeField]
        protected FitMode horizontalFit = FitMode.Unconstrained;
 
        /// <summary>
        /// The fit mode to use to determine the width.
        /// </summary>
        public FitMode HorizontalFit
        {
            get => horizontalFit;
            set
            {
                if (horizontalFit == value) return;
                horizontalFit = value;
                SetDirty();
            }
        }
 
        [SerializeField]
        protected FitMode verticalFit = FitMode.Unconstrained;
 
        /// <summary>
        /// The fit mode to use to determine the height.
        /// </summary>
        public FitMode VerticalFit
        {
            get => verticalFit;
            set
            {
                if (verticalFit == value) return;
                verticalFit = value;
                SetDirty();
            }
        }
 
        [Tooltip("Maximum Preferred size when using Preferred Size")]
        public Vector2 MaximumPreferredSize;
 
        [System.NonSerialized]
        private RectTransform rectTransform;
 
        private RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }
 
        private DrivenRectTransformTracker tracker;
 
        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }
 
        protected override void OnDisable()
        {
            tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
            base.OnDisable();
        }
 
        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }
 
        private void HandleSelfFittingAlongAxis(int axis)
        {
            var fitting = (axis == 0 ? HorizontalFit : VerticalFit);
            if (fitting == FitMode.Unconstrained)
            {
                // Keep a reference to the tracked transform, but don't control its properties:
                tracker.Add(this, RectTransform, DrivenTransformProperties.None);
                return;
            }
 
            tracker.Add(this, RectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));
 
            switch (fitting)
            {
                // Set size to min or preferred size
                case FitMode.MinSize:
                    RectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) axis, LayoutUtility.GetMinSize(rectTransform, axis));
                    break;
 
                case FitMode.PreferredSize:
                    RectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) axis, Mathf.Clamp(LayoutUtility.GetPreferredSize(rectTransform, axis), 0, axis == 0 ? MaximumPreferredSize.x : MaximumPreferredSize.y));
                    break;
            }
        }
 
        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }
 
        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }
 
        protected void SetDirty()
        {
            if (!IsActive())
                return;
 
            LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
        }
 
    #if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
 
    #endif
    }
}