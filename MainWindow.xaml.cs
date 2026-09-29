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

    private void UpdateMainDisplay()
    {
        if(mainNumber == "")
        {
            MainText.Text = "_";
        }
        else
        {
            MainText.Text = mainNumber;
        }
    }

    /// <summary>
    /// Ajoute le chiffre du bouton cliqué à la saisie en cours (mainNumber),
    /// en remplaçant l'affichage si une nouvelle saisie doit démarrer.
    /// </summary>
    /// <param name="sender">Le bouton chiffre qui a été cliqué. </param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
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
        UpdateMainDisplay();
    }

    /// <summary>
    /// Ajoute la virgule décimale à la saisie en cours, ou démarre une nouvelle saisie avec "0,".
    /// Ignore le clic si une virgule est déjà présente.
    /// </summary>
    /// <param name="sender">Le bouton virgule qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
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
        UpdateMainDisplay();
    }

    /// <summary>
    /// Inverse le signe (positif/négatif) de la saisie en cours.
    /// </summary>
    /// <param name="sender">Le bouton +/- qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
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
            UpdateMainDisplay();
        }
    }

    /// <summary>
    /// Enregistre l'opérateur choisi (+, -, ×, ÷). Selon le contexte, stocke le nombre en cours,
    /// déclenche un calcul intermédiaire si un opérateur était déjà en attente, ou remplace
    /// l'opérateur précédent si aucun nouveau chiffre n'a été saisi entre-temps.
    /// </summary>
    /// <param name="sender">Le bouton opérateur qui a été cliqué.</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void GetOperator(object sender, RoutedEventArgs e)
    {
        Button clickedButton = sender as Button;

        if (selectedOperator == null && mainNumber!="")
        {
            selectedOperator = clickedButton.Content.ToString();
            StockedText.Text = mainNumber + " " + selectedOperator + " ";
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            mainNumber = "";
            reset = true;
        }

        else if (selectedOperator == null && mainNumber=="")
        {
            selectedOperator = clickedButton.Content.ToString();
            mainNumber = "0";
            StockedText.Text = mainNumber + " " + selectedOperator + " ";
            UpdateMainDisplay();
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
            UpdateMainDisplay();
            mainNumber = "";                
        }
        
        else if (selectedOperator != null && mainNumber=="")
        {   
            string oldOperator = selectedOperator;
            selectedOperator = clickedButton.Content.ToString();
            StockedText.Text = StockedText.Text.Replace(oldOperator +" ",selectedOperator +" ");
            UpdateMainDisplay();
            mainNumber = "";                
        }
    }

    /// <summary>
    /// Calcule et affiche le résultat final de l'opération en attente.
    /// </summary>
    /// <param name="sender">Le bouton = qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void GetEquality(object sender, RoutedEventArgs e)
    {
        if (!reset && mainNumber!="") 
        {
            StockedText.Text += mainNumber;
            DoCalculation();
            StockedText.Text += $" = ";
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;
        }
    }

    /// <summary>
    /// Élève la saisie en cours au carré. Si un opérateur est déjà en attente,
    /// enchaîne avec le calcul de cette opération.
    /// </summary>
    /// <param name="sender">Le bouton x² qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void GetSquare(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = mainNumber + "²" +" = ";
            mainNumber = Calculate.Multiplication(mainNumber, mainNumber).ToString();
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            reset = true;
        }
        else if (selectedOperator != null && mainNumber!="")
        {
            string result = Calculate.Multiplication(mainNumber, mainNumber).ToString();
            StockedText.Text += mainNumber + "²" + " = ";
            mainNumber = result;
            DoCalculation();
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    /// <summary>
    /// Calcule la racine carrée de la saisie en cours. Si un opérateur est déjà en attente,
    /// enchaîne avec le calcul de cette opération.
    /// </summary>
    /// <param name="sender">Le bouton √x qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void GetSquareRoot(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = "²√" + mainNumber +" = ";
            mainNumber = Calculate.SquareRoot(mainNumber).ToString();
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            reset = true;
        }
        else if (selectedOperator != null && mainNumber!="")
        {
            string result = Calculate.SquareRoot(mainNumber).ToString();
            StockedText.Text += "²√" + mainNumber + " = ";
            mainNumber = result;
            DoCalculation();
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    /// <summary>
    /// Calcule l'inverse (1/x) de la saisie en cours. Si un opérateur est déjà en attente,
    /// enchaîne avec le calcul de cette opération. Affiche un message d'erreur si la saisie vaut 0.
    /// </summary>
    /// <param name="sender">Le bouton 1/x qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void GetOneDivided(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = "1/" + mainNumber +" = ";
            try
            {
                mainNumber = Calculate.Division("1", mainNumber).ToString();
                UpdateMainDisplay();
                stockedNumber = mainNumber;
            }
            catch (DivideByZeroException ex)
            {
                StockedText.Text = ex.Message;
                mainNumber = "";
                UpdateMainDisplay();
            }
            reset = true;
        }
        else if (selectedOperator != null && mainNumber!="")
        {
            StockedText.Text += "1/" + mainNumber + " = ";
            try
            {
                mainNumber = Calculate.Division("1", mainNumber).ToString();
                DoCalculation();
                UpdateMainDisplay();
                stockedNumber = mainNumber;
            }
            catch (DivideByZeroException ex)
            {
                StockedText.Text = ex.Message;
                mainNumber = "";
                UpdateMainDisplay();
            }
            selectedOperator = null;
            reset = true;              
        }
    }

    /// <summary>
    /// Convertit la saisie en cours en pourcentage. Le comportement varie selon l'opérateur en
    /// attente : avec + ou -, le pourcentage est calculé par rapport au nombre stocké ; avec
    /// × ou ÷, il est appliqué directement à la saisie.
    /// </summary>
    /// <param name="sender">Le bouton % qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void GetPercent(object sender, RoutedEventArgs e)
    {
        if (selectedOperator == null && mainNumber!="")
        {   
            StockedText.Text = mainNumber + "%" + " = ";
            mainNumber = Calculate.Division(mainNumber, "100").ToString();
            UpdateMainDisplay();
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
            UpdateMainDisplay();
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
            UpdateMainDisplay();
            stockedNumber = mainNumber;
            selectedOperator = null;
            reset = true;              
        }
    }

    /// <summary>
    /// Effectue le calcul entre le nombre stocké et la saisie en cours, selon l'opérateur
    /// sélectionné, et met à jour mainNumber avec le résultat. En cas de division par zéro,
    /// affiche le message d'erreur et vide mainNumber plutôt que de laisser l'exception se propager.
    /// </summary>
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
                    StockedText.Text = ex.Message;
                    mainNumber = "";
                    break;
                }
        }
    }
        
    /// <summary>
    /// Supprime le dernier caractère saisi dans le nombre en cours.
    /// N'a aucun effet si la saisie doit être réinitialisée (reset actif).
    /// </summary>
    /// <param name="sender">Le bouton DEL qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
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
            UpdateMainDisplay();
        }
    }

    /// <summary>
    /// Efface la saisie en cours. Si un résultat vient d'être affiché (reset actif),
    /// efface en réalité tout le calcul (équivalent à DeleteAll).
    /// </summary>
    /// <param name="sender">Le bouton CE qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void DeleteMain(object sender, RoutedEventArgs e)
    {
        if (reset)
        {
            DeleteAll(null, null);
        }
        else
        {
            mainNumber = "";
            UpdateMainDisplay();
        }
    }

    /// <summary>
    /// Réinitialise entièrement la calculatrice : efface la saisie en cours, le nombre stocké,
    /// l'opérateur sélectionné et l'affichage de l'expression.
    /// </summary>
    /// <param name="sender">Le bouton C qui a été cliqué (non utilisé ici).</param>
    /// <param name="e">Données de l'événement de clic (non utilisées ici).</param>
    private void DeleteAll(object sender, RoutedEventArgs e)
    {
        mainNumber = "";
        UpdateMainDisplay();
        stockedNumber = null;
        StockedText.Text = "";
        reset = false;
        selectedOperator = null;
    }
}



