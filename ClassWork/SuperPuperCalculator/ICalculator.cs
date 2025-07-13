namespace SuperPuperCalculator;

public interface ICalculator<TOperand>
{
    TOperand Add(TOperand a, TOperand b);
    TOperand Subtract(TOperand a, TOperand b);
    TOperand Multiply(TOperand a, TOperand b);
    TOperand Divide(TOperand a, TOperand b);
}