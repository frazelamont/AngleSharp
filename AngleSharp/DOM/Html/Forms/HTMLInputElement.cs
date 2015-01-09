﻿namespace AngleSharp.DOM.Html
{
    using AngleSharp.DOM.Io;
    using AngleSharp.Extensions;
    using AngleSharp.Html;
    using AngleSharp.Html.InputTypes;
    using System;

    /// <summary>
    /// Represents an HTML input element.
    /// </summary>
    sealed class HTMLInputElement : HTMLTextFormControlElement, IHtmlInputElement
    {
        #region Fields

        BaseInputType _type;
        Boolean? _checked;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new HTML input element.
        /// </summary>
        public HTMLInputElement(Document owner)
            : base(Tags.Input, NodeFlags.SelfClosing)
        {
            Owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default value of the input field.
        /// </summary>
        public override String DefaultValue
        {
            get { return GetAttribute(AttributeNames.Value); }
            set { SetAttribute(AttributeNames.Value, value); }
        }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public Boolean IsDefaultChecked
        {
            get { return GetAttribute(AttributeNames.Checked) != null; }
            set { SetAttribute(AttributeNames.Checked, value ? String.Empty : null); }
        }

        /// <summary>
        /// Gets or sets if the input element is checked or not.
        /// </summary>
        public Boolean IsChecked
        {
            get { return _checked.HasValue ? _checked.Value : IsDefaultChecked; }
            set { _checked = value; }
        }

        /// <summary>
        /// Gets or sets the type of the input field.
        /// </summary>
        public String Type
        {
            get { return _type.Name; }
            set { SetAttribute(AttributeNames.Type, value); }
        }

        /// <summary>
        /// Gets or sets if the state if indeterminate.
        /// </summary>
        public Boolean IsIndeterminate 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the multiple HTML attribute, whichindicates whether multiple items can be selected.
        /// </summary>
        public Boolean IsMultiple
        {
            get { return GetAttribute(AttributeNames.Multiple) != null; }
            set { SetAttribute(AttributeNames.Multiple, value ? String.Empty : null); }
        }

        /// <summary>
        /// Gets or sets the value of the element, interpreted as a date, or null
        /// if conversion is not possible.
        /// </summary>
        public DateTime? ValueAsDate
        {
            get { return _type.ConvertToDate(Value); }
            set
            {
                if (value == null)
                    Value = String.Empty;
                else
                    Value = _type.ConvertFromDate(value.Value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the element, interpreted as one of the following in order:
        /// 1.) Time value 2.) Number 3.) otherwise NaN.
        /// </summary>
        public Double ValueAsNumber
        {
            get { return _type.ConvertToNumber(Value) ?? Double.NaN; }
            set
            {
                if (Double.IsInfinity(value))
                    throw new DomException(ErrorCode.TypeMismatch);
                else if (Double.IsNaN(value))
                    Value = String.Empty;
                else
                    Value = _type.ConvertFromNumber(value);
            }
        }

        /// <summary>
        /// Gets or sets the URI of a resource that processes information submitted by the button.
        /// If specified, this attribute overrides the action attribute of the form element that owns this element.
        /// </summary>
        public String FormAction
        {
            get { if (Form == null) return String.Empty; return Form.Action; }
            set { if (Form != null) Form.Action = value; }
        }

        /// <summary>
        /// Gets or sets the type of content that is used to submit the form to the server. If specified, this
        /// attribute overrides the enctype attribute of the form element that owns this element.
        /// </summary>
        public String FormEncType
        {
            get { if (Form == null) return String.Empty; return Form.Enctype; }
            set { if (Form != null) Form.Enctype = value; }
        }

        /// <summary>
        /// Gets or sets the HTTP method that the browser uses to submit the form. If specified, this attribute
        /// overrides the method attribute of the form element that owns this element.
        /// </summary>
        public String FormMethod
        {
            get { if (Form == null) return String.Empty; return Form.Method; }
            set { if (Form != null) Form.Method = value; }
        }

        /// <summary>
        /// Gets or sets that the form is not to be validated when it is submitted. If specified, this attribute
        /// overrides the enctype attribute of the form element that owns this element.
        /// </summary>
        public Boolean FormNoValidate
        {
            get { if (Form == null) return false; return Form.NoValidate; }
            set { if (Form != null) Form.NoValidate = value; }
        }

        /// <summary>
        /// Gets or sets A name or keyword indicating where to display the response that is received after submitting
        /// the form. If specified, this attribute overrides the target attribute of the form element that owns this element.
        /// </summary>
        public String FormTarget
        {
            get { if (Form == null) return String.Empty; return Form.Target; }
            set { if (Form != null) Form.Target = value; }
        }

        /// <summary>
        /// Gets or sets the accept HTML attribute, containing comma-separated list of
        /// file types accepted by the server when type is file.
        /// </summary>
        public String Accept
        {
            get { return GetAttribute(AttributeNames.Accept); }
            set { SetAttribute(AttributeNames.Accept, value); }
        }

        /// <summary>
        /// Gets or sets the alignment of the element.
        /// </summary>
        public Alignment Align
        {
            get { return GetAttribute(AttributeNames.Align).ToEnum(Alignment.Left); }
            set { SetAttribute(AttributeNames.Align, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the alt HTML attribute, containing alternative
        /// text to use when type is image.
        /// </summary>
        public String AlternativeText
        {
            get { return GetAttribute(AttributeNames.Alt); }
            set { SetAttribute(AttributeNames.Alt, value); }
        }

        /// <summary>
        /// Gets or sets the autocomplete HTML attribute, indicating whether
        /// the value of the control can be automatically completed by the
        /// browser. Ignored if the value of the type attribute is hidden,
        /// checkbox, radio, file, or a button type (button, submit, reset,
        /// image).
        /// </summary>
        public String Autocomplete
        {
            get { return GetAttribute(AttributeNames.AutoComplete); }
            set { SetAttribute(AttributeNames.AutoComplete, value); }
        }

        /// <summary>
        /// Gets a list of selected files.
        /// </summary>
        public IFileList Files
        {
            get
            {
                var type = _type as FileInputType;

                if (type != null)
                    return type.Files;

                return null;
            }
        }

        /// <summary>
        /// Gets the datalist element in the same document.
        /// Only options that are valid values for this input element will
        /// be displayed. This attribute is ignored when the type
        /// attribute's value is hidden, checkbox, radio, file, or a button type.
        /// </summary>
        public IHtmlDataListElement List
        {
            get 
            {
                var owner = Owner;

                if (owner != null)
                    return owner.GetElementById(GetAttribute(AttributeNames.List)) as IHtmlDataListElement; 

                return null;
            }
        }

        /// <summary>
        /// Gets or sets max HTML attribute, containing the maximum (numeric
        /// or date-time) value for this item, which must not be less than its
        /// minimum (min attribute) value.
        /// </summary>
        public String Maximum
        {
            get { return GetAttribute(AttributeNames.Max); }
            set { SetAttribute(AttributeNames.Max, value); }
        }

        /// <summary>
        /// Gets or sets the min HTML attribute, containing the minimum (numeric
        /// or date-time) value for this item, which must not be greater than its
        /// maximum (max attribute) value.
        /// </summary>
        public String Minimum
        {
            get { return GetAttribute(AttributeNames.Min); }
            set { SetAttribute(AttributeNames.Min, value); }
        }

        /// <summary>
        /// Gets or sets the pattern HTML attribute, containing a regular expression
        /// that the control's value is checked against. The pattern must match the
        /// entire value, not just some subset. This attribute applies when the value
        /// of the type attribute is text, search, tel, url or email; otherwise it is ignored.
        /// </summary>
        public String Pattern
        {
            get { return GetAttribute(AttributeNames.Pattern); }
            set { SetAttribute(AttributeNames.Pattern, value); }
        }

        /// <summary>
        /// Gets or sets the size HTML attribute, containing size of the control. This value
        /// is in pixels unless the value of type is text or password, in which case, it is
        /// an integer number of characters. Applies only when type is set to text, search, tel,
        /// url, email, or password; otherwise it is ignored.
        /// </summary>
        public Int32 Size
        {
            get { return GetAttribute(AttributeNames.Size).ToInteger(20); }
            set { SetAttribute(AttributeNames.Size, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the src HTML attribute, which specifies a URI for the location of an
        /// image to display on the graphical submit button, if the value of type is image;
        /// otherwise it is ignored.
        /// </summary>
        public String Source
        {
            get { return GetAttribute(AttributeNames.Src); }
            set { SetAttribute(AttributeNames.Src, value); }
        }

        /// <summary>
        /// Gets or sets the step HTML attribute, which works with min and max to limit the
        /// increments at which a numeric or date-time value can be set. It can be the string
        /// any or a positive floating point number. If this is not set to any, the control
        /// accepts only values at multiples of the step value greater than the minimum.
        /// </summary>
        public String Step
        {
            get { return GetAttribute(AttributeNames.Step); }
            set { SetAttribute(AttributeNames.Step, value); }
        }

        /// <summary>
        /// Gets or sets a client-side image map.
        /// </summary>
        public String UseMap
        {
            get { return GetAttribute(AttributeNames.UseMap); }
            set { SetAttribute(AttributeNames.UseMap, value); }
        }

        /// <summary>
        /// Gets or sets the width HTML attribute, which defines the width of the image
        /// displayed for the button, if the value of type is image.
        /// </summary>
        public Int32 DisplayWidth
        {
            get { return GetAttribute(AttributeNames.Width).ToInteger(OriginalWidth); }
            set { SetAttribute(AttributeNames.Width, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the height HTML attribute, which defines the
        /// height of the image displayed for the button, if the value
        /// of type is image.
        /// </summary>
        public Int32 DisplayHeight
        {
            get { return GetAttribute(AttributeNames.Height).ToInteger(OriginalHeight); }
            set { SetAttribute(AttributeNames.Height, value.ToString()); }
        }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public Int32 OriginalWidth
        {
            get
            {
                var type = _type as ImageInputType;

                if (type != null)
                    return type.Width;

                return 0;
            }
        }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public Int32 OriginalHeight
        {
            get
            {
                var type = _type as ImageInputType;

                if (type != null)
                    return type.Height;

                return 0;
            }
        }

        #endregion

        #region Design properties

        /// <summary>
        /// Gets or sets if the link has been visited.
        /// </summary>
        internal Boolean IsVisited
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if the link is currently active.
        /// </summary>
        internal Boolean IsActive
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public sealed override INode Clone(Boolean deep = true)
        {
            var node = (HTMLInputElement)base.Clone(deep);
            node._checked = _checked;
            return node;
        }

        internal override FormControlState SaveControlState()
        {
            return new FormControlState(Name, Type, Value);
        }

        internal override void RestoreFormControlState(FormControlState state)
        {
            if (state.Type == Type && state.Name == Name)
                Value = state.Value;
        }

        /// <summary>
        /// Increments the value by (step * n), where n defaults to 1 if not specified.
        /// </summary>
        /// <param name="n">Optional: The number of steps to take.</param>
        public void StepUp(Int32 n = 1)
        {
            _type.DoStep(n);
        }

        /// <summary>
        /// Decrements the value by (step * n), where n defaults to 1 if not specified. 
        /// </summary>
        /// <param name="n">Optional: The number of steps to take.</param>
        public void StepDown(Int32 n = 1)
        {
            _type.DoStep(-n);
        }

        #endregion

        #region Internal Properties

        internal Boolean IsMutable 
        {
            get { return !IsDisabled && !IsReadOnly; }
        }

        #endregion

        #region Internal Methods

        internal override void Close()
        {
            base.Close();
            RegisterAttributeHandler(AttributeNames.Type, UpdateType);
            UpdateType(GetAttribute(AttributeNames.Type));
        }

        void UpdateType(String type)
        {
            _type = InputTypeFactory.Create(this, type);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Constructs the data set (called from a form).
        /// </summary>
        /// <param name="dataSet">The dataset to construct.</param>
        /// <param name="submitter">The given submitter.</param>
        internal override void ConstructDataSet(FormDataSet dataSet, HTMLElement submitter)
        {
            _type.ConstructDataSet(dataSet);
        }

        /// <summary>
        /// Resets the form control to its initial value.
        /// </summary>
        internal override void Reset()
        {
            base.Reset();
            _checked = null;
            UpdateType(Type);
        }

        /// <summary>
        /// Checks the form control for validity.
        /// </summary>
        /// <param name="state">The element's validity state tracker.</param>
        protected override void Check(ValidityState state)
        {
            base.Check(state);
            _type.Check(state);
        }

        protected override Boolean CanBeValidated()
        {
            return _type.CanBeValidated && base.CanBeValidated();
        }

        #endregion
    }
}
