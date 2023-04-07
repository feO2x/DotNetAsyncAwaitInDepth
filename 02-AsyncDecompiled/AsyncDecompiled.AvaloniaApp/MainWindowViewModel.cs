using System;
using System.Runtime.CompilerServices;
using Light.ViewModels;

namespace AsyncDecompiled.AvaloniaApp;

public sealed class MainWindowViewModel : BaseNotifyPropertyChanged
{
    private int _firstNumber = 2;
    private bool _isCalculating;
    private string _result = string.Empty;
    private int _secondNumber = 20;

    public MainWindowViewModel() => CalculateCommand = new (CalculateOnBackgroundThreadDecompiled, () => !IsCalculating);

    public int FirstNumber
    {
        get => _firstNumber;
        set => SetIfDifferent(ref _firstNumber, value);
    }

    public int SecondNumber
    {
        get => _secondNumber;
        set => SetIfDifferent(ref _secondNumber, value);
    }

    public bool IsCalculating
    {
        get => _isCalculating;
        private set
        {
            Set(out _isCalculating, value);
            CalculateCommand.RaiseCanExecuteChanged();
        }
    }

    public DelegateCommand CalculateCommand { get; }

    public string Result
    {
        get => _result;
        private set => Set(out _result, value);
    }

    private void CalculateSynchronously()
    {
        if (FirstNumber < 0 || SecondNumber < 0 || FirstNumber >= SecondNumber)
        {
            Result = "Please provide valid numbers";
            return;
        }

        Result = string.Empty;
        IsCalculating = true;

        var result = Math.CalculateLowestCommonMultiple(FirstNumber, SecondNumber);

        IsCalculating = false;
        Result = result == -1L ?
            "Lowest common multiple is too large for 64 bit number" :
            result.ToString("N0");
    }

    private async void CalculateOnBackgroundThread()
    {
        if (FirstNumber < 0 || SecondNumber < 0 || FirstNumber >= SecondNumber)
        {
            Result = "Please provide valid numbers";
            return;
        }

        Result = string.Empty;
        IsCalculating = true;

        var result = await Math.CalculateLowestCommonMultipleAsync(FirstNumber, SecondNumber);

        IsCalculating = false;
        Result = result == -1L ?
            "Lowest common multiple is too large for 64 bit number" :
            result.ToString("N0");
    }

    private void CalculateOnBackgroundThreadDecompiled()
    {
        var stateMachine = new AsyncStateMachine
        {
            Builder = AsyncVoidMethodBuilder.Create(),
            ViewModel = this,
            State = -1
        };
        stateMachine.Builder.Start(ref stateMachine);
    }

    #nullable disable
    private struct AsyncStateMachine : IAsyncStateMachine
    {
        public MainWindowViewModel ViewModel;
        public AsyncVoidMethodBuilder Builder;
        public TaskAwaiter<long> TaskAwaiter;

        // -2 = done (successful or exception caught), -1 = running, other states for different await statements
        public int State;

        public void MoveNext()
        {
            try
            {
                if (State == -2)
                    return;

                if (State == 0)
                    goto GetResultFromTaskAwaiter;
            
                if (ViewModel.FirstNumber < 0 || ViewModel.SecondNumber < 0 || ViewModel.FirstNumber >= ViewModel.SecondNumber)
                {
                    ViewModel.Result = "Please provide valid numbers";
                    goto Complete;
                }

                ViewModel.Result = string.Empty;
                ViewModel.IsCalculating = true;

                var taskAwaiter = Math.CalculateLowestCommonMultipleAsync(ViewModel.FirstNumber, ViewModel.SecondNumber)
                                      .GetAwaiter();
                if (taskAwaiter.IsCompleted)
                    goto Continuation;

                State = 0;
                TaskAwaiter = taskAwaiter;
                Builder.AwaitOnCompleted(ref TaskAwaiter, ref this);
                return;

                GetResultFromTaskAwaiter:
                taskAwaiter = TaskAwaiter;
                TaskAwaiter = default;
                State = -1;
            
                Continuation:
                var result = taskAwaiter.GetResult();

                ViewModel.IsCalculating = false;
                ViewModel.Result = result == -1L ?
                    "Lowest common multiple is too large for 64 bit number" :
                    result.ToString("N0");
            
                Complete:
                State = -2;
                Builder.SetResult();
            }
            catch (Exception exception)
            {
                State = -2;
                Builder.SetException(exception);
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) => Builder.SetStateMachine(stateMachine);
    }
    #nullable restore
}