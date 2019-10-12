using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DesktopControlLibrary.GridControl
{
    interface BaseViewControlInterface
    {
        void NewItem();
        void OpenEditItem();
        void RemoveItem();
        void RemoveSelectedItem();
        void RefreshDataList();
        void SetEditInRow(bool allowEdit);
        void SetEditButtonMode(GridEditingMode mode);
        void ExportExcel();
        void ExportPDF();
        void ExportHtml();
        void ExportXml();
        void SetMultiSelectMode(GridMultiSelectMode mode);
    }
}
