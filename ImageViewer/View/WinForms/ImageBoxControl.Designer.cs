using System;

namespace ClearCanvas.ImageViewer.View.WinForms
{
    partial class ImageBoxControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

				if (_imageBox != null)
				{
					_imageBox.Drawing -= new EventHandler(OnDrawing);
					_imageBox.SelectionChanged -= new EventHandler<ImageBoxEventArgs>(OnImageBoxSelectionChanged);
					_imageBox.TileAdded -= new EventHandler<TileEventArgs>(OnTileAdded);
					_imageBox.TileRemoved -= new EventHandler<TileEventArgs>(OnTileRemoved);
				}
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }
}
