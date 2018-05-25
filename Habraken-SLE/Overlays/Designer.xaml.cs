using Spire.Barcode;
using System;
using System.Collections.Generic;
using System.IO;
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
using WPF.JoshSmith.Controls;


namespace Habraken_SLE.Overlays
{
    /// <summary>
    /// Interaction logic for Designer.xaml
    /// </summary>
    public partial class Designer : UserControl
    {

        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\Periode 11-12\Habraken\Habraken-SLE\dbLabelEditor.mdf;Integrated Security=True";


        public TextBox textbox;
        public Image barcode, bcTemp;
        public Line line, lTemp;
        public Rectangle box, bTemp;
        private string elementBeingDragged;
        private int boxIndex, lineIndex, textboxIndex, barcodeIndex;
        private bool firstClick, firstGenerate, finishedAddingComboBoxItems, activeElement;

        List<TextBox> tbList = new List<TextBox>();
        List<Image> barcodeList = new List<Image>();
        List<Line> lineList = new List<Line>();
        List<Rectangle> boxList = new List<Rectangle>();

        public Designer()
        {
            InitializeComponent();

            btnSave.IsEnabled = false;
            btnNew.IsEnabled = true;
            btnOpen.IsEnabled = false;
            btnDelete.IsEnabled = false;

            UpdateListview();
        }

