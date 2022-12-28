using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TemporaryHiding
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    class App : IExternalApplication
    {
        static string AddInPath = typeof(App).Assembly.Location;
        public Result OnStartup(UIControlledApplication app)
        {
            CreateRibbon(app);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        private void CreateRibbon(UIControlledApplication app)
        {
            string PanelName = "УтилитыКазГАП";
            string TabName = "АО Казанский Гипронииавиапром";

            try
            {
                app.CreateRibbonTab(TabName);
            }
            catch { }

            RibbonPanel ribbonAOKazGAP = null;

            List<RibbonPanel> panels = app.GetRibbonPanels(TabName);
            foreach (RibbonPanel rb in panels)
            {
                if (rb.Name == PanelName)
                {
                    ribbonAOKazGAP = rb;
                    continue;
                }
            }
            if (ribbonAOKazGAP == null)
            {
                ribbonAOKazGAP = app.CreateRibbonPanel(TabName, PanelName);
            }

            ribbonAOKazGAP.Visible = true;

            PushButtonData pushButtonData = new PushButtonData("TemporaryHiding", "Скрытие/Изоляция\nпо категориям", AddInPath, "TemporaryHiding.Command");
            pushButtonData.LargeImage = convertFromBitmap(Properties.Resources.Icon);
            pushButtonData.ToolTip = "Плагин выполняет функции временного скрытия/изоляции для элементов одной категории(с учетом связанных с ней) по выбранному параметру.";
            PushButton pushButton = ribbonAOKazGAP.AddItem(pushButtonData) as PushButton;
        }
        BitmapSource convertFromBitmap(System.Drawing.Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
