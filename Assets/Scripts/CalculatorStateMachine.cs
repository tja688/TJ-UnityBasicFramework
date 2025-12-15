using UnityEngine;
using System;
using System.Data; // 用于模拟 eval()
using System.Globalization;

// 保存信息中要求：默认返回完整、无任何省略、带简要注释的代码，注释应为中文。

public class CalculatorStateMachine : MonoBehaviour
{
    #region 数据模型 (Data Model)
    // 对应 SCXML 中的 datamodel
    [SerializeField] private string long_expr = "";
    [SerializeField] private string short_expr = "";
    [SerializeField] private double res = 0;
    #endregion

    #region 状态定义 (State Definition)
    // 将 SCXML 的层级状态扁平化为枚举，以便在 MonoBehaviour 中管理
    public enum State
    {
        Wrapper_On_Ready_Begin,     // 初始状态
        Operand1_Int1,              // 输入第一个操作数的整数部分
        Operand1_Frac1,             // 输入第一个操作数的小数部分
        Operand1_Zero1,             // 第一个操作数为0的情况
        Negated1,                   // 第一个操作数取反
        OpEntered,                  // 输入了运算符
        Operand2_Int2,              // 输入第二个操作数的整数部分
        Operand2_Frac2,             // 输入第二个操作数的小数部分
        Operand2_Zero2,             // 第二个操作数为0的情况
        Negated2,                   // 第二个操作数取反
        Result                      // 显示结果状态
    }

    [Header("Debug")]
    public State currentState = State.Wrapper_On_Ready_Begin;
    public string displayOutput = "0";
    #endregion

    #region Unity 生命周期
    void Start()
    {
        // 对应 state id="begin" 的 onentry
        EnterBeginState();
    }

    void Update()
    {
        // 简单的键盘输入模拟，方便测试
        HandleKeyboardInput();
    }
    
