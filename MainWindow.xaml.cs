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
    string mainNumber = "0";
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
        if (reset || mainNumber=="0")
        {   
            mainNumber = clickedButton.Content.ToString();
            reset = false;
        }
        else
        {
            mainNumber += clickedButton.Content.ToString();
        }
        MainText.Text = mainNumber;
    }

    // methode opérateur (récupere le signe passe reset à true et stock les valeurs) à revoir
    private void GetOperator(object sender, RoutedEventArgs e)
    {
        Button clickedButton = sender as Button;
        
        if (!reset)
        {   
            if (selectedOperator == null)
            {
                selectedOperator = clickedButton.Content.ToString();
                StockedText.Text = mainNumber + " " + selectedOperator + " ";
                MainText.Text = "0";
                stockedNumber = mainNumber;
                mainNumber = "0";
                reset = true;
            }
            else
            {
                // si il y a deja un operator selectionné on effectue le premier calcul 
            }
            
        }
        else
        {
            selectedOperator = clickedButton.Content.ToString();
            StockedText.Text = mainNumber + " " + selectedOperator + " ";
            MainText.Text = "0";
            stockedNumber = mainNumber;
            mainNumber = "0";
            reset = false;
        }
    }

    private void GetEquality(object sender, RoutedEventArgs e)
    {
        if (!reset && selectedOperator!=null && mainNumber!="") 
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
                    break;
                }
        }
    }
        
    private void DeleteLast(object sender, RoutedEventArgs e)
    {   
        if (reset)
        {
            DeleteAll(null, null);
        }
        else
        {
            if (mainNumber.Length > 1)
            {
                mainNumber = mainNumber.Remove(mainNumber.Length -1);
            }
            else if (mainNumber.Length == 1)
            {
                mainNumber = "0";
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
            mainNumber = "0";
            MainText.Text = "0";
        }
        
    }

    private void DeleteAll(object sender, RoutedEventArgs e)
    {
        mainNumber = "0";
        MainText.Text = "0";
        stockedNumber = null;
        StockedText.Text = "";
        reset = false;
        selectedOperator = null;
    }

    

    // methode de supression 1 DEL(dernier caractere)  1 C(tout)  1 CE(tout le main)

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
        return result;
    }

    public static double Subtraction(string numberString1, string numberString2)
    {   
        double result = TransformStringToDouble(numberString1) - TransformStringToDouble(numberString2);
        return result;
    }

    public static double Multiplication(string numberString1, string numberString2)
    {
        double result = TransformStringToDouble(numberString1) * TransformStringToDouble(numberString2);
        return result;
    }

    public static double Division(string numberString1, string numberString2)
    {   
        double d1 = TransformStringToDouble(numberString1);
        double d2 = TransformStringToDouble(numberString2);
        if (d2 == 0)
            throw new DivideByZeroException("Impossible de diviser par zéro...");
    
        return d1 / d2;     
    }

}