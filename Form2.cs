using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Quize
{
    class MyContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            Console.WriteLine("MyContext.Post");
            base.Post(d, state);
         
        }
        public override void Send(SendOrPostCallback d, object state)
        {
            Console.WriteLine("MyContext.Send");
            base.Send(d, state);
        }

    }
    static class E {
        public static void PostRight(this SynchronizationContext context, SendOrPostCallback d, object state)
        {
            var postAction = d;

            if (context is SynchronizationContext winFormsContext)
                postAction = s =>
                {
                    try
                    {
                        Console.WriteLine(" in try PostRight block");
                        d(s);
                    }
                    catch (MyException  e)
                    {
                        int id = Thread.CurrentThread.ManagedThreadId;
                        Console.WriteLine("Run thread in MyException :  " + id + " "+e.Message1  );
                       
                    } 
                };

            context.Post(postAction, state);
        }
    }

    public class Form2 : Form
    {
        private ListBox mListBox;
        private ToolStrip toolStrip1;
        private ToolStripButton mToolStripButtonThreads;
        public Form2()
        {
            // смотри id потока
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(" Form2() thread: " + id);
            InitializeComponent();

        }

        private void mToolStripButtonThreads_Click(object sender, EventArgs e)
        {
            // посмотрим id потока
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("mToolStripButtonThreads_Click thread: " + id);

         //  SynchronizationContext.SetSynchronizationContext(new MyContext());
            SynchronizationContext uiContext = SynchronizationContext.Current;

            // Создадим поток и зададим ему метод Run для исполнения
            Thread thread = new Thread(Run);

            // Запустим поток и установим ему контекст синхронизации,
            // таким образом этот поток сможет обновлять UI
            thread.Start(uiContext);
        }

        private void Run(object state)
        {
            SynchronizationContext perant = state as SynchronizationContext;

            // смотри id потока

            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Run thread: " + id);


            for (int i = 0; i < 1; i++)
            {
                //Thread.Sleep(10);              

                try
                {
                    // perant.Post(UpdateUI, "line " + i.ToString()); 
                    //   perant.Send(UpdateUI, "line " + i.ToString()); 
                 //   perant.Post(UpdateUI2, "line " + i.ToString());
                   perant.PostRight(UpdateUI2, "line " + i.ToString());
                    //   perant.Send(UpdateUI2, "line " + i.ToString()); 
                }
                catch (MyException e)
                {

                    Console.WriteLine("Run thread MyException: " + id + " " + e.Message1);

                }
                catch (System.Reflection.TargetInvocationException e)
                {
                    MyException my = e.InnerException as MyException;
                    Console.WriteLine("Run thread TargetInvocationException-> MyException: " + id + " " + my?.Message1);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Run thread Exception: " + id + " " + e?.Message);

                }

            }
        }


        private void UpdateUI(object state)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("UpdateUI thread:" + id);
            string text = state as string;
            mListBox.Items.Add(text);
        }
        private void UpdateUI2(object state)
        {
              throw new MyException("Boom");
        }






        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
              new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.mListBox = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mToolStripButtonThreads = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // mListBox
            //
            this.mListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mListBox.FormattingEnabled = true;
            this.mListBox.Location = new System.Drawing.Point(50, 50);
            this.mListBox.Name = "mListBox";
            this.mListBox.Size = new System.Drawing.Size(230, 230);
            this.mListBox.TabIndex = 0;
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.mToolStripButtonThreads});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            //
            // mToolStripButtonThreads
            //
            this.mToolStripButtonThreads.DisplayStyle =
              System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            //this.mToolStripButtonThreads.Image = ((System.Drawing.Image)
            //(resources.GetObject("mToolStripButtonThreads.Image")));

            this.mToolStripButtonThreads.ImageTransparentColor =
               System.Drawing.Color.Magenta;

            this.mToolStripButtonThreads.Name = "mToolStripButtonThreads";
            this.mToolStripButtonThreads.Size = new System.Drawing.Size(148, 22);
            this.mToolStripButtonThreads.Text = "Press Here to start threads";
            this.mToolStripButtonThreads.Click +=

              new System.EventHandler(this.mToolStripButtonThreads_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.mListBox);
            this.Controls.Add(this.toolStrip1);

            this.Name = "Form2";
            this.Text = "Form2";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