    // 用于在 Inspector 显示当前计算器屏幕
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 100));
        GUILayout.Box($"Display: {displayOutput}");
        GUILayout.Label($"State: {currentState}");
        GUILayout.Label($"Long Expr: {long_expr}");
        GUILayout.Label($"Short Expr: {short_expr}");
        GUILayout.Label($"Res: {res}");
        GUILayout.EndArea();
    }
    #endregion

    #region 内部转换逻辑 (Internal Transitions)
    // 对应 SCXML wrapper state 中的 transition event="CALC.DO"
    private void Raise_CALC_DO()
    {
        Debug.Log("[Event] CALC.DO");
        short_expr = "" + res;
        long_expr = "";
        res = 0;
    }

    // 对应 SCXML wrapper state 中的 transition event="CALC.SUB"
    private void Raise_CALC_SUB()
    {
        Debug.Log("[Event] CALC.SUB");
        if (short_expr != "")
        {
            // SCXML 逻辑：把当前短表达式拼接到长表达式后面，并加括号
            // 注意：为了让 DataTable.Compute 能计算，我们需要确保格式正确
            // SCXML 原文: long_expr+'('+short_expr+')'
            long_expr = long_expr + "(" + short_expr + ")";
        }

        // 模拟 eval(long_expr)
        res = EvaluateExpression(long_expr);
        short_expr = "";
        
        Raise_DISPLAY_UPDATE();
    }

    // 对应 SCXML wrapper state 中的 transition event="DISPLAY.UPDATE"
    private void Raise_DISPLAY_UPDATE()
    {
        string val = (short_expr == "") ? res.ToString() : short_expr;
        Debug.Log($"[Display Update] result: {val}");
        
        // 发送事件 send event="updateDisplay"
        displayOutput = val;
    }

    // 对应 SCXML wrapper state 中的 transition event="OP.INSERT"
    private void Raise_OP_INSERT(string operatorName)
    {
        Debug.Log($"[Event] OP.INSERT: {operatorName}");
        
        if (operatorName == "OPER.PLUS") long_expr += "+";
        else if (operatorName == "OPER.MINUS") long_expr += "-";
        else if (operatorName == "OPER.STAR") long_expr += "*";
        else if (operatorName == "OPER.DIV") long_expr += "/";
    }
    #endregion

    #region 状态机逻辑 (State Machine Logic)

    // 重置逻辑，对应 state id="begin" onentry
    private void EnterBeginState()
    {
        long_expr = "";
        short_expr = "0";
        res = 0;
        currentState = State.Wrapper_On_Ready_Begin;
        Raise_DISPLAY_UPDATE();
    }

    // 处理数字输入 (DIGIT)
    public void InputDigit(int digit)
    {
        string digitStr = digit.ToString();
        string eventName = "DIGIT." + digitStr;

        switch (currentState)
        {
            case State.Wrapper_On_Ready_Begin:
            case State.Result: // SCXML中 ready 包含了 begin 和 result
                if (digit == 0)
                {
                    currentState = State.Operand1_Zero1;
                    short_expr = ""; // transition assign
                }
                else
                {
                    currentState = State.Operand1_Int1;
                    short_expr = ""; // transition assign
                    AppendToShortExpr(digitStr); // Int1 onEntry
                }
                break;

            case State.Operand1_Int1:
                AppendToShortExpr(digitStr);
                break;

            case State.Operand1_Frac1:
                AppendToShortExpr(digitStr);
                break;

            case State.Operand1_Zero1:
                if (digit != 0)
                {
                    currentState = State.Operand1_Int1;
                    AppendToShortExpr(digitStr);
                }
                // 如果是 0，保持在 Zero1
                break;
            
            case State.Negated1:
                 if (digit == 0) currentState = State.Operand1_Zero1; // 简化处理，逻辑同 int1/zero1 转换
                 else { currentState = State.Operand1_Int1; AppendToShortExpr(digitStr); }
                 break;

            case State.OpEntered:
                if (digit == 0) currentState = State.Operand2_Zero2;
                else 
                { 
                    currentState = State.Operand2_Int2; 
                    AppendToShortExpr(digitStr); // Int2 onEntry logic (simplified)
                }
                break;

            case State.Negated2:
                if (digit == 0) currentState = State.Operand2_Zero2;
                else { currentState = State.Operand2_Int2; AppendToShortExpr(digitStr); }
                break;

            case State.Operand2_Int2:
                AppendToShortExpr(digitStr);
                break;

            case State.Operand2_Frac2:
                AppendToShortExpr(digitStr);
                break;

            case State.Operand2_Zero2:
                if (digit != 0)
                {
                    currentState = State.Operand2_Int2;
                    AppendToShortExpr(digitStr);
                }
                break;
        }
    }

    // 处理小数点 (POINT)
    public void InputPoint()
    {
        switch (currentState)
        {
            case State.Wrapper_On_Ready_Begin:
            case State.Result:
                short_expr = ""; 
                currentState = State.Operand1_Frac1;
                AppendToShortExpr("."); // Frac1 OnEntry
                break;

            case State.Operand1_Int1:
            case State.Operand1_Zero1:
            case State.Negated1:
                currentState = State.Operand1_Frac1;
                AppendToShortExpr(".");
                break;

            case State.OpEntered:
            case State.Negated2:
                currentState = State.Operand2_Frac2;
                AppendToShortExpr(".");
                break;

            case State.Operand2_Int2:
            case State.Operand2_Zero2:
                currentState = State.Operand2_Frac2;
                AppendToShortExpr(".");
                break;
        }
    }

    // 处理操作符 (OPER)
    public void InputOper(string operType) // OPER.PLUS, OPER.MINUS etc.
    {
        // 特殊处理：Wait/Begin 状态下的 OPER.MINUS -> Negated1
        if (currentState == State.Wrapper_On_Ready_Begin && operType == "OPER.MINUS")
        {
            currentState = State.Negated1;
            short_expr = "-"; // Negated1 OnEntry
            Raise_DISPLAY_UPDATE();
            return;
        }

        // OpEntered 下的 OPER.MINUS -> Negated2
        if (currentState == State.OpEntered && operType == "OPER.MINUS")
        {
            currentState = State.Negated2;
            short_expr = "-"; // Negated2 OnEntry
            Raise_DISPLAY_UPDATE();
            return;
        }

        // 状态 Operand1 (Int, Frac, Zero) -> OpEntered
        if (IsOperand1State(currentState))
        {
            TransitionToOpEntered(operType);
            return;
        }

        // 状态 Operand2 (Int, Frac, Zero) -> OpEntered (链式计算)
        if (IsOperand2State(currentState))
        {
            // raise CALC.SUB
            Raise_CALC_SUB();
            // raise OP.INSERT
            Raise_OP_INSERT(operType); // 此时已经是在 OpEntered 的逻辑里了
            
            currentState = State.OpEntered;
            // OpEntered OnEntry
            // SCXML: raise CALC.SUB (Operand2 transition did it), send OP.INSERT (done above)
            return;
        }
        
        // 如果已经在 Ready 状态且按下了操作符（比如用上次结果继续计算）
        if (currentState == State.Result || currentState == State.Wrapper_On_Ready_Begin)
        {
             TransitionToOpEntered(operType);
        }
    }

    // 处理等号 (EQUALS)
    public void InputEquals()
    {
        if (IsOperand2State(currentState))
        {
            currentState = State.Result;
            // target="result" -> raise CALC.SUB -> raise CALC.DO
            Raise_CALC_SUB();
            Raise_CALC_DO();
        }
    }

    // 处理清除 (C)
    public void InputClear()
    {
        // transition event="C" target="on" (重置到 on 的初始状态 ready -> begin)
        EnterBeginState();
    }

    #endregion

    #region 辅助函数 (Helpers)

    private void AppendToShortExpr(string str)
    {
        // 对应 int1/frac1 等状态的 OnEntry 或 internal transition
        // 处理小数点逻辑： substr(_event.name.lastIndexOf('.')+1) 在 SCXML 里是提取输入
        // 这里简化为直接拼接
        short_expr += str;
        Raise_DISPLAY_UPDATE();
    }

    private void TransitionToOpEntered(string operType)
    {
        currentState = State.OpEntered;
        // OpEntered OnEntry: raise CALC.SUB, send OP.INSERT
        Raise_CALC_SUB();
        Raise_OP_INSERT(operType);
    }

    private bool IsOperand1State(State s)
    {
        return s == State.Operand1_Int1 || s == State.Operand1_Frac1 || s == State.Operand1_Zero1;
    }

    private bool IsOperand2State(State s)
    {
        return s == State.Operand2_Int2 || s == State.Operand2_Frac2 || s == State.Operand2_Zero2;
    }

    // 模拟 Eval() 函数
    private double EvaluateExpression(string expression)
    {
        if (string.IsNullOrEmpty(expression)) return 0;

        try
        {
            // 使用 DataTable 进行计算 (System.Data)
            DataTable table = new DataTable();
            // 需要处理一下空括号或者不完整表达式的问题，防止报错
            string safeExpr = expression; 
            // 简单的 try-eval
            var result = table.Compute(safeExpr, string.Empty);
            return Convert.ToDouble(result);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Eval failed for '{expression}': {e.Message}");
            return res; // 计算失败返回上一次结果
        }
    }

    // 键盘监听
    private void HandleKeyboardInput()
    {
        // 数字
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString())) InputDigit(i);
        }

        // 小数点
        if (Input.GetKeyDown(KeyCode.Period)) InputPoint();

        // 运算符
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) InputOper("OPER.PLUS");
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) InputOper("OPER.MINUS");
        if (Input.GetKeyDown(KeyCode.Asterisk) || Input.GetKeyDown(KeyCode.KeypadMultiply)) InputOper("OPER.STAR");
        if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.KeypadDivide)) InputOper("OPER.DIV");

        // 等号 (Enter 或 =)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Equals)) InputEquals();

        // 清除 (C 或 Escape)
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape)) InputClear();
    }

    #endregion
}