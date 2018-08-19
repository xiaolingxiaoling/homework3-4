using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic_Calculator
{
    public partial class calculator : Form
    {
        public calculator()
        {
            InitializeComponent();
            subtract_btn.Text = "\u2212";
        }

        // Calculator fields //
        
        // The value currently shown on the calc_output label
        private string currentDisplayedInput = "";

        // Enumeration type for preventing user error
        enum inputTypes { Operator, Number, LeftParen, RightParen, empty };
        // Prevents consecutive Operators and decimals 
        private inputTypes lastInput = inputTypes.empty;
        // Used to prevent error with parenthesis
        private int LeftParenCount = 0;



        // Input token parser
        private void input_Parsing(String userInput) {
            var delimiters = new[] { "(", ")", "+", "\u2212", "/", "*", "^", "-" };
            ArrayList tokenizedInput = new ArrayList();
            String tempToken = "";
            double number;
            foreach (Char c in userInput) {
                if (delimiters.Contains(c.ToString())) {
                    //Add functionality so that numbers represented like 2.3E+13 are not missparsed
                    if (tempToken.Length > 0 && tempToken[tempToken.Length -1] == 'E') {
                        tempToken += c;
                        continue;
                    }
                    if (tempToken.Length > 0) {
                        // This if statement makes sure the token is a number before adding it to the list
                        if (Double.TryParse(tempToken, out number) != true) { incorrect_input(); return; }
                        tokenizedInput.Add(tempToken);
                        tempToken = "";
                    }
                    tokenizedInput.Add(c.ToString());
                } else {
                    tempToken += c;
                }
            }
            if (tempToken.Length > 0) {
                // This if statement makes sure the token is a number before adding it to the list
                if (Double.TryParse(tempToken, out number) != true) { incorrect_input(); return; }
                tokenizedInput.Add(tempToken);
            }
            
            try
            {
                calc_output.Text = calculate_Value(tokenizedInput);
            }
            catch (Exception InvalidOperationException)
            {
                calc_output.Text = "Invalid Input";
            }

            return;
        }

        private String calculate_Value(ArrayList tokenizedInput)
        {
            var operator_symbols = new[] { "+", "\u2212", "/", "*", "^" };
            // Set up two stacks, one for operators like +-/, one for operands like numbers
            Stack<String> operators = new Stack<String> { };
            Stack<String> operands = new Stack<String> { };
            foreach(String s in tokenizedInput) {
                // If the token is an operator do the following checks
                if (operator_symbols.Contains(s))
                {
                    // If the operator is the first one or has a larger precedence than the current operator, add it to the operator stack
                    if (operators.Count == 0 || check_Precedence(operators.Peek()) < check_Precedence(s))
                    {
                        operators.Push(s);
                    }
                    // If the operator has an equal precedence or a smaller precedence then calculate the last value and add it to the operands stack
                    else if (check_Precedence(operators.Peek()) == check_Precedence(s) || check_Precedence(operators.Peek()) > check_Precedence(s))
                    {
                        operands.Push(simple_Calculate(operands.Pop(), operands.Pop(), operators.Pop()));
                        operators.Push(s);
                    }
                }
                // If the token is a left paren or minus, add it to the stack, used as a sentinel 
                else if (s == "(" || s == "-")
                {
                    operators.Push(s);
                }
                // If the token is a right paren, start evaluating the stacks until you hit a left paren.
                else if (s == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        // If the operator is a negative sign, negate the top operand value
                        if(operators.Peek() == "-")
                        {
                            operands.Push((Double.Parse(operands.Pop()) * -1).ToString());
                            operators.Pop();
                            continue;
                        }
                        operands.Push(simple_Calculate(operands.Pop(), operands.Pop(), operators.Pop()));
                    }
                    // Remove left paren
                    operators.Pop();
                }
                // If the token is not an operator or paren, then it is an operand and gets put on the operand stack
                else {
                    operands.Push(s);
                }
            }
            // Once the tokens have all been parsed, finish calculating the stacks and return the final value in the operand stack
            while (operators.Count != 0)
            {
                // If the operator is a negative sign, negate the top operand value
                if (operators.Peek() == "-")
                {
                    operands.Push((Double.Parse(operands.Pop()) * -1).ToString());
                    operators.Pop();
                    continue;
                }
                operands.Push(simple_Calculate(operands.Pop(), operands.Pop(), operators.Pop()));
            }
            return operands.Pop();
        }

        // Returns the precedence of the operator
        private int check_Precedence(String op) {
            switch (op)
            {
                case "-":
                    return -1;
                case "(":
                    return 0;
                case ")":
                    return 0;
                case "^":
                    return 3;
                case "*":
                    return 2;
                case "/":
                    return 2;
                case "+":
                    return 1;
                case "\u2212":
                    return 1;
            }
            // If the operator was not found return -1
            return -1;
        }

        // Returns the calculation of two numbers and an operator
        private String simple_Calculate(string val2, string val1, string op) {
            double number1 = Double.Parse(val1);
            double number2 = Double.Parse(val2);
            switch (op)
            {
                case "^":
                    return Math.Pow(number1,number2).ToString();
                case "*":
                    return (number1*number2).ToString();
                case "/":
                    return (number1/number2).ToString();
                case "+":
                    return (number1+number2).ToString();
                case "\u2212":
                    return (number1-number2).ToString();
            }
            return "dff";
        }

        private void incorrect_input() {
            calc_output.Text = "Invalid Input";
        }

        private void correct_input(ArrayList a) {
            calc_output.Text = a.ToString();
        }

        // This function will display the currentDisplayedInput field to the calc_output label
        private void update_calc_output_label()
        {
            calc_output.Text = currentDisplayedInput;
        }

        // Click event handlers for calculator buttons //

        // Click event handler for the 0 button
        private void num0_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "0";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 1 button
        private void num1_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "1";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 2 button
        private void num2_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "2";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 3 button
        private void num3_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "3";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 4 button
        private void num4_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "4";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 5 button
        private void num5_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "5";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 6 button
        private void num6_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "6";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 7 button
        private void num7_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "7";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 8 button
        private void num8_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "8";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the 9 button
        private void num9_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + "9";
            update_calc_output_label();
            lastInput = inputTypes.Number;
        }

        // Click event handler for the decimal button
        private void decimal_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = currentDisplayedInput + ".";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the equals/enter button
        private void equals_btn_Click(object sender, EventArgs e) {
            if(currentDisplayedInput == "") { return; }
            input_Parsing(currentDisplayedInput);
            // If the displayed value is a number, keep it as the current input
            if (calc_output.Text == "Invalid Input" || calc_output.Text == "Infinity") {
                currentDisplayedInput = "";
                lastInput = inputTypes.empty;
                }
            else {
                currentDisplayedInput = calc_output.Text;
                lastInput = inputTypes.Number;
            }
        }

        // Click event handler for the additon button
        private void add_btn_Click(object sender, EventArgs e) {
            // Checks to prevent user error
            if (lastInput == inputTypes.Operator || lastInput == inputTypes.LeftParen || lastInput == inputTypes.empty) { return; }
            currentDisplayedInput = currentDisplayedInput + "+";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the subtract button
        private void subtract_btn_Click(object sender, EventArgs e) {
            // Checks to prevent user error
            if (lastInput == inputTypes.Operator || lastInput == inputTypes.LeftParen || lastInput == inputTypes.empty) { return; }
            currentDisplayedInput = currentDisplayedInput + "\u2212";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the multiplication button
        private void times_btn_Click(object sender, EventArgs e) {
            // Checks to prevent user error
            if (lastInput == inputTypes.Operator || lastInput == inputTypes.LeftParen || lastInput == inputTypes.empty) { return; }
            currentDisplayedInput = currentDisplayedInput + "*";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the division button
        private void division_btn_Click(object sender, EventArgs e) {
            // Checks to prevent user error
            if (lastInput == inputTypes.Operator || lastInput == inputTypes.LeftParen || lastInput == inputTypes.empty) { return; }
            currentDisplayedInput = currentDisplayedInput + "/";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the back button
        private void back_btn_Click(object sender, EventArgs e) {
            var delimiters = new[] { ".", "+", "\u2212", "/", "*", "^", "-" };
            if (lastInput == inputTypes.empty) { return; }
            // Remove the last input
            currentDisplayedInput = currentDisplayedInput.Remove(currentDisplayedInput.Length-1);
            update_calc_output_label();
            // If there is still an item in the currentDisplayedInput, set last input to its type, else set it to empty
            if (currentDisplayedInput != "") {
                String pastToken = currentDisplayedInput[currentDisplayedInput.Length - 1].ToString();
                if (delimiters.Contains(pastToken)) {
                    lastInput = inputTypes.Operator;
                } else if (pastToken == "(") {
                    lastInput = inputTypes.LeftParen;
                } else if (pastToken == ")") {
                    lastInput = inputTypes.RightParen;
                } else {
                    lastInput = inputTypes.Number;
                }
            } else {
                lastInput = inputTypes.empty;
            }
        }

        // Click event handler for the change sign button
        private void change_sign_btn_Click(object sender, EventArgs e) {
            // Checks to prevent user error
            if (lastInput == inputTypes.Number || lastInput == inputTypes.RightParen || currentDisplayedInput != "" && currentDisplayedInput[currentDisplayedInput.Length - 1] == '.') { return; }
            currentDisplayedInput = currentDisplayedInput + "-";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the clear button
        private void clear_btn_Click(object sender, EventArgs e) {
            currentDisplayedInput = "";
            update_calc_output_label();
            lastInput = inputTypes.empty;
        }

        // Click event handler for the power button
        private void power_bt_Click(object sender, EventArgs e) {
            // Checks to prevent user error
            if (lastInput == inputTypes.Operator || lastInput == inputTypes.LeftParen || lastInput == inputTypes.empty) { return; }
            currentDisplayedInput = currentDisplayedInput + "^";
            update_calc_output_label();
            lastInput = inputTypes.Operator;
        }

        // Click event handler for the left parenthesis button
        private void left_paren_Click(object sender, EventArgs e) {
            // Dont allow left parens 
            if (lastInput == inputTypes.Number || lastInput == inputTypes.RightParen) { return; }
            currentDisplayedInput = currentDisplayedInput + "(";
            update_calc_output_label();
            lastInput = inputTypes.LeftParen;
            // Increase paren count
            LeftParenCount++;
        }

        // Click event handler for the right parenthesis button
        private void right_paren_Click(object sender, EventArgs e) {
            // Dont allow right parens without any left parens
            if(LeftParenCount <= 0 || lastInput == inputTypes.LeftParen || lastInput == inputTypes.Operator) { return; }
            currentDisplayedInput = currentDisplayedInput + ")";
            update_calc_output_label();
            lastInput = inputTypes.RightParen;
            // Decrease paren count
            LeftParenCount--;
        }

    }
}
