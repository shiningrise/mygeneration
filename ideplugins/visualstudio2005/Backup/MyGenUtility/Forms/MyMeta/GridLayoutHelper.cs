using System;
using System.Windows.Forms;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for GridLayoutHelper.
	/// </summary>
	public class GridLayoutHelper
	{
		public GridLayoutHelper(DataGrid grid, 
								DataGridTableStyle tableStyle,
								decimal[] percentages,
								int[] minimumWidths)
		{
			this.grid = grid;
			this.tableStyle = tableStyle;
			this.percentages = percentages;
			this.minimumWidths = minimumWidths;

			foreach(DataGridColumnStyle col in tableStyle.GridColumnStyles)
			{
				col.WidthChanged += new EventHandler(this.Column_WidthChanged);
			}

			grid.Resize += new System.EventHandler(this.Grid_SizeChanged);

			this.SizeTheGrid();
		}

		public void SizeTheGrid()
		{
			try
			{
				if(grid.DataSource == null) return;

				if (tableStyle == null)	return;
				GridColumnStylesCollection colStyles = tableStyle.GridColumnStyles;
				if (colStyles.Count == 0) return;

				// Okay, we've got the data to do the sizing
				inManualSize = true;

				//get the target width
				int width = 0;
				width = GetGridWidth();

				if(IsScrollBarVisible(grid))
					width -= SystemInformation.VerticalScrollBarWidth;

				int nCols = tableStyle.GridColumnStyles.Count;
				int lastColIndex  = nCols - 1;

				// Set to zero so horizontal scroll does not show as your size
				// Frees up room in case leading cols grid during resize and try to flash teh HScroll
				colStyles[lastColIndex].Width = 0;

				decimal dWidth = (decimal)width;
				int totalWidth = width;
				int colWidth;

				// By Default we make colWidth equally distributed
				colWidth = width / nCols;
				for(int i = 0; i < lastColIndex; ++i)
				{
					if(null != percentages)
					{
						// We're using Percentages
						colWidth = (int)(dWidth * percentages[i]);
					}

					if(null != minimumWidths)
						totalWidth -= colStyles[i].Width = Math.Max(colWidth, minimumWidths[i]);
					else
						totalWidth -= colStyles[i].Width = colWidth;
				}

				// Add on any left over due to rounding
				if(null != minimumWidths)
					colStyles[lastColIndex].Width = Math.Max(totalWidth, minimumWidths[lastColIndex]);
				else
					colStyles[lastColIndex].Width = totalWidth;

				inManualSize = false;
			}
			catch
			{
				throw;
			}
		}

		public void FixupLastColumn()
		{
			if(grid.DataSource == null) return;

			if (tableStyle == null)	return;
			GridColumnStylesCollection colStyles = tableStyle.GridColumnStyles;
			if (colStyles.Count == 0) return;

			// Okay, we've got the data to do the sizing
			inManualSize = true;

			//get the target width
			int width = GetGridWidth();

			if(IsScrollBarVisible(grid))
				width -= SystemInformation.VerticalScrollBarWidth;

			int nCols = tableStyle.GridColumnStyles.Count;
			int lastColIndex  = nCols - 1;

			int totalWidth = width;

			for(int i = 0; i < lastColIndex; ++i)
			{
				totalWidth -= colStyles[i].Width;
			}

			int minSizeLastCol = -1;
			if(null != minimumWidths)
			{
				minSizeLastCol = minimumWidths[lastColIndex];
			}

			// Last column always gets remainder of space
			if(totalWidth > minSizeLastCol)
			{
				colStyles[lastColIndex].Width = totalWidth;
			}

			inManualSize = false;
		}

		private int GetGridWidth() 
		{
			int borderWidth = 0;

			switch (grid.BorderStyle) 
			{
				case System.Windows.Forms.BorderStyle.Fixed3D : 
				{
					borderWidth = SystemInformation.Border3DSize.Width * 2;

				}
					break;
				case System.Windows.Forms.BorderStyle.FixedSingle : 
				{
					borderWidth = SystemInformation.BorderSize.Width * 2;
				}
					break;
			}

			return grid.ClientSize.Width - (grid.RowHeaderWidth + borderWidth);

		}
			
		#region IsScrollBarVisible

		protected bool IsScrollBarVisible(Control aControl)
		{
			foreach(Control c in aControl.Controls)
			{
				if (c.GetType().Equals(typeof(VScrollBar)))
				{
					return c.Visible;
				}
			}
			return false;
		}

		#endregion

		#region Properties
		public decimal[] Percentages
		{
			get
			{
				return percentages;
			}
		}

		public int[] MinimumColumnWidths
		{
			get
			{
				return minimumWidths;
			}
		}
		#endregion

		#region Event Handlers
		public void Grid_SizeChanged(object sender, System.EventArgs e)
		{
			this.Handle_SizeChanged(grid, tableStyle);
		}

		public void Column_WidthChanged(object sender, EventArgs e)
		{
			this.Handle_WidthChanged(grid, tableStyle);
		}

		public void Handle_SizeChanged(DataGrid grid, DataGridTableStyle ts)
		{
			if(!userOverride)
			{
				this.SizeTheGrid();
			}
			else
			{
				this.FixupLastColumn();
			}
		}

		public void Handle_WidthChanged(DataGrid grid, DataGridTableStyle ts)
		{
			if(!inManualSize) 
			{
				userOverride = true;
				this.FixupLastColumn();
			}
		}
		#endregion

		private DataGrid grid;
		private DataGridTableStyle tableStyle;

		private bool userOverride = false;
		private bool inManualSize = false;

		private decimal[] percentages   = null;
		private int[]     minimumWidths = null;
	}
}
