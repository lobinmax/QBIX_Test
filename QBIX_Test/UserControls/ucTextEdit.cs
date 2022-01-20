using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

using System.ComponentModel;

namespace QBIX_Test
{
    [UserRepositoryItem("Register_ucTextEdit")]
    public class RepositoryItemQbixButtonEdit : RepositoryItemButtonEdit
    {
        static RepositoryItemQbixButtonEdit()
        {
            RegisterAmpereButtonEdit();
        }
        public const string CustomEditName = "ucTextEdit";

        public override string EditorTypeName => CustomEditName;

        public static void RegisterAmpereButtonEdit()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(QbixButtonEdit), typeof(RepositoryItemQbixButtonEdit), typeof(QbixButtonEditViewInfo), new QbixButtonEditPainter(), true, null));
        }

        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                if (!(item is RepositoryItemQbixButtonEdit)) return;
                //
            }
            finally
            {
                EndUpdate();
            }
        }
    }

    [ToolboxItem(true)]
    public sealed class QbixButtonEdit : ButtonEdit
    {
        static QbixButtonEdit()
        {
            RepositoryItemQbixButtonEdit.RegisterAmpereButtonEdit();
        }

        public QbixButtonEdit()
        {
            this.EnterMoveNextControl = true;
            Properties?.Buttons?.Clear();
            var btnClear = new EditorButton();
            btnClear.Image = QBIX_Test.Properties.Resources.Editor_Clear;
            btnClear.ToolTip = "Очистить поле";
            btnClear.Kind = ButtonPredefines.Glyph;
            Properties?.Buttons?.Add(btnClear);

            this.ButtonClick += BtnClear_Click;
        }

        private void BtnClear_Click(object sender, ButtonPressedEventArgs e)
        {
            var btn = e.Button;
            if (((ButtonEdit)sender).ReadOnly) return;

            if (btn.Index == 0)
            {
                this.EditValue = null;
                this.Text = null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemQbixButtonEdit Properties => base.Properties as RepositoryItemQbixButtonEdit;

        public override string EditorTypeName => RepositoryItemQbixButtonEdit.CustomEditName;
    }

    public class QbixButtonEditViewInfo : ButtonEditViewInfo
    {
        public QbixButtonEditViewInfo(RepositoryItem item) : base(item)
        {
        }
    }

    public class QbixButtonEditPainter : ButtonEditPainter
    {
    }
}
