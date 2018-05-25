using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Habraken_SLE.Classes.Elements;

namespace Habraken_SLE.Overlays
{
    /// <summary>
    /// Interaction logic for ElementVarialbes.xaml
    /// </summary>
    public partial class ElementVarialbes : UserControl
    {
        //private List<Barcode> barcodes;
        // private List<Textbox> textBoxes;

        string currentLabel;
        List<string> informationFields = new List<string>();

        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\Periode 11-12\Habraken\Habraken-SLE\dbLabelEditor.mdf;Integrated Security=True";

        public ElementVarialbes()
        {
            InitializeComponent();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var getLabelID = (from g in db.tbl_labels
                              where g.Name == currentLabel
                              select g.Id).FirstOrDefault();

            foreach (var item in lvInfoFields.Items)
            {
                if (item.ToString().Contains("barcode"))
                {
                    var getBarcodeID = (from b in db.tbl_barcodes
                                        where b.LabelID == getLabelID && b.Name == item.ToString().Split(',')[0]
                                        select b.Id).FirstOrDefault();

                    var query = (from information in db.tbl_InformationFields
                                 where information.InformationName == item.ToString().Split(',')[1]
                                 select information).FirstOrDefault();

                    MessageBox.Show(item.ToString().Split(',')[1]);

                    tbl_infoBarcode barcode = new tbl_infoBarcode
                    {
                        BarcodeID = getBarcodeID,
                        InformationID = int.Parse(item.ToString().Split(',')[2]),
                        NumCharPos = int.Parse(item.ToString().Split(',')[3])
                    };

                    db.tbl_infoBarcodes.InsertOnSubmit(barcode);
                    db.SubmitChanges();

                }
                if (item.ToString().Contains("textbox"))
                {
                    var getTextboxID = (from t in db.tbl_textboxes
                                        where t.LabelID == getLabelID && t.Name == item.ToString().Split(',')[0]
                                        select t.Id).FirstOrDefault();

                    var query = (from information in db.tbl_InformationFields
                                 where information.InformationName == item.ToString().Split(',')[1]
                                 select information).FirstOrDefault();

                    tbl_infoTextbox textbox = new tbl_infoTextbox
                    {
                        TextboxID = getTextboxID,
                        InformationID = int.Parse(item.ToString().Split(',')[2]),
                        NumCharPos = int.Parse(item.ToString().Split(',')[3])
                    };

                    db.tbl_infoTextboxes.InsertOnSubmit(textbox);
                    db.SubmitChanges();
                }
            }
            lvInfoFields.Items.Clear();
            cbInfoFields.Items.Clear();
            cbItems.Items.Clear();
            this.Visibility = Visibility.Hidden;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tempvalue;
                if (!int.TryParse(tbInfoCharLengths.Text, out tempvalue))
                {
                    MessageBox.Show("Textbox 'CharLengths' can only contrain a numeric value.", "Warning");
                }
                else
                {
                    var db = new HLE_LinqtoSQLDataContext(con);

                    var query =
                        from information in db.tbl_InformationFields
                        where information.InformationName == cbInfoFields.SelectedItem.ToString()
                        select information.Id;

                    List<int> temp = new List<int>();
                    temp.AddRange(query);

                    informationFields.Add("[" + temp[0].ToString() + "," + tbInfoCharLengths.Text + "]");
                    string info = "" + cbItems.SelectedItem.ToString() + ",\n " + cbInfoFields.SelectedItem.ToString() + ",\n " + temp[0].ToString() + ",\n " + tbInfoCharLengths.Text + "";
                    if (!lvInfoFields.Items.Contains(info))
                    {
                        if (info.Contains("textbox") || info.Contains("barcode"))
                        {
                            lvInfoFields.Items.Add(info);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Value " + cbInfoFields.SelectedItem.ToString() + " was already filled in for element: " + cbItems.SelectedItem.ToString() + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void SetCurrentLabel(string lblName)
        {
            this.currentLabel = lblName;
        }

        public void LoadElements()
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var selectLabel = (from q in db.tbl_labels
                               where q.Name == currentLabel
                               select q).SingleOrDefault();

            if (selectLabel != null)
            {
                if (selectLabel.tbl_barcodes.Any())
                {
                    SelectBarcode(db, selectLabel);
                }

                if (selectLabel.tbl_textboxes.Any())
                {
                    SelectTextbox(db, selectLabel);
                }

                if (selectLabel.tbl_lines.Any())
                {
                    SelectLine(db, selectLabel);
                }

                if (selectLabel.tbl_boxes.Any())
                {
                    SelectBox(db, selectLabel);
                }
            }
        }

        private void SelectBox(HLE_LinqtoSQLDataContext db, tbl_label selectLabel)
        {
            var selectBox = from sbx in db.tbl_boxes
                            where sbx.LabelID == selectLabel.Id
                            select sbx;

            if (selectBox.Any())
            {
                foreach (var item in selectBox)
                {
                    cbItems.Items.Add(item.Name);
                }
            }
        }

        private void SelectLine(HLE_LinqtoSQLDataContext db, tbl_label selectLabel)
        {
            var selectLine = from sl in db.tbl_lines
                             where sl.LabelID == selectLabel.Id
                             select sl;

            if (selectLine.Any())
            {
                foreach (var item in selectLine)
                {
                    cbItems.Items.Add(item.Name);
                }
            }
        }

        private void SelectTextbox(HLE_LinqtoSQLDataContext db, tbl_label selectLabel)
        {
            var selectTextbox = from st in db.tbl_textboxes
                                where st.LabelID == selectLabel.Id
                                select st;

            if (selectTextbox.Any())
            {
                foreach (var item in selectTextbox)
                {
                    cbItems.Items.Add(item.Name);
                }
            }
        }

        private void SelectBarcode(HLE_LinqtoSQLDataContext db, tbl_label selectLabel)
        {
            var selectBarcode = from sb in db.tbl_barcodes
                                where sb.LabelID == selectLabel.Id
                                select sb;

            if (selectBarcode.Any())
            {
                foreach (var item in selectBarcode)
                {
                    cbItems.Items.Add(item.Name);
                }
            }
        }

        public void LoadInfoFields()
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var selectInfoField = from sif in db.tbl_InformationFields
                                  select sif;

            if (selectInfoField.Any())
            {
                foreach (var item in selectInfoField)
                {
                    cbInfoFields.Items.Add(item.InformationName);
                }
            }
        }
    }
}