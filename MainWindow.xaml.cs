using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSharp_Calculator_GUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{   
    string mainNumber = "";
    string? stockedNumber = null;
    string? selectedOperator = null;
    bool reset = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void GetNumber(object sender, RoutedEventArgs e)
    {
        Button clickedButton = sender as Button;
        if (reset || mainNumber=="")
        {   
            mainNumber = clickedButton.Content.ToString();
            reset = false;
            if (StockedText.Text.Contains("="))
            {
                stockedNumber = null;
                StockedText.Text = "";
            }
        }
        else
        {
            mainNumber += clickedButton.Content.ToString();
        }
        MainText.Text = mainNumber;
    }

    private void GetDecimal(object sender, RoutedEventArgs e)
    {
        if (reset || mainNumber=="")
        {   
            mainNumber = "0,";
            reset = false;
            if (StockedText.Text.Contains("="))
            {
                stockedNumber = null;
                StockedText.Text = "";
            }
        }
        else if(mainNumber.Contains(","))
        {
            return;
        }
        else
        {
            mainNumber += ",";
        }
        MainText.Text = mainNumber;
    }

    private void GetReverse(object sender, RoutedEventArgs e)
    {
        if (mainNumber != "")
        {
            if (mainNumber.Contains("-"))
            {
                mainNumber = mainNumber.Replace("-","");
            }
            else
            {
                mainNumber = "-" + mainNumber;
            }
            MainText.Text = mainNumber;
        }
    }

    private void GetOperator(object sender, RoutedEventArgs e)
    {
        Button clickedButton = sender as Button;

        if (selectedOperator == null && mainNumber!="")
        {
            selectedOperator = clickedButton.Content.ToString();
            StockedText.Text = mainNumber + " " + selectedOperator + " ";
            MainText.Text = "_";
            stockedNumber = mainNumber;
            mainNumber = "";
            reset = true;
        }

        else if (selectedOperator == null && mainNumber=="")
        {
            selectedOperator = clickedButton.Content.ToString();
            mainNumber = "0";
            StockedText.Text = mainNumber + " " + selectedOperator + " ";
            MainText.Text = "_";
            stockedNumber = mainNumber;
            mainNumber = "";
            reset = true;
        }

        else if (selectedOperator != null && mainNumber!="")
        {
            DoCalculation();
            stockedNumber = mainNumber;
            StockedText.Text = mainNumber;
            selectedOperator = clickedButton.Content.ToString();
            StockedText.Text += " " + selectedOperator + " ";
            MainText.Text = "_";
            mainNumber = "";                
        }
        
        else if (selectedOperator != null && mainNumber=="")
        {   
            string oldOperator = selectedOperator;
            selectedOperator = clickedButton.Content.ToString();
            StockedText.Text = StockedText.Text.Replace(oldOperator +" ",selectedOperator +" ");
            MainText.Text = "_";
            mainNumber = "";                
        }
    }

    private void GetEquality(object sender, RoutedEventArgs e)
    {
        if (!reset && mainNumber!="") 
        {
            StockedText.Text += mainNumber;
            DoCalculation();
            StockedText.Text += $" = ";
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;
        }
        
        
    }

    private void GetSquare(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = mainNumber + "²" +" = ";
            mainNumber = Calculate.Multiplication(mainNumber, mainNumber).ToString();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            reset = true;
        }
        else if (selectedOperator != null && mainNumber!="")
        {
            string result = Calculate.Multiplication(mainNumber, mainNumber).ToString();
            StockedText.Text += mainNumber + "²" + " = ";
            mainNumber = result;
            DoCalculation();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    private void GetSquareRoot(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = "²√" + mainNumber +" = ";
            mainNumber = Calculate.SquareRoot(mainNumber).ToString();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            reset = true;
        }
        else if (selectedOperator != null && mainNumber!="")
        {
            string result = Calculate.SquareRoot(mainNumber).ToString();
            StockedText.Text += "²√" + mainNumber + " = ";
            mainNumber = result;
            DoCalculation();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    private void GetOneDivided(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = "1/" + mainNumber +" = ";
            mainNumber = Calculate.Division("1", mainNumber).ToString();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            reset = true;
        }
        else if (selectedOperator != null && mainNumber!="")
        {
            string result = Calculate.Division("1", mainNumber).ToString();
            StockedText.Text += "1/" + mainNumber + " = ";
            mainNumber = result;
            DoCalculation();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    private void GetPercent(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = mainNumber + "%" + " = ";
            mainNumber = Calculate.Division(mainNumber, "100").ToString();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            reset = true;
        }
        else if ((selectedOperator == "+" || selectedOperator == "-") && mainNumber!="")
        {
            string resultP = Calculate.Division(mainNumber, "100").ToString();
            StockedText.Text += mainNumber + "%" + " = ";
            string result = Calculate.Multiplication(stockedNumber, resultP).ToString();
            mainNumber = result;
            DoCalculation();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
        else if ((selectedOperator == "/" || selectedOperator == "×") && mainNumber!="")
        {
            string result = Calculate.Division(mainNumber, "100").ToString();
            StockedText.Text += mainNumber + "%" + " = ";
            mainNumber = result;
            DoCalculation();
            MainText.Text = mainNumber;
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    private void DoCalculation()
    {
        switch (selectedOperator)
        {
            case "+":
                // methode addition
                {
                mainNumber = Calculate.Addition(stockedNumber, mainNumber).ToString();
                break;
                }
            case "-":
                // methode soustraction 
                {
                mainNumber = Calculate.Subtraction(stockedNumber, mainNumber).ToString();
                break;
                }
            case "×":
                // methode multiplication 
                {
                mainNumber = Calculate.Multiplication(stockedNumber, mainNumber).ToString();
                break;
                }
            case "÷":
                // methode division 
                try
                {
                    mainNumber = Calculate.Division(stockedNumber, mainNumber).ToString();
                    break;
                }
                catch (DivideByZeroException ex)
                {
                    // appel de la fonction pour tout supprimer
                    StockedText.Text = ex.Message;
                    mainNumber = "_";
                    break;
                }
        }
    }
        
    private void DeleteLast(object sender, RoutedEventArgs e)
    {   
        if (!reset)
        {
            if (mainNumber.Length > 1)
            {
                mainNumber = mainNumber.Remove(mainNumber.Length -1);
            }
            else if (mainNumber.Length == 1)
            {
                mainNumber = "";
            }
            MainText.Text = mainNumber;
        }
    }

    private void DeleteMain(object sender, RoutedEventArgs e)
    {
        if (reset)
        {
            DeleteAll(null, null);
        }
        else
        {
            mainNumber = "";
            MainText.Text = "_";
        }
    }

    private void DeleteAll(object sender, RoutedEventArgs e)
    {
        mainNumber = "";
        MainText.Text = "_";
        stockedNumber = null;
        StockedText.Text = "";
        reset = false;
        selectedOperator = null;
    }
}




class Calculate
{
    private static double TransformStringToDouble(string numberString)
    {
        double numberDouble = double.Parse(numberString);
        return numberDouble;
    }

    public static double Addition(string numberString1, string numberString2)
    {
        double result = TransformStringToDouble(numberString1) + TransformStringToDouble(numberString2);
        return Math.Round(result, 6);
    }

    public static double Subtraction(string numberString1, string numberString2)
    {   
        double result = TransformStringToDouble(numberString1) - TransformStringToDouble(numberString2);
        return Math.Round(result, 6);
    }

    public static double Multiplication(string numberString1, string numberString2)
    {
        double result = TransformStringToDouble(numberString1) * TransformStringToDouble(numberString2);
        return Math.Round(result, 6);
    }

    public static double Division(string numberString1, string numberString2)
    {   
        double d1 = TransformStringToDouble(numberString1);
        double d2 = TransformStringToDouble(numberString2);
        if (d2 == 0)
            throw new DivideByZeroException("Impossible de diviser par zéro...");
        
        double result = d1 / d2;
        return Math.Round(result, 6);  
    }

    public static double SquareRoot(string numberString1)
    {
        double result = Math.Sqrt(TransformStringToDouble(numberString1));
        return Math.Round(result, 6);
    }
}