using System;

using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecondForms
{



        class Program
        {

       

            static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
            {
                Console.WriteLine(sender.GetType());
                MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
                // here you can log the exception ...
            }

            static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
            {
                Exception e = (Exception)args.ExceptionObject;
                Console.WriteLine("MyHandler caught : " + e.Message);
                Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
                Console.WriteLine("!!!!!!!!");
                MessageBox.Show((args.ExceptionObject as Exception).Message, "Unhandled UI Exception");
                // here you can log the exception ...
            }
            static void Main()
            {

               
                Application.ThreadException +=
    Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException +=
    CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            try
            {
                // Application.Run(new Form3());
              
                Application.Run(new Quiz());
            }
            catch (Exception e)
            {
               
                Console.WriteLine("MAin thread Exception: "+ e.Message);

            }




        }
        private static void CheckContext()
        {

            // проверим наличие контекста синхронизации
            var context = SynchronizationContext.Current;
            if (context == null)
                MessageBox.Show("No context 55 for this thread");
            else
                MessageBox.Show("We 55 got a context");

            // создадим форму
            Form1 form = new Form1();

            // проверим наличие контекста синхронизации еще раз
            context = SynchronizationContext.Current;

            if (context == null)
                MessageBox.Show("No context for this thread");
            else
                MessageBox.Show("We got a context");

            //if (context == null)
            //MessageBox.Show("No context for this thread");
        }

    }




}
