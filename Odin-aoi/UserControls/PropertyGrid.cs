using System.Collections.Generic;
using System.Windows.Forms;

namespace power_aoi.UserControls
{
    public class PropertyGrid : System.Windows.Forms.PropertyGrid
    {
        #region Public Members

        public GridItem FindItem(string itemLabel)
        {
            GridItem rootItem;
            GridItem matchingItem;
            Queue<GridItem> searchItems;

            matchingItem = null;

            // Find the GridItem root.
            rootItem = this.SelectedGridItem;
            while (rootItem.Parent != null)
            {
                rootItem = rootItem.Parent;
            }

            // Search the tree.
            searchItems = new Queue<GridItem>();

            searchItems.Enqueue(rootItem);

            while (searchItems.Count != 0 && matchingItem == null)
            {
                GridItem checkItem;

                checkItem = searchItems.Dequeue();

                if (checkItem.Label == itemLabel)
                {
                    matchingItem = checkItem;
                }

                foreach (GridItem item in checkItem.GridItems)
                {
                    searchItems.Enqueue(item);
                }
            }

            return matchingItem;
        }

        public void SelectItem(string itemLabel)
        {
            GridItem selection;

            selection = this.FindItem(itemLabel);
            if (selection != null)
            {
                try
                {
                    this.SelectedGridItem = selection;
                }
                catch
                {
                    // ignore
                }
            }
        }
        #endregion
    }
}