/// <summary>
/// Fournit les opérations arithmétiques de base utilisées par la calculatrice,
/// à partir de valeurs saisies sous forme de texte.
/// </summary>
class Calculate
{
    /// <summary>
    /// Convertit une chaîne de texte représentant un nombre en valeur numérique double.
    /// </summary>
    /// <param name="numberString">Le nombre à convertir, sous forme de texte.</param>
    /// <returns>La valeur numérique correspondante.</returns>
    private static double TransformStringToDouble(string numberString)
    {
        double numberDouble = double.Parse(numberString);
        return numberDouble;
    }

    /// <summary>
    /// Additionne deux nombres fournis sous forme de texte.
    /// </summary>
    /// <param name="numberString1">Le premier nombre.</param>
    /// <param name="numberString2">Le second nombre.</param>
    /// <returns>La somme des deux nombres, arrondie à 6 décimales.</returns>
    public static double Addition(string numberString1, string numberString2)
    {
        double result = TransformStringToDouble(numberString1) + TransformStringToDouble(numberString2);
        return Math.Round(result, 6);
    }

    /// <summary>
    /// Soustrait le second nombre au premier, tous deux fournis sous forme de texte.
    /// </summary>
    /// <param name="numberString1">Le nombre duquel on soustrait.</param>
    /// <param name="numberString2">Le nombre à soustraire.</param>
    /// <returns>Le résultat de la soustraction, arrondi à 6 décimales.</returns>
    public static double Subtraction(string numberString1, string numberString2)
    {   
        double result = TransformStringToDouble(numberString1) - TransformStringToDouble(numberString2);
        return Math.Round(result, 6);
    }

    /// <summary>
    /// Multiplie deux nombres fournis sous forme de texte.
    /// </summary>
    /// <param name="numberString1">Le premier nombre.</param>
    /// <param name="numberString2">Le second nombre.</param>
    /// <returns>Le produit des deux nombres, arrondi à 6 décimales.</returns>
    public static double Multiplication(string numberString1, string numberString2)
    {
        double result = TransformStringToDouble(numberString1) * TransformStringToDouble(numberString2);
        return Math.Round(result, 6);
    }

    /// <summary>
    /// Divise le premier nombre par le second, tous deux fournis sous forme de texte.
    /// </summary>
    /// <param name="numberString1">Le dividende.</param>
    /// <param name="numberString2">Le diviseur.</param>
    /// <returns>Le résultat de la division, arrondi à 6 décimales.</returns>
    /// <exception cref="DivideByZeroException">Levée si le diviseur vaut zéro.</exception>

    public static double Division(string numberString1, string numberString2)
    {   
        double d1 = TransformStringToDouble(numberString1);
        double d2 = TransformStringToDouble(numberString2);
        if (d2 == 0)
            throw new DivideByZeroException("Impossible de diviser par zéro...");
        
        double result = d1 / d2;
        return Math.Round(result, 6);  
    }
    
    /// <summary>
    /// Calcule la racine carrée d'un nombre fourni sous forme de texte.
    /// </summary>
    /// <param name="numberString1">Le nombre dont on calcule la racine carrée.</param>
    /// <returns>La racine carrée, arrondie à 6 décimales.</returns>
    public static double SquareRoot(string numberString1)
    {
        double result = Math.Sqrt(TransformStringToDouble(numberString1));
        return Math.Round(result, 6);
    }
}