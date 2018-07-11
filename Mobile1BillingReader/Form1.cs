using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Mobile1BillingReader
{
    public partial class Main : Form
    {
        private decimal _billingvalue;
        private int _processed;
        private int _progres;
        private DataTable _myDataTable;

        public Main()
        {
            InitializeComponent();
            if (_myDataTable == null) btnExport.Visible = false;

            progressBar.Visible = false;
            lblSaveState.Text = string.Empty;
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
            var arrAllFiles = openFileDialog.FileNames; //used when Multiselect = true

            if (!arrAllFiles.Any()) return;
            lblNrOfInvoices.Text = arrAllFiles.Length.ToString();
            _progres = 100 / arrAllFiles.Length;
            _billingvalue = 0;
            _processed = 0;
            progressBar.Visible = true;
            progressBar.Value = 0;
            _myDataTable = new DataTable();
            _myDataTable?.Columns.Add("Invoice Number");
            _myDataTable?.Columns.Add("Invoice Date");
            _myDataTable?.Columns.Add("CellPhone Number");
            _myDataTable?.Columns.Add("Billing Item Date");
            _myDataTable?.Columns.Add("Billing Item");
            _myDataTable?.Columns.Add("Billing Item Value");

            foreach (var item in arrAllFiles)
                try
                {
                    await ReadResult(await PdfText(item)).ConfigureAwait(false);
                    if (progressBar.InvokeRequired)
                        progressBar.Invoke(progressBar.Value + _progres > 100
                            ? delegate { progressBar.Value = _progres; }
                            : new MethodInvoker(delegate { progressBar.Value += _progres; }));
                    else
                        progressBar.Value = progressBar.Value + 1;

                    if (lblProcessed.InvokeRequired)
                        lblProcessed.Invoke(new MethodInvoker(delegate
                        {
                            lblProcessed.Text = (_processed = _processed + 1).ToString();
                        }));
                    else
                        lblProcessed.Text = (_processed + 1).ToString();

                    await Task.Delay(50).ConfigureAwait(false);
                }
                catch
                {
                    // ReSharper disable once LocalizableElement
                    MessageBox.Show(
                        @"Something must have gone wrong, please contact your developer and send through the PDF as below" +
                        "\n\n" + item,
                        @"Oeps!!!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            if (_myDataTable != null)
                if (btnExport.InvokeRequired)
                    btnExport.Invoke(new MethodInvoker(delegate { btnExport.Visible = true; }));
                else
                    btnExport.Visible = true;

            if (progressBar.InvokeRequired)
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.Visible = false; }));
            else
                progressBar.Visible = false;
        }

        private static async Task<string> PdfText(string path)
        {
            var reader = new PdfReader(path);
            var text = string.Empty;
            await Task.Run(() =>
            {
                for (var page = 1; page <= reader.NumberOfPages; page++)
                    text += PdfTextExtractor.GetTextFromPage(reader, page,
                        new SimpleTextExtractionStrategy());

                reader.Close();
            }).ConfigureAwait(false);

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

            await UpdateLabel("Exporting please wait...");
            btnExport.Enabled = false;
            BtnLoad.Enabled = false;
            await Task.Run(() =>
            {
                try
                {
                    using (var wb = new XLWorkbook())
                    {
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
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }).ConfigureAwait(false);


            await UpdateLabel("Export completed...");
            btnExport.Enabled = true;
            BtnLoad.Enabled = true;
        }

        private async Task UpdateLabel(string message)
        {
            await Task.Run(() =>
            {
                if (lblSaveState.InvokeRequired)
                    lblSaveState.Invoke(new MethodInvoker(delegate { lblSaveState.Text = message; }));
                else
                    lblBillableValue.Text = message;
            }).ConfigureAwait(false);
        }

        private async Task ReadResult(string result)
        {
            await Task.Run(() =>
            {
                var lines = Regex.Split(result, "\n");

                var invoiceNr = lines[32].Trim();
                var invoicedate = lines[33].Trim();
                var cellPhoneNr = lines[31].Trim();
                cellPhoneNr = cellPhoneNr.Replace(" ", "");

                var startline = 0;
                for (var i = 1; i < lines.Length; i++)
                    if (lines[i].StartsWith("DATE TRANSACTION AMOUNT"))
                        startline = i + 1;
                for (var line = startline; line < lines.Length; line++)
                {
                    if (lines[line].StartsWith("TOTAL EXCLUDING VAT")) break;

                    var tempDate = lines[line].Substring(0, 10);
                    var date = string.Empty;
                    // ReSharper disable once UnusedVariable
                    if (DateTime.TryParseExact(tempDate, "dd/MM/yyyy", null, 0, out var dt))
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
                    row["Invoice Date"] = invoicedate;
                    row["CellPhone Number"] = cellPhoneNr;
                    row["Billing Item Date"] = date;
                    row["Billing Item"] = item.Replace("-", string.Empty).Trim();
                    row["Billing Item Value"] = value;
                    _myDataTable.Rows.Add(row);
                    _billingvalue = _billingvalue + value;
                    if (lblBillableValue.InvokeRequired)
                        lblBillableValue.Invoke(new MethodInvoker(delegate
                        {
                            lblBillableValue.Text = @"R " +_billingvalue.ToString(CultureInfo.CurrentCulture);
                        }));
                    else
                        lblBillableValue.Text = _billingvalue.ToString(CultureInfo.CurrentCulture);
                }
            }).ConfigureAwait(false);
        }

        public string ExtractDecimalFromString(string str)
        {
            var lenght = str.Length;
            var newStr = string.Empty;
            var newStr2 = string.Empty;
            var temp = 0;
            for (var i = lenght - 1; i > 0; i--)
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


            if (!newStr2.Contains(","))
            {
                if (int.TryParse(newStr2, out _))
                {
                    newStr = newStr2 + newStr;
                }
                
            }
           

            return newStr.Replace(" ", string.Empty);
        }


    }
}