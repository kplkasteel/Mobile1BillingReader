using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Mobile1BillingReader
{
    public partial class Main : Form
    {
        private decimal _billingValue;
        private int _processed;
        private DataTable _myDataTable;
       

        public Main()
        {
            InitializeComponent();
        }

        private async void BtnLoad_Click(object sender, EventArgs e)
        {
            lblSaveState.Text = string.Empty;
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"All Files (*.pdf)|*.pdf",
                FilterIndex = 1,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var arrAllFiles = openFileDialog.FileNames; //used when Multi select = true

            if (!arrAllFiles.Any()) return;


            SetControls(arrAllFiles);

            CreateTable();


            using var tokenSource = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                foreach (var item in arrAllFiles)
                    try
                    {
                        ReadResult(PdfText(item));
                        lblProcessed.SetPropertyValue(a => a.Text, (_processed += 1).ToString());

                    }
                    catch(Exception exception)
                    {
                        // ReSharper disable once LocalizableElement
                        MessageBox.Show(
                            @"Something must have gone wrong, please contact your developer and send through the PDF as below" +
                            "\n\n" + item + "\n\n" + exception.Message,
                            @"Oops!!!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                if (_myDataTable != null) btnExport.SetPropertyValue(a => a.Visible, true);   // ControlVisible(true, btnExport);

            }, tokenSource.Token);
            if (await Task.WhenAny(task).ConfigureAwait(false) == task) return;
            tokenSource.Cancel();
            task.Wait(tokenSource.Token);
        }

        private void CreateTable()
        {
            _myDataTable = new DataTable();
            _myDataTable?.Columns.Add("Invoice Number");
            _myDataTable?.Columns.Add("Invoice Date");
            _myDataTable?.Columns.Add("CellPhone Number");
            _myDataTable?.Columns.Add("Billing Item Date");
            _myDataTable?.Columns.Add("Billing Item");
            _myDataTable?.Columns.Add("Billing Item Value");
        }

        private void SetControls(string[] arrAllFiles)
        {
            _billingValue = 0;
            _processed = 0;
            lblNrOfInvoices.SetPropertyValue(a => a.Text, arrAllFiles.Length.ToString());
        }

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        private static string PdfText(string path)
        {
            var text = string.Empty;
            using var reader = new PdfReader(path);

            for (var page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page,
                    new SimpleTextExtractionStrategy(), new Dictionary<string, IContentOperator>());
            }

            reader.Close();
            reader.Dispose();

            return text;
        }

        private async void BtnExport_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = @"Excel Files (*.xlsx)|*.xlsx",
                FilterIndex = 1
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var filename = saveFileDialog.FileName;

            lblSaveState.SetPropertyValue(a => a.Text, "Exporting please wait...");
            btnExport.SetPropertyValue(a => a.Enabled, false);
            BtnLoad.SetPropertyValue(a => a.Enabled, false);
            await Task.Run(() =>
            {
                try
                {
                    using var wb = new XLWorkbook();
                    wb.Worksheets.Add(_myDataTable, "MTN Billing");
                    wb.Worksheet(1).ShowRowColHeaders = true;
                    wb.Worksheet(1).Column(1).DataType = XLDataType.Text;
                    wb.Worksheet(1).Column(2).DataType = XLDataType.Text;
                    wb.Worksheet(1).Column(3).DataType = XLDataType.Text;
                    wb.Worksheet(1).Column(4).DataType = XLDataType.Text;
                    wb.Worksheet(1).Column(5).DataType = XLDataType.Text;
                    wb.Worksheet(1).Column(6).DataType = XLDataType.Text;
                    wb.SaveAs(filename);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }).ConfigureAwait(false);

            lblSaveState.SetPropertyValue(a => a.Text, "Export completed...");
            btnExport.SetPropertyValue(a => a.Enabled, true);
            BtnLoad.SetPropertyValue(a => a.Enabled, true);
        }


        private  void ReadResult(string result)
        {
           
                var lines = Regex.Split(result, "\n");

                var invoiceDate = string.Empty;
                var invoiceNr = string.Empty;
                var cellPhoneNr =  string.Empty; 
                
                var parseResult1 = (DateTime.TryParseExact(lines[17].Trim(), "dd/MM/yyyy", null, 0, out _));
                var parseResult2 = (DateTime.TryParseExact(lines[18].Trim(), "dd/MM/yyyy", null, 0, out _));

                if (parseResult1)
                {
                    invoiceDate = lines[17].Trim();

                    invoiceNr = lines[11].Substring(lines[11].IndexOf(".:", StringComparison.Ordinal) + 2).Trim();

                    cellPhoneNr = lines[20].Substring(0, lines[20].IndexOf("SUBSCRIBER", StringComparison.Ordinal)).Replace(" ", "");
                }
                if (parseResult2)
                {
                    invoiceDate = lines[18].Trim();

                    invoiceNr = lines[12].Substring(lines[12].IndexOf(".:", StringComparison.Ordinal) + 2).Trim();

                    cellPhoneNr = lines[21].Substring(0, lines[21].IndexOf("SUBSCRIBER", StringComparison.Ordinal)).Replace(" ", "");
                }


                var startLine = 0;
                for (var i = 1; i < lines.Length; i++)
                    if (lines[i].StartsWith("DATE TRANSACTION AMOUNT"))
                        startLine = i + 1;
                for (var line = startLine; line < lines.Length; line++)
                {
                    if (lines[line].StartsWith("TOTAL EXCLUDING VAT")) break;

                    var tempDate = lines[line].Substring(0, 10);
                    var date = string.Empty;
                    if (DateTime.TryParseExact(tempDate, "dd/MM/yyyy", null, 0, out _))
                    {
                        date = tempDate;
                        lines[line] = lines[line].Remove(0, 10);
                    }

                    lines[line] = lines[line].TrimStart(' ');
                    lines[line] = lines[line].TrimEnd(' ');

                    var value = decimal.Parse(ExtractDecimalFromString(lines[line].Replace(".", ",")));

                    var item = lines[line].Substring(0,
                        lines[line].Length - value.ToString(CultureInfo.CurrentCulture).Length);

                    var row = _myDataTable.NewRow();
                    row["Invoice Number"] = invoiceNr;
                    row["Invoice Date"] = invoiceDate;
                    row["CellPhone Number"] = cellPhoneNr;
                    row["Billing Item Date"] = date;
                    row["Billing Item"] = item.Replace("-", string.Empty).Trim();
                    row["Billing Item Value"] = value;
                    _myDataTable.Rows.Add(row);
                    _billingValue += value;

                    lblBillableValue.SetPropertyValue(a => a.Text, $"R {_billingValue.ToString(CultureInfo.CurrentCulture)}");
                }
          
        }

        public string ExtractDecimalFromString(string str)
        {
            if(string.IsNullOrEmpty(str)) return string.Empty;
            var newStr = string.Empty;
            var newStr2 = string.Empty;
            var temp = 0;
            for (var i = str.Length - 1; i > 0; i--)
            {
                if (str[i].ToString() == " ") break;
                {
                    newStr = str[i] + newStr;
                    temp = i;
                }
            }

            for (var i = temp - 2; i > 0; i--)
            {
                if (str[i].ToString() == " ") break;
                {
                    newStr2 = str[i] + newStr2;
                }
            }

            if (newStr2.Contains(",")) return newStr.Replace(" ", string.Empty);
            if (int.TryParse(newStr2, out _))
            {
                newStr = newStr2 + newStr;
            }
            return newStr.Replace(" ", string.Empty);
        }
    }


}