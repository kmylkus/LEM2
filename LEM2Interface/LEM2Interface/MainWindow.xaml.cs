using Algorithm_LEM2;
using Microsoft.Win32;
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

namespace LEM2Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath;
        List<string> attr;
        List<Dictionary<string, string>> rows;
        List<string> testAttr;
        List<Dictionary<string, string>> testRows;
        List<Rule> rules;

        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = ".TAB";
            openFile.Filter = "TAB document (.TAB)|*.TAB";
            if (openFile.ShowDialog() == true)
            {
                tbItem_Classyfi_test.IsEnabled=tbItem_Classifier.IsEnabled = false;
                sp_ClassifierMain.Children.Clear();

                filePath = openFile.FileName;
                txt_FileName.Text = System.IO.Path.GetFileName(filePath);


                attr = new List<string>();
                rows = new List<Dictionary<string, string>>();

                string line, attrText = null;
                bool isFirstSquareDucky = false, isSecondSquareDucky = false;

                StreamReader file = new StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    if (!isFirstSquareDucky)
                    {
                        if (line.Contains("["))
                        {
                            isFirstSquareDucky = true;
                            int index = line.IndexOf("[");
                            line = line.Remove(index, 1);

                            if (line.Contains("]"))
                            {
                                isSecondSquareDucky = true;
                                index = line.IndexOf("]");
                                line.Remove(index - 1, 1);
                            }
                        }
                    }
                    if (isFirstSquareDucky && !isSecondSquareDucky)
                    {
                        if (line.Contains("]"))
                        {
                            isSecondSquareDucky = true;
                            int index = line.IndexOf("]");
                            line = line.Remove(index, 1);
                        }
                        attrText += line + " ";
                        if (isSecondSquareDucky)
                            AddAttr(attrText, attr);
                    }
                    if (isFirstSquareDucky && isSecondSquareDucky)
                    {
                        string[] items = line.Split(' ');
                        List<string> t = new List<string>();
                        foreach (var item in items)
                        {
                            if (item != "")
                                t.Add(item);
                        }
                        if (t.Count == attr.Count)
                        {
                            Dictionary<string, string> row = new Dictionary<string, string>();
                            for (int i = 0; i < attr.Count; i++)
                            {
                                row.Add(attr[i], t[i]);
                            }
                            rows.Add(row);
                        }
                    }
                }
                file.Close();

                btn_StartLem2.IsEnabled = true;
            }
        }


        //Helpers
        private void AddAttr(string text, List<string> myattr)
        {
            string[] items = text.Split(' ');
            foreach (var item in items)
            {
                if (item != null && item != "")
                    myattr.Add(item);
            }
        }



        private void btn_StartLem2_Click(object sender, RoutedEventArgs e)
        {
            DataSet x = new DataSet();
            x.Attributes = attr;
            x.Rows = rows;

            x.StartAlgorithmLEM2();
            rules = x.Rules;
            tbItem_Classyfi_test.IsEnabled=tbItem_Classifier.IsEnabled = true;

            text_ErrorCount.Text = $"The number of errors: {x.Wrong}";

            txt_Rules.Clear();
            txt_Rules.Text = x.GetRulesAsString() + "\n";

            if (x.tmpDeletedRules.Count > 0)
            {
                txt_Rules.Text += "Removed rules:\n";
                foreach (var item in x.tmpDeletedRules)
                {
                    txt_Rules.Text += item.ToString() + "\n";
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tbItem_Classifier.IsSelected && sp_ClassifierMain.Children.Count == 0)
            {
                for (int i = 0; i < attr.Count-1; i++)
                {
                    List<string> values = new List<string>();
                    foreach (var row in rows)
                    {
                        values.Add(row[attr[i]]);
                    }
                    TextBlock t = new TextBlock() { Text = attr[i] };
                    t.Name = $"txt_{i}";
                    sp_ClassifierMain.Children.Add(t);
                    ComboBox c = new ComboBox();
                    c.Name = $"cb_{i}";
                    c.ItemsSource = values.GroupBy(x => x).Select(x => x.Key);
                    c.SelectedIndex = 0;
                    sp_ClassifierMain.Children.Add(c);
                }
                Button b = new Button() { Content = "Classify!" };
                b.Margin = new Thickness(5);
                b.MinHeight = 35;
                b.MaxWidth = 200;
                b.Click += new RoutedEventHandler(Classify_Click);
                sp_ClassifierMain.Children.Add(b);

                TextBox rez = new TextBox();
                sp_ClassifierMain.Children.Add(rez);

                TextBlock rule = new TextBlock() { Name = "txt_Rule" };
                sp_ClassifierMain.Children.Add(rule);
            }
        }

        private void Classify_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();
            for (int i = 0; i < attr.Count - 1; i++)
            {
                string attr = null, value = null;
                for (int itr = 0; itr < sp_ClassifierMain.Children.Count - 3; itr++)
                {
                    if (sp_ClassifierMain.Children[itr] is TextBlock && ((TextBlock)sp_ClassifierMain.Children[itr]).Name.Equals($"txt_{i}"))
                    {
                        attr = ((TextBlock)sp_ClassifierMain.Children[itr]).Text;
                    }
                    else if (sp_ClassifierMain.Children[itr] is ComboBox && ((ComboBox)sp_ClassifierMain.Children[itr]).Name.Equals($"cb_{i}"))
                    {
                        value = ((ComboBox)sp_ClassifierMain.Children[itr]).SelectedValue.ToString();
                    }
                }
                obj.Add(attr, value);
            }

            Rule rule = null;
            foreach (var item in rules)
            {
                var combo = item.AttrAndValue.Except(obj);
                if (!combo.Any())
                {
                    rule = item;
                    break;
                }
            }

            var txt_Result = (TextBox)sp_ClassifierMain.Children[sp_ClassifierMain.Children.Count - 2];
            var txt_Rule = (TextBlock)sp_ClassifierMain.Children[sp_ClassifierMain.Children.Count - 1];
            if (rule == null)
            {
                txt_Result.Text = "The lack of a decision";
                txt_Rule.Text = "";
            }
            else
            {
                txt_Result.Text = rule.DecisionClass;
                txt_Rule.Text = $"Rule: {rule.ToString()}";
            }
        }

        private void btn_OpenTestFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openTestFile = new OpenFileDialog();
            openTestFile.DefaultExt = ".TAB";
            openTestFile.Filter = "TAB document (.TAB)|*.TAB";
            if (openTestFile.ShowDialog() == true)
            {

                filePath = openTestFile.FileName;
                txt_FileName.Text = System.IO.Path.GetFileName(filePath);

                testAttr = new List<string>();
                testRows = new List<Dictionary<string, string>>();

                string line, attrText = null;
                bool isFirstSquareDucky = false, isSecondSquareDucky = false;

                StreamReader file = new StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    if (!isFirstSquareDucky)
                    {
                        if (line.Contains("["))
                        {
                            isFirstSquareDucky = true;
                            int index = line.IndexOf("[");
                            line = line.Remove(index, 1);

                            if (line.Contains("]"))
                            {
                                isSecondSquareDucky = true;
                                index = line.IndexOf("]");
                                line.Remove(index - 1, 1);
                            }
                        }
                    }
                    if (isFirstSquareDucky && !isSecondSquareDucky)
                    {
                        if (line.Contains("]"))
                        {
                            isSecondSquareDucky = true;
                            int index = line.IndexOf("]");
                            line = line.Remove(index, 1);
                        }
                        attrText += line + " ";
                        if (isSecondSquareDucky)
                           AddAttr(attrText, testAttr);
                    }
                    if (isFirstSquareDucky && isSecondSquareDucky)
                    {
                        string[] items = line.Split(' ');
                        List<string> t = new List<string>();
                        foreach (var item in items)
                        {
                            if (item != "")
                                t.Add(item);
                        }
                        if (t.Count == testAttr.Count)
                        {
                            Dictionary<string, string> row = new Dictionary<string, string>();
                            for (int i = 0; i < testAttr.Count; i++)
                            {
                                row.Add(testAttr[i], t[i]);
                            }
                            testRows.Add(row);
                        }
                    }
                }
                file.Close();

                btn_StartLem2test.IsEnabled = true;
            }
        }

        private void btn_StartLem2Test_Click(object sender, RoutedEventArgs e)
        {

            Rule rule = null;
            int lackCounter = 0;
            int badCounter = 0;
            int correctCounter = 0;
            

            foreach (var obj in testRows)
            {
                foreach (var item in rules)
                {
                    var combo = item.AttrAndValue.Except(obj);
                    if (!combo.Any())
                    {
                        rule = item;
                        break;
                    }
                }

                int indx = testRows.IndexOf(obj)+1;
                if (rule==null)
                {
                    txt_testResult.AppendText(indx + ") The lack of a decision for:" + Environment.NewLine);
                    txt_testResult.AppendText(obj.ToString() + Environment.NewLine);
                    lackCounter++;

                }
                else
                {
                    string objctSting = string.Join("  ", obj.Select(x => x.Value).ToArray());
                    string attributeString= string.Join("  ", obj.Select(x => x.Key).ToArray());

                    if (rule.DecisionClass.Equals(obj.Last().Value))
                    {
                        txt_testResult.AppendText(indx + ") Correct rule." + Environment.NewLine);
                        txt_testResult.AppendText("Rule: " + rule.ToString() + Environment.NewLine);
                        txt_testResult.AppendText("Data Attributes: " + attributeString + Environment.NewLine);
                        txt_testResult.AppendText("Data Values:      " + objctSting + Environment.NewLine);
                        correctCounter++;
                    }
                    else{
                        txt_testResult.AppendText(indx + ") Wrong rule for: " + Environment.NewLine);
                        txt_testResult.AppendText("Data Attributes: " + attributeString + Environment.NewLine);
                        txt_testResult.AppendText("Data Values:      " + objctSting + Environment.NewLine);
                        badCounter++;
                    }
                }
                    
            }
            text_ResultCount.Text = $"Number of correct rule: {correctCounter}. Number of wrong rule: {badCounter}. Number Lack of decision: {lackCounter}.";

        }
    }
}
