using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Navigation;
using System.IO;

namespace Jund.DesktopControlLibrary.OtherControl.PDFControl
{
    public partial class PDFControl : DevExpress.XtraEditors.XtraUserControl
    {
        List<string> pdf_list = new List<string>();

        public string PDFViewerPageCaptain { get => pdfRibbonPage1.Text; set => pdfRibbonPage1.Text = value; }
        public string PDFCommentPageCapatin { get => pdfCommentRibbonPage1.Text; set => pdfCommentRibbonPage1.Text = value; }
        public string PDFFormDataPageCapatin { get => pdfFormDataRibbonPage1.Text; set => pdfFormDataRibbonPage1.Text = value; }
        public string PDFExportButtonCaptain { get => pdfExportFormDataBarItem1.Caption; set => pdfExportFormDataBarItem1.Caption = value; }
        public string PDFImportButtonCaptain { get => pdfImportFormDataBarItem1.Caption; set => pdfImportFormDataBarItem1.Caption = value; }
        public string PDFTextUnderlineButtonCaptain { get => pdfTextUnderlineBarItem1.Caption; set => pdfTextUnderlineBarItem1.Caption = value; }
        public string PDFTextStrikethroughButtonCaptain { get => pdfTextStrikethroughBarItem1.Caption; set => pdfTextStrikethroughBarItem1.Caption = value; }
        public string PDFTextHighlightButtonCaptain { get => pdfTextHighlightBarItem1.Caption; set => pdfTextHighlightBarItem1.Caption = value; }
        public string PDFSetFitVisibleZoomModeButtonCaptain { get => pdfSetFitVisibleZoomModeCheckItem1.Caption; set => pdfSetFitVisibleZoomModeCheckItem1.Caption = value; }
        public string PDFSetFitWidthZoomModeButtonCaptain { get => pdfSetFitWidthZoomModeCheckItem1.Caption; set => pdfSetFitWidthZoomModeCheckItem1.Caption = value; }
        public string PDFSetPageLevelZoomModeButtonCaptain { get => pdfSetPageLevelZoomModeCheckItem1.Caption; set => pdfSetPageLevelZoomModeCheckItem1.Caption = value; }
        public string PDFSetActualSizeZoomModeButtonCaptain { get => pdfSetActualSizeZoomModeCheckItem1.Caption; set => pdfSetActualSizeZoomModeCheckItem1.Caption = value; }
        public string PDFZoomListButtonCaptain { get => pdfExactZoomListBarSubItem1.Caption; set => pdfExactZoomListBarSubItem1.Caption = value; }
        public string PDFZoomInButtonCaptain { get => pdfZoomInBarItem1.Caption; set => pdfZoomInBarItem1.Caption = value; }
        public string PDFZoomOutButtonCaptain { get => pdfZoomOutBarItem1.Caption; set => pdfZoomOutBarItem1.Caption = value; }
        public string PDFPageNumberLabelFormatCaptain { get => repositoryItemPageNumberEdit1.LabelFormat; set => repositoryItemPageNumberEdit1.LabelFormat = value; }
        public string PDFSetPageNumberButtonCaptain { get => pdfSetPageNumberBarItem1.Caption; set => pdfSetPageNumberBarItem1.Caption = value; }
        public string PDFNextPageButtonCaptain { get => pdfNextPageBarItem1.Caption; set => pdfNextPageBarItem1.Caption = value; }
        public string PDFPreviousPageButtonCaptain { get => pdfPreviousPageBarItem1.Caption; set => pdfPreviousPageBarItem1.Caption = value; }
        public string PDFFindTextButtonCaptain { get => pdfFindTextBarItem1.Caption; set => pdfFindTextBarItem1.Caption = value; }
        public string PDFFilePrintButtonCaptain { get => pdfFilePrintBarItem1.Caption; set => pdfFilePrintBarItem1.Caption = value; }
        public string PDFFileSaveAsButtonCaptain { get => pdfFileSaveAsBarItem1.Caption; set => pdfFileSaveAsBarItem1.Caption = value; }
        public string PDFFileOpenButtonCaptain { get => pdfFileOpenBarItem1.Caption; set => pdfFileOpenBarItem1.Caption = value; }


        public string PDFListTitle { get => accordionControlElement1.Text; set => accordionControlElement1.Text = value; }
        public PDFControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
        }

        public void LoadFile(string file)
        {
            this.pdfViewer1.LoadDocument(file);
        }

        public void LoadFile()
        {
            DialogControl.OpenFileDialogControl openFileDialog = new DialogControl.OpenFileDialogControl();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                this.pdfViewer1.LoadDocument(openFileDialog.FileName);
        }

        public void SaveFile(string file)
        {
            this.pdfViewer1.SaveDocument(file);
        }

        public void SaveFileAs()
        {
            DialogControl.SaveFileDialogControl saveFileDialog = new DialogControl.SaveFileDialogControl();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                this.pdfViewer1.SaveDocument(saveFileDialog.FileName);
        }

        private void pdfViewer1_DocumentChanged(object sender, DevExpress.XtraPdfViewer.PdfDocumentChangedEventArgs e)
        {
            FileInfo file = new FileInfo(e.DocumentFilePath);
            if (!pdf_list.Contains(file.FullName))
            {
                AccordionControlElement pdf = new AccordionControlElement();
                pdf.Name = "accordionControlElement2";
                pdf.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
                pdf.Text = file.Name;
                pdf.Tag = file.FullName;
                pdf.Hint= file.FullName;
                pdf.Click += Pdf_Click;
                pdf.ImageOptions.SvgImage = svgImageCollection1[0];
                this.accordionControlElement1.Elements.Add(pdf);

                pdf_list.Add(file.FullName);
            }
        }

        private void Pdf_Click(object sender, EventArgs e)
        {
            pdfViewer1.LoadDocument((sender as AccordionControlElement).Tag.ToString());
        }
    }
}
