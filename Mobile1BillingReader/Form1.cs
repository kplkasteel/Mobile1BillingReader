using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Mobile1BillingReader
{
    public partial class Main : Form
    {
        private StringBuilder _invoiceList;
        private int _progres;
        private decimal _billingvalue;
        private int _processed;

        public Main()
        {
            InitializeComponent();
            if (_invoiceList == null) btnExport.Visible = false;

            progressBar.Visible = false;
        }

        private async void BtnLoad_Click(object sender, EventArgs e)
        {
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

            _invoiceList = new StringBuilder();
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
                catch (Exception exception)
                {
                    // ReSharper disable once LocalizableElement
                    MessageBox.Show(
                        @"Something must have gone wrong, please contact your deveoper" + "\n\n" + exception,
                        @"Oeps!!!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            if (!string.IsNullOrEmpty(_invoiceList.ToString()))
                if (btnExport.InvokeRequired)
                    btnExport.Invoke(new MethodInvoker(delegate { btnExport.Visible = true; }));
                else
                    btnExport.Visible = true;

            if (progressBar.InvokeRequired)
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.Visible = false; }));
            else
                progressBar.Visible = false;
        }

        private async Task<string> PdfText(string path)
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = @"CSV files (*.csv)|*.csv",
                FilterIndex = 1
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var filename = saveFileDialog.FileName;
            try
            {
                File.WriteAllText(filename, _invoiceList.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
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
                {
                    if (lines[i].StartsWith("DATE TRANSACTION AMOUNT"))
                    {
                        startline = i + 1;
                    }
                }
                for (var line = 1; line < lines.Length; line++)
                {
                    if (lines[line].StartsWith("TOTAL EXCLUDING VAT")) break;

                    if (line < startline) continue;
                    var itemresult = lines[line];
                    if (itemresult == "- EAGLE EYE - 13.16 13.16")
                    {
                        var date = string.Empty;
                        var item = lines[line].Substring(0, 19);
                        var value = lines[line].Substring(20, lines[line].Length - 20);
                        value = value.Trim();
                        _invoiceList.AppendLine($"{invoiceNr},{invoicedate},{cellPhoneNr},{date},{item},{value}");
                        _billingvalue = _billingvalue + decimal.Parse(value.Replace(".", ","));
                        if (lblBillableValue.InvokeRequired)
                            lblBillableValue.Invoke(new MethodInvoker(delegate
                            {
                                lblBillableValue.Text = _billingvalue.ToString(CultureInfo.InvariantCulture);
                            }));
                        else
                            lblBillableValue.Text = _billingvalue.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var tempDate = lines[line].Substring(0, 10);
                        var date = string.Empty;
                        // ReSharper disable once UnusedVariable
                        if (DateTime.TryParse(tempDate, out var dt))
                        {
                            date = tempDate;
                            lines[line] = lines[line].Remove(0, 10);
                        }

                        lines[line] = lines[line].TrimStart(' ');
                        lines[line] = lines[line].TrimStart(' ');
                        var item = Regex.Replace(lines[line], @"[\d-,.]", string.Empty);
                        item = item.Trim();
                        var value = lines[line].Replace(item, "");
                        value = value.Trim();
                        _invoiceList.AppendLine($"{invoiceNr},{invoicedate},{cellPhoneNr},{date},{item},{value}");
                        _billingvalue = _billingvalue + decimal.Parse(value.Replace(".", ","));
                        if (lblBillableValue.InvokeRequired)
                            lblBillableValue.Invoke(new MethodInvoker(delegate
                            {
                                lblBillableValue.Text = _billingvalue.ToString(CultureInfo.InvariantCulture);
                            }));
                        else
                            lblBillableValue.Text = _billingvalue.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }).ConfigureAwait(false);
        }
    }
}