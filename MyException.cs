using System;
namespace SecondForms
{
    public class MyException : Exception
    {   public  string Message1 { get; set; }
        public MyException() { }

        public MyException(string message)
            : base(message) {
            Message1 = message;
               }

        public MyException(string message, Exception inner)
            : base(message, inner) { }
    }
}
