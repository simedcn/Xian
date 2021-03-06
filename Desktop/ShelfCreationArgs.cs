#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

namespace ClearCanvas.Desktop
{
    /// <summary>
    /// Holds parameters that control the creation of a <see cref="Shelf"/>.
    /// </summary>
    public class ShelfCreationArgs : DesktopObjectCreationArgs
    {
        private IApplicationComponent _component;
        private ShelfDisplayHint _displayHint;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShelfCreationArgs()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
		/// <param name="component">The <see cref="IApplicationComponent"/> that is to be hosted in the <see cref="Shelf"/>.</param>
		/// <param name="title">The title of the <see cref="Shelf"/>.</param>
		/// <param name="name">A name/identifier for the <see cref="Shelf"/>.</param>
		/// <param name="displayHint">A hint for how the <see cref="Shelf"/> should be initially displayed.</param>
		public ShelfCreationArgs(IApplicationComponent component, string title, string name, ShelfDisplayHint displayHint)
            : base(title, name)
        {
            _component = component;
            _displayHint = displayHint;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
		/// <param name="component">The <see cref="IApplicationComponent"/> that is to be hosted in the <see cref="Shelf"/>.</param>
		/// <param name="title">The title of the <see cref="Shelf"/>.</param>
		/// <param name="name">A name/identifier for the <see cref="Shelf"/>.</param>
		public ShelfCreationArgs(IApplicationComponent component, string title, string name)
            : this(component, name, title, ShelfDisplayHint.None)
        {
        }

        /// <summary>
        /// Gets or sets the component to host.
        /// </summary>
        public IApplicationComponent Component
        {
            get { return _component; }
            set { _component = value; }
        }

        /// <summary>
        /// Gets or sets the display hint that affects the initial positioning of the shelf.
        /// </summary>
        public ShelfDisplayHint DisplayHint
        {
            get { return _displayHint; }
            set { _displayHint = value; }
        }
    }
}