        private void liBarcode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!activeElement)
            {
                // Generate the barcode
                if (firstGenerate == false)
                {
                    BarcodeSettings bs = new BarcodeSettings();
                    bs.Type = BarCodeType.Code128;
                    bs.Data = "placeholder";
                    BarCodeGenerator bg = new BarCodeGenerator(bs);
                    bg.GenerateImage().Save("code128.png");
                }

                barcode = new Image();
                barcode.Margin = new Thickness(0, 0, 157, 0);
                barcode.Width = 100;
                barcode.Height = 50;
                barcode.MouseUp += new MouseButtonEventHandler(this.Element_MouseUp);

                string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "code128.png");
                BitmapImage barcode_ = new BitmapImage();
                barcode_.BeginInit();
                barcode_.UriSource = new Uri(filePath);
                barcode_.EndInit();
                barcode.Source = barcode_;
                barcode.Name = "barcode_" + barcodeIndex;
                elementBeingDragged = "barcode";
                canvas_.Children.Add(barcode);
                barcodeIndex++;
                firstGenerate = true;
                activeElement = true;
            }
            else
            {
                MessageBox.Show("Element is already Active");
            }
        } // Generate New Barcode

        private void liLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!activeElement)
            {
                line = new Line();
                line.X1 = 50;
                line.Y1 = 50;
                line.X2 = 0;
                line.Y2 = 0;
                line.StrokeThickness = 5;
                line.MouseUp += new MouseButtonEventHandler(this.Element_MouseUp);
                SolidColorBrush colorBlack = new SolidColorBrush();
                colorBlack.Color = Colors.Black;
                line.Stroke = colorBlack;
                line.Name = "line_" + lineIndex;
                elementBeingDragged = "line";
                canvas_.Children.Add(line);
                lineIndex++;
                activeElement = true;
            }
            else
            {
                MessageBox.Show("Element is already Active");
            }
        } // Generate New Line     

        private void liTextbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!activeElement)
            {
                textbox = new TextBox();
                textbox.Width = 100;
                textbox.Height = 25;
                textbox.Margin = new Thickness(0, 0, 149, 0);
                textbox.Text = "textbox_" + textboxIndex;
                textbox.Name = "textbox_" + textboxIndex;
                textbox.IsReadOnly = true;
                textbox.MouseUp += new MouseButtonEventHandler(this.Element_MouseUp);
                elementBeingDragged = "textbox";
                canvas_.Children.Add(textbox);
                textboxIndex++;
                activeElement = true;
            }
            else
            {
                MessageBox.Show("Element is already Active");
            }

        }// Generate New Textbox

        private void liBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!activeElement)
            {
                box = new Rectangle();
                box.Height = 25;
                box.Width = 60;
                box.RadiusY = 5;
                box.RadiusX = 5;
                box.StrokeThickness = 5;
                box.MouseUp += new MouseButtonEventHandler(this.Element_MouseUp);
                SolidColorBrush colorBlack = new SolidColorBrush();
                colorBlack.Color = Colors.Black;
                box.Stroke = colorBlack;
                elementBeingDragged = "box";
                box.Name = "_box_" + boxIndex;
                canvas_.Children.Add(box);
                boxIndex++;
                activeElement = true;
            }
            else
            {
                MessageBox.Show("Element is already Active");
            }
        } // Generate New Box

        private void gr_preview_MouseEnter(object sender, MouseEventArgs e)
        {
            if (lbToolbox.SelectedItem != null)
            {
                if (elementBeingDragged == "textbox")
                {
                    TextBox tempTextbox = new TextBox();
                    tempTextbox.Width = 100;
                    tempTextbox.Height = 25;
                    tempTextbox.Text = textbox.Name;
                    tempTextbox.Name = "textbox_";
                    tempTextbox.IsReadOnly = true;
                    tempTextbox.Name = textbox.Name;
                    tbList.Add(tempTextbox);
                    canvas_.Children.Remove(textbox);
                    canvas_Preview.Children.Add(tempTextbox);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }
                if (elementBeingDragged == "box")
                {
                    Rectangle tempBox = new Rectangle();
                    tempBox.Height = 25;
                    tempBox.Width = 60;
                    tempBox.StrokeThickness = 3;
                    SolidColorBrush colorBlack = new SolidColorBrush();
                    colorBlack.Color = Colors.Black;
                    tempBox.Stroke = colorBlack;
                    tempBox.Name = box.Name;
                    boxList.Add(tempBox);
                    canvas_.Children.Remove(box);
                    canvas_Preview.Children.Add(tempBox);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }
                if (elementBeingDragged == "line")
                {
                    Line tempLine = new Line();
                    tempLine.X1 = 0;
                    tempLine.Y1 = 0;
                    tempLine.X2 = 50;
                    tempLine.Y2 = 20;
                    tempLine.Width = 50;
                    tempLine.Height = 3;
                    tempLine.StrokeThickness = 3;

                    SolidColorBrush colorBlack = new SolidColorBrush();
                    colorBlack.Color = Colors.Black;
                    line.Stroke = colorBlack;

                    tempLine.Name = line.Name;
                    lineList.Add(tempLine);
                    canvas_.Children.Remove(line);
                    canvas_Preview.Children.Add(tempLine);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }
                if (elementBeingDragged == "barcode")
                {
                    Image tempBarcode = new Image();
                    tempBarcode.Width = 100;
                    tempBarcode.Height = 30;
                    string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "code128.png");
                    BitmapImage barcodeImage = new BitmapImage();
                    barcodeImage.BeginInit();
                    barcodeImage.UriSource = new Uri(filePath);
                    barcodeImage.EndInit();

                    tempBarcode.Source = barcodeImage;
                    tempBarcode.Name = barcode.Name;
                    tempBarcode.Stretch = Stretch.Fill;

                    elementBeingDragged = "barcode";
                    barcodeList.Add(tempBarcode);
                    canvas_.Children.Remove(barcode);
                    canvas_Preview.Children.Add(tempBarcode);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }
                activeElement = false;
            }
        } // Handle Element Drop On Preview Grid

        private void Element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (firstClick == false)
            {
                canvas_.Children.Remove(textbox);
                canvas_.Children.Remove(line);
                canvas_.Children.Remove(barcode);
                canvas_.Children.Remove(box);
                firstClick = true;
            }
            else
            {
                firstClick = false;
            }
        } // Handle Element Dispose

        private void UpdateComboBox()
        {
            finishedAddingComboBoxItems = false;
            Cb_controls.Items.Clear();

            foreach (TextBox tb in tbList)
            {
                Cb_controls.Items.Add(tb.Name);
            }
            foreach (Line l in lineList)
            {
                Cb_controls.Items.Add(l.Name);
            }
            foreach (Image bc in barcodeList)
            {
                Cb_controls.Items.Add(bc.Name);
            }
            foreach (Rectangle box in boxList)
            {
                Cb_controls.Items.Add(box.Name);
            }
            finishedAddingComboBoxItems = true;
        } // Handle Update Of Combobox       

        private void Cb_controls_SelectionChanged(object sender, SelectionChangedEventArgs e) // Update Value Boxes In Properties Grid
        {
            if (rb_boxEdgesCurved.Visibility == Visibility.Visible)
            {
                rb_boxEdgesCurved.Visibility = Visibility.Hidden;
            }
            if (finishedAddingComboBoxItems == true)
            {
                if (Cb_controls.SelectedItem.ToString().Contains("textbox"))
                {
                    foreach (TextBox element in tbList)
                    {
                        if (element.Name == Cb_controls.SelectedItem.ToString())
                        {
                            Point relativePoint = element.TransformToAncestor(gr_preview).Transform(new Point(0, 0));
                            tb_width.Text = element.Width.ToString();
                            tb_height.Text = element.Height.ToString();
                            tb_posX.Text = relativePoint.X.ToString();
                            tb_posY.Text = relativePoint.Y.ToString();
                        }
                    }
                }
                else if (Cb_controls.SelectedItem.ToString().Contains("line"))
                {
                    foreach (Line element in lineList)
                    {
                        if (element.Name == Cb_controls.SelectedItem.ToString())
                        {
                            Point relativePoint = element.TransformToAncestor(gr_preview).Transform(new Point(0, 0));
                            tb_width.Text = element.Width.ToString();
                            tb_height.Text = element.Height.ToString();
                            tb_posX.Text = relativePoint.X.ToString();
                            tb_posY.Text = relativePoint.Y.ToString();
                        }
                    }
                }
                else if (Cb_controls.SelectedItem.ToString().Contains("_box"))
                {
                    rb_boxEdgesCurved.Visibility = Visibility.Visible;
                    foreach (Rectangle element in boxList)
                    {
                        if (element.Name == Cb_controls.SelectedItem.ToString())
                        {
                            Point relativePoint = element.TransformToAncestor(gr_preview).Transform(new Point(0, 0));
                            tb_width.Text = element.ActualWidth.ToString();
                            tb_height.Text = element.ActualHeight.ToString();
                            tb_posX.Text = relativePoint.X.ToString();
                            tb_posY.Text = relativePoint.Y.ToString();
                        }
                    }
                }
                else if (Cb_controls.SelectedItem.ToString().Contains("barcode"))
                {
                    foreach (Image element in barcodeList)
                    {
                        if (element.Name == Cb_controls.SelectedItem.ToString())
                        {
                            double positionX = Canvas.GetLeft(element);
                            double positionY = Canvas.GetTop(element);
                            tb_width.Text = element.Width.ToString();
                            tb_height.Text = element.Height.ToString();
                            tb_posX.Text = positionX.ToString();
                            tb_posY.Text = positionY.ToString();
                        }
                    }
                }
            }
        }

        private void btn_UpdateElement_Click(object sender, RoutedEventArgs e) // Resize / Move Elements In Preview
        {
            double canvasWidth = gr_preview.Width;
            double canvasHeight = gr_preview.Height;

            if (Cb_controls.SelectedItem.ToString().Contains("textbox"))
            {
                foreach (TextBox element in tbList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        double posX = Convert.ToDouble(tb_posX.Text);
                        double posY = Convert.ToDouble(tb_posY.Text);

                        // check if element goes out of canvas
                        if (posX < 0 || posX > canvasWidth - element.ActualWidth)
                        {
                            MessageBox.Show("Value for 'Pos X' out of bounds.\n Maximum value for 'Pos X' is " + (gr_preview.Width - element.Width), "Warning");
                        }
                        else
                        {
                            Canvas.SetLeft(element, Convert.ToInt32(posX));
                        }
                        if (posY < 0 || posY > canvasHeight - element.ActualHeight)
                        {
                            MessageBox.Show("Value for 'Pos Y' out of bounds.\n Maximum value for 'Pos Y' is " + gr_preview.Height.ToString(), "Warning");
                        }
                        else
                        {
                            Canvas.SetTop(element, Convert.ToInt32(posY));
                        }

                        double newWidth = Convert.ToDouble(tb_width.Text);
                        double newHeight = Convert.ToDouble(tb_height.Text);
                        if (posX > canvasWidth - newWidth)
                        {
                            MessageBox.Show("Unable to change width of element.\nElement would go offscreen.", "Warning");
                        }
                        else
                        {
                            element.Width = Convert.ToInt32(tb_width.Text);
                        }
                        if (posY > canvasHeight - newHeight)
                        {
                            MessageBox.Show("Unable to change height of element.\nElement would go offscreen.", "Warning");
                        }
                        else
                        {
                            element.Height = Convert.ToInt32(tb_height.Text);
                        }
                    }
                }
            }

            if (Cb_controls.SelectedItem.ToString().Contains("_box"))
            {
                foreach (Rectangle element in boxList)
                {
                    if (box.Name == Cb_controls.SelectedItem.ToString())
                    {
                        double posX = Convert.ToDouble(tb_posX.Text);
                        double posY = Convert.ToDouble(tb_posY.Text);

                        // check if element goes out of canvas
                        if (posX < 0 || posX > canvasWidth - element.ActualWidth)
                        {
                            MessageBox.Show("Value for 'Pos X' out of bounds.\n Maximum value for 'Pos X' is " + (gr_preview.Width - element.Width), "Warning");
                        }
                        else
                        {
                            Canvas.SetLeft(element, Convert.ToInt32(posX));
                        }
                        if (posY < 0 || posY > canvasHeight - element.ActualHeight)
                        {
                            MessageBox.Show("Value for 'Pos Y' out of bounds.\n Maximum value for 'Pos Y' is " + gr_preview.Height.ToString(), "Warning");
                        }
                        else
                        {
                            Canvas.SetTop(element, Convert.ToInt32(posY));
                        }

                        double newWidth = Convert.ToDouble(tb_width.Text);
                        double newHeight = Convert.ToDouble(tb_height.Text);
                        if (posX > canvasWidth - newWidth)
                        {
                            MessageBox.Show("Unable to change width of element.\nElement would go offscreen.", "Warning");
                        }
                        else
                        {
                            element.Width = Convert.ToInt32(tb_width.Text);
                        }
                        if (posY > canvasHeight - newHeight)
                        {
                            MessageBox.Show("Unable to change height of element.\nElement would go offscreen.", "Warning");
                        }
                        else
                        {
                            element.Height = Convert.ToInt32(tb_height.Text);
                        }

                        if (rb_boxEdgesCurved.IsChecked == true)
                        {
                            element.RadiusY = 5;
                            element.RadiusX = 5;
                        }
                        else
                        {
                            element.RadiusY = 0;
                            element.RadiusX = 0;
                        }
                    }
                }
            }

            if (Cb_controls.SelectedItem.ToString().Contains("line"))
            {
                foreach (Line element in lineList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        double posX = Convert.ToDouble(tb_posX.Text);
                        double posY = Convert.ToDouble(tb_posY.Text);

                        // check if element goes out of canvas
                        if (posX < 0 || posX > canvasWidth - element.ActualWidth)
                        {
                            MessageBox.Show("Value for 'Pos X' out of bounds.\n Maximum value for 'Pos X' is " + (gr_preview.Width - element.Width), "Warning");
                        }
                        else
                        {
                            Canvas.SetLeft(element, Convert.ToInt32(posX));
                        }
                        if (posY < 0 || posY > canvasHeight - element.ActualHeight)
                        {
                            MessageBox.Show("Value for 'Pos Y' out of bounds.\n Maximum value for 'Pos Y' is " + gr_preview.Height.ToString(), "Warning");
                        }
                        else
                        {
                            Canvas.SetTop(element, Convert.ToInt32(posY));
                        }

                        double newWidth = Convert.ToDouble(tb_width.Text);
                        double newHeight = Convert.ToDouble(tb_height.Text);
                        if (posX > canvasWidth - newWidth)
                        {
                            MessageBox.Show("Unable to change width of element.\nElement would go offscreen.", "Warning");
                        }
                        else
                        {
                            element.Width = Convert.ToInt32(tb_width.Text);
                        }
                        if (posY > canvasHeight - newHeight)
                        {
                            MessageBox.Show("Unable to change height of element.\nElement would go offscreen.", "Warning");
                        }
                        else
                        {
                            element.Height = Convert.ToInt32(tb_height.Text);
                        }

                    }
                }
            }

            if (Cb_controls.SelectedItem.ToString().Contains("barcode"))
            {
                foreach (Image element in barcodeList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        double posX = Convert.ToDouble(tb_posX.Text);
                        double posY = Convert.ToDouble(tb_posY.Text);

                        try
                        {


                            // check if element goes out of canvas
                            if (posX < 0 || posX > canvasWidth - element.ActualWidth)
                            {
                                MessageBox.Show("Value for 'Pos X' out of bounds.\n Maximum value for 'Pos X' is " + (gr_preview.Width - element.Width), "Warning");
                            }
                            else
                            {
                                Canvas.SetLeft(element, Convert.ToInt32(posX));
                            }
                            if (posY < 0 || posY > canvasHeight - element.ActualHeight)
                            {
                                MessageBox.Show("Value for 'Pos Y' out of bounds.\n Maximum value for 'Pos Y' is " + gr_preview.Height.ToString(), "Warning");
                            }
                            else
                            {
                                Canvas.SetTop(element, Convert.ToInt32(posY));
                            }

                            double newWidth = Convert.ToDouble(tb_width.Text);
                            double newHeight = Convert.ToDouble(tb_height.Text);
                            if (posX > canvasWidth - newWidth)
                            {
                                MessageBox.Show("Unable to change width of element.\nElement would go offscreen.", "Warning");
                            }
                            else
                            {
                                element.Width = Convert.ToInt32(tb_width.Text);
                            }
                            if (posY > canvasHeight - newHeight)
                            {
                                MessageBox.Show("Unable to change height of element.\nElement would go offscreen.", "Warning");
                            }
                            else
                            {
                                element.Height = Convert.ToInt32(tb_height.Text);
                            }

                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Dit Element kan niet buiten de grid komen te staan");
                        }
                    }
                }
            }
        }

        private void btn_removeElement_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> temptblist = new List<TextBox>();
            List<Line> templinelist = new List<Line>();
            List<Rectangle> tempboxlist = new List<Rectangle>();
            List<Image> tempbarcodelist = new List<Image>();

            if (Cb_controls.SelectedItem.ToString().Contains("textbox"))
            {
                temptblist = tbList;
                foreach (TextBox element in tbList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        if (MessageBox.Show("Are you sure you want to remove this element?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            canvas_Preview.Children.Remove(element);
                            temptblist.Remove(element);
                            break;
                        }
                    }
                }
                tbList = temptblist;
            }

            if (Cb_controls.SelectedItem.ToString().Contains("_box"))
            {
                tempboxlist = boxList;
                foreach (Rectangle element in boxList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        if (MessageBox.Show("Are you sure you want to remove this element?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            canvas_Preview.Children.Remove(element);
                            tempboxlist.Remove(element);
                            break;
                        }
                    }
                }
                boxList = tempboxlist;
            }

            if (Cb_controls.SelectedItem.ToString().Contains("line"))
            {
                templinelist = lineList;
                foreach (Line element in lineList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        if (MessageBox.Show("Are you sure you want to remove this element?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            canvas_Preview.Children.Remove(element);
                            templinelist.Remove(element);
                            break;
                        }
                    }
                }
                lineList = templinelist;
            }

            if (Cb_controls.SelectedItem.ToString().Contains("barcode"))
            {
                tempbarcodelist = barcodeList;
                foreach (Image element in barcodeList)
                {
                    if (element.Name == Cb_controls.SelectedItem.ToString())
                    {
                        if (MessageBox.Show("Are you sure you want to remove this element?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            canvas_Preview.Children.Remove(element);
                            tempbarcodelist.Remove(element);
                            break;
                        }
                    }
                }
                barcodeList = tempbarcodelist;
            }
            UpdateComboBox();
            tb_height.Text = "0";
            tb_width.Text = "0";
            tb_posX.Text = "0";
            tb_posY.Text = "0";
        } // Removes the selected element

        private void lvLabels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvLabels.SelectedItem != null)
            {
                btnOpen.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnOpen.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvLabels.SelectedItem != null)
            {
                if (MessageBox.Show("Do you really wish to Delete this Item?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var db = new HLE_LinqtoSQLDataContext(con);

                    var query = (from q in db.tbl_labels
                                 where q.Name == lvLabels.SelectedItem.ToString().Split(',')[0].Remove(0, 6)
                                 select q).SingleOrDefault();

                    if (query != null)
                    {
                        var selectBarcode = from q in db.tbl_barcodes
                                            where q.LabelID == query.Id
                                            select q;

                        db.tbl_barcodes.DeleteAllOnSubmit(selectBarcode);

                        var selectTextbox = from q in db.tbl_textboxes
                                            where q.LabelID == query.Id
                                            select q;

                        db.tbl_textboxes.DeleteAllOnSubmit(selectTextbox);

                        var selectBox = from q in db.tbl_boxes
                                        where q.LabelID == query.Id
                                        select q;

                        db.tbl_boxes.DeleteAllOnSubmit(selectBox);

                        var selectLines = from q in db.tbl_lines
                                          where q.LabelID == query.Id
                                          select q;


                        db.tbl_lines.DeleteAllOnSubmit(selectLines);

                        db.tbl_labels.DeleteOnSubmit(query);

                        db.SubmitChanges();
                        UpdateListview();


                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            canvas_Preview.Children.Clear();
            lineList.Clear();
            barcodeList.Clear();
            tbList.Clear();
            tbLabelName.Text = "";
            boxList.Clear();
            Cb_controls.Items.Clear();
        }

        private void tbLabelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbLabelName.Text != "" || tbLabelName.Text != null)
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var query = (from q in db.tbl_labels
                         where q.Name == lvLabels.SelectedItem.ToString().Split(',')[0].Remove(0, 6)
                         select q).SingleOrDefault();

            if (query != null)
            {
                tbLabelName.Text = query.Name;

                foreach (var item in query.tbl_barcodes)
                {
                    Image tempBarcode = new Image();
                    tempBarcode.Width = item.Width;
                    tempBarcode.Height = item.Height;

                    string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "code128.png");
                    BitmapImage barcodeImage = new BitmapImage();
                    barcodeImage.BeginInit();
                    barcodeImage.UriSource = new Uri(filePath);
                    barcodeImage.EndInit();

                    tempBarcode.Source = barcodeImage;
                    tempBarcode.Name = item.Name;
                    tempBarcode.Stretch = Stretch.Fill;
                    //tempBarcode.Margin = new Thickness(item.PosX, item.PosY, 0, 0);
                    Canvas.SetTop(tempBarcode, item.PosY);
                    Canvas.SetLeft(tempBarcode, item.PosX);

                    elementBeingDragged = "barcode";
                    barcodeList.Add(tempBarcode);
                    canvas_Preview.Children.Add(tempBarcode);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }

                foreach (var item in query.tbl_boxes)
                {
                    Rectangle tempBox = new Rectangle();
                    tempBox.Height = item.Heigth;
                    tempBox.Width = item.Width;
                    tempBox.StrokeThickness = 3;
                    tempBox.Margin = new Thickness(item.PosX, item.PosY, 0, 0);

                    SolidColorBrush colorBlack = new SolidColorBrush();
                    colorBlack.Color = Colors.Black;
                    tempBox.Stroke = colorBlack;
                    tempBox.Name = item.Name;

                    boxList.Add(tempBox);
                    canvas_.Children.Remove(box);
                    canvas_Preview.Children.Add(tempBox);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }

                foreach (var item in query.tbl_lines)
                {
                    Line tempLine = new Line();
                    tempLine.X1 = 0;
                    tempLine.Y1 = 0;
                    tempLine.X2 = item.PosX;
                    tempLine.Y2 = item.PosY;
                    tempLine.Name = item.Name;
                    tempLine.Width = 50;
                    tempLine.Height = 3;
                    tempLine.StrokeThickness = 3;

                    SolidColorBrush colorBlack = new SolidColorBrush();
                    colorBlack.Color = Colors.Black;
                    tempLine.Stroke = colorBlack;

                    lineList.Add(tempLine);
                    //canvas_.Children.Remove(line);
                    canvas_Preview.Children.Add(tempLine);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }

                foreach (var item in query.tbl_textboxes)
                {
                    TextBox tempTextbox = new TextBox();
                    tempTextbox.Width = item.Width;
                    tempTextbox.Height = item.Height;
                    tempTextbox.Text = item.Name;
                    tempTextbox.Name = item.Name;
                    tempTextbox.Margin = new Thickness(item.PosX, item.PosY, 0, 0);
                    tempTextbox.IsReadOnly = true;

                    tbList.Add(tempTextbox);
                    canvas_.Children.Remove(textbox);
                    canvas_Preview.Children.Add(tempTextbox);
                    elementBeingDragged = "";
                    UpdateComboBox();
                }
            }

            btnOpen.IsEnabled = false;
            lvLabels.SelectedItem = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (tbLabelName.Text != null || tbLabelName.Text != "")
            {
                try
                {
                    var db = new HLE_LinqtoSQLDataContext(con);

                    var selectLabel = (from q in db.tbl_labels
                                       where q.Name == tbLabelName.Text
                                       select q).SingleOrDefault();

                    if (selectLabel != null)
                    {
                        selectLabel.Heigth = (int)gr_preview.Height;
                        selectLabel.Width = (int)gr_preview.Width;

                        if (barcodeList.Any())
                        {
                            SelectBarcode();
                        }

                        if (tbList.Any())
                        {
                            SelectTextbox();
                        }

                        if (lineList.Any())
                        {
                            SelectLine();
                        }

                        if (boxList.Any())
                        {
                            SelectBox();
                        }
                    }
                    else
                    {
                        tbl_label label = new tbl_label
                        {
                            Heigth = (int)gr_preview.Height,
                            Width = (int)gr_preview.Width,
                            Name = tbLabelName.Text
                        };

                        db.tbl_labels.InsertOnSubmit(label);

                        db.SubmitChanges();

                        if (barcodeList.Any())
                        {
                            SelectBarcode();
                        }

                        if (tbList.Any())
                        {
                            SelectTextbox();
                        }

                        if (lineList.Any())
                        {
                            SelectLine();
                        }

                        if (boxList.Any())
                        {
                            SelectBox();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Something went wrong While Saving this Label");
                }
                finally
                {
                    ucElementsVariables.SetCurrentLabel(tbLabelName.Text);
                    ucElementsVariables.LoadElements();
                    ucElementsVariables.LoadInfoFields();
                    ucElementsVariables.Visibility = Visibility.Visible;
                    UpdateListview();
                }
            }
            else
            {
                MessageBox.Show("You forgot to Give your label a Name");
            }
        }

        private void UpdateListview()
        {
            lvLabels.Items.Clear();

            var db = new HLE_LinqtoSQLDataContext(con);

            var query = from q in db.tbl_labels
                        select q;

            foreach (var item in query)
            {
                lvLabels.Items.Add(String.Format("Name: {0},\nNumber of Elements: {1}", item.Name, CountElements(db, item)));
            }
        }

        private int CountElements(HLE_LinqtoSQLDataContext db, tbl_label lbl)
        {
            int numberOfElements = 0;

            var countBarcodes = (from cb in db.tbl_barcodes
                                 where cb.LabelID == lbl.Id
                                 select cb).Count();

            var countTextboxes = (from tb in db.tbl_textboxes
                                  where tb.LabelID == lbl.Id
                                  select tb).Count();

            var countBoxes = (from bx in db.tbl_boxes
                              where bx.LabelID == lbl.Id
                              select bx).Count();

            var countLines = (from ln in db.tbl_lines
                              where ln.LabelID == lbl.Id
                              select ln).Count();

            numberOfElements = countBarcodes + countTextboxes + countBoxes + countLines;

            return numberOfElements;
        }

        private void SelectBox()
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var selectLabel = (from q in db.tbl_labels
                               where q.Name == tbLabelName.Text
                               select q).SingleOrDefault();

            var selectbox = from st in db.tbl_boxes
                            where st.LabelID == selectLabel.Id
                            select st;

            if (selectbox.Any())
            {
                foreach (var listItem in boxList)
                {
                    foreach (var bx in selectbox)
                    {
                        var boxes = (from l in selectbox
                                     where listItem.Name == l.Name
                                     select l).SingleOrDefault();

                        if (boxes != null)
                        {
                            boxes.PosX = Canvas.GetLeft(listItem);
                            boxes.PosY = Canvas.GetTop(listItem);
                            boxes.Heigth = (int)listItem.Height;
                            boxes.Width = (int)listItem.Width;
                            db.SubmitChanges();
                        }
                        else
                        {
                            double tempPosX, tempPosY;
                            if (Canvas.GetLeft(listItem) < 0)
                            {
                                tempPosX = 0;
                            }
                            else
                            {
                                tempPosX = Canvas.GetLeft(listItem);
                            }
                            if (Canvas.GetTop(listItem) < 0)
                            {
                                tempPosY = 0;
                            }
                            else
                            {
                                tempPosY = Canvas.GetTop(listItem);
                            }

                            tbl_box box = new tbl_box
                            {
                                LabelID = selectLabel.Id,
                                Name = listItem.Name,
                                PosX = tempPosX,
                                PosY = tempPosY
                            };

                            db.tbl_boxes.InsertOnSubmit(box);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in boxList)
                {
                    double tempPosX, tempPosY;
                    if (Canvas.GetLeft(item) <= 0)
                    {
                        tempPosX = 0;
                    }
                    else
                    {
                        tempPosX = Canvas.GetLeft(item);
                    }
                    if (Canvas.GetTop(item) <= 0)
                    {
                        tempPosY = 0;
                    }
                    else
                    {
                        tempPosY = Canvas.GetTop(item);
                    }

                    tbl_box box = new tbl_box
                    {
                        LabelID = selectLabel.Id,
                        Name = item.Name,
                        PosX = tempPosX,
                        PosY = tempPosY
                    };

                    db.tbl_boxes.InsertOnSubmit(box);
                    db.SubmitChanges();
                }
            }
        }

        private void SelectLine()
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var selectLabel = (from q in db.tbl_labels
                               where q.Name == tbLabelName.Text
                               select q).SingleOrDefault();

            var selectLine = from st in db.tbl_lines
                             where st.LabelID == selectLabel.Id
                             select st;

            if (selectLine.Any())
            {
                foreach (var listItem in lineList)
                {
                    foreach (var ln in selectLine)
                    {
                        var lines = (from l in selectLine
                                     where listItem.Name == l.Name
                                     select l).SingleOrDefault();

                        if (lines != null)
                        {
                            lines.PosX = Canvas.GetLeft(listItem);
                            lines.PosY = Canvas.GetTop(listItem);

                            db.SubmitChanges();
                        }
                        else
                        {
                            double tempPosX, tempPosY;
                            if (Canvas.GetLeft(listItem) <= 0)
                            {
                                tempPosX = 0;
                            }
                            else
                            {
                                tempPosX = Canvas.GetLeft(listItem);
                            }
                            if (Canvas.GetTop(listItem) <= 0)
                            {
                                tempPosY = 0;
                            }
                            else
                            {
                                tempPosY = Canvas.GetTop(listItem);
                            }

                            tbl_line line = new tbl_line
                            {
                                LabelID = selectLabel.Id,
                                Name = listItem.Name,
                                PosX = tempPosX,
                                PosY = tempPosY
                            };

                            db.tbl_lines.InsertOnSubmit(line);

                            db.SubmitChanges();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in lineList)
                {
                    double tempPosX, tempPosY;
                    if (Canvas.GetLeft(item) <= 0)
                    {
                        tempPosX = 0;
                    }
                    else
                    {
                        tempPosX = Canvas.GetLeft(item);
                    }
                    if (Canvas.GetTop(item) <= 0)
                    {
                        tempPosY = 0;
                    }
                    else
                    {
                        tempPosY = Canvas.GetTop(item);
                    }

                    tbl_line line = new tbl_line
                    {
                        LabelID = selectLabel.Id,
                        Name = item.Name,
                        PosX = tempPosX,
                        PosY = tempPosY
                    };

                    db.tbl_lines.InsertOnSubmit(line);
                    db.SubmitChanges();
                }
            }
        }

        private void SelectTextbox()
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var selectLabel = (from q in db.tbl_labels
                               where q.Name == tbLabelName.Text
                               select q).SingleOrDefault();

            var selectTextbox = from st in db.tbl_textboxes
                                where st.LabelID == selectLabel.Id
                                select st;

            if (selectTextbox.Any())
            {
                foreach (var listItem in tbList)
                {
                    foreach (var tb in selectTextbox)
                    {
                        var textboxes = (from t in selectTextbox
                                         where listItem.Name == t.Name
                                         select t).SingleOrDefault();

                        if (textboxes != null)
                        {
                            textboxes.PosX = Canvas.GetLeft(listItem);
                            textboxes.PosY = Canvas.GetTop(listItem);
                            textboxes.Height = (int)listItem.Height;
                            textboxes.Width = (int)listItem.Width;

                            db.SubmitChanges();
                        }
                        else
                        {
                            double tempPosX, tempPosY;
                            if (Canvas.GetLeft(listItem) <= 0)
                            {
                                tempPosX = 0;
                            }
                            else
                            {
                                tempPosX = Canvas.GetLeft(listItem);
                            }
                            if (Canvas.GetTop(listItem) <= 0)
                            {
                                tempPosY = 0;
                            }
                            else
                            {
                                tempPosY = Canvas.GetTop(listItem);
                            }

                            tbl_textbox textbox = new tbl_textbox
                            {
                                LabelID = selectLabel.Id,
                                Name = listItem.Name,
                                PosX = tempPosX,
                                PosY = tempPosY,
                                Height = (int)listItem.Height,
                                Width = (int)listItem.Width
                            };

                            db.tbl_textboxes.InsertOnSubmit(textbox);

                            db.SubmitChanges();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in tbList)
                {
                    double tempPosX, tempPosY;
                    if (Canvas.GetLeft(item) <= 0)
                    {
                        tempPosX = 0;
                    }
                    else
                    {
                        tempPosX = Canvas.GetLeft(item);
                    }
                    if (Canvas.GetTop(item) <= 0)
                    {
                        tempPosY = 0;
                    }
                    else
                    {
                        tempPosY = Canvas.GetTop(item);
                    }

                    tbl_textbox textbox = new tbl_textbox
                    {
                        LabelID = selectLabel.Id,
                        Name = item.Name,
                        PosX = tempPosX,
                        PosY = tempPosY,
                        Height = (int)item.Height,
                        Width = (int)item.Width
                    };

                    db.tbl_textboxes.InsertOnSubmit(textbox);
                    db.SubmitChanges();
                }
            }
        }

        private void SelectBarcode()
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var selectLabel = (from q in db.tbl_labels
                               where q.Name == tbLabelName.Text
                               select q).SingleOrDefault();

            var selectBarcode = from sb in db.tbl_barcodes
                                where sb.LabelID == selectLabel.Id
                                select sb;



            if (selectBarcode.Any())
            {
                foreach (var listItem in barcodeList)
                {
                    foreach (var bc in selectBarcode)
                    {
                        var barcodes = (from b in selectBarcode
                                        where listItem.Name == b.Name
                                        select b).SingleOrDefault();

                        if (barcodes != null)
                        {
                            barcodes.PosX = Canvas.GetLeft(listItem);
                            barcodes.PosY = Canvas.GetTop(listItem);
                            barcodes.Height = (int)listItem.Height;
                            barcodes.Width = (int)listItem.Width;

                            db.SubmitChanges();
                        }
                        else
                        {
                            double tempPosX, tempPosY;
                            if (Canvas.GetLeft(listItem) <= 0)
                            {
                                tempPosX = 0;
                            }
                            else
                            {
                                tempPosX = Canvas.GetLeft(listItem);
                            }
                            if (Canvas.GetTop(listItem) <= 0)
                            {
                                tempPosY = 0;
                            }
                            else
                            {
                                tempPosY = Canvas.GetTop(listItem);
                            }

                            tbl_barcode barcode = new tbl_barcode
                            {
                                LabelID = selectLabel.Id,
                                Name = listItem.Name,
                                PosX = tempPosX,
                                PosY = tempPosY,
                                Height = (int)listItem.Height,
                                Width = (int)listItem.Width
                            };

                            db.tbl_barcodes.InsertOnSubmit(barcode);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in barcodeList)
                {
                    double tempPosX, tempPosY;
                    if (Canvas.GetLeft(item) <= 0)
                    {
                        tempPosX = 0;
                    }
                    else
                    {
                        tempPosX = Canvas.GetLeft(item);
                    }
                    if (Canvas.GetTop(item) <= 0)
                    {
                        tempPosY = 0;
                    }
                    else
                    {
                        tempPosY = Canvas.GetTop(item);
                    }

                    tbl_barcode barcode = new tbl_barcode
                    {
                        LabelID = selectLabel.Id,
                        Name = item.Name,
                        PosX = tempPosX,
                        PosY = tempPosY,
                        Height = (int)item.Height,
                        Width = (int)item.Width
                    };

                    db.tbl_barcodes.InsertOnSubmit(barcode);
                    db.SubmitChanges();
                }
            }
        }
    }
}