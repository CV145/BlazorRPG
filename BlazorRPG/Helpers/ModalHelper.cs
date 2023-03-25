using Blazorise;

namespace BlazorRPG.Helpers
{
    public class ModalHelper
    {
        public Modal? ModalRef { get; set; } = null;

        public void ShowModal() => ModalRef?.Show();

        public void HideModal() => ModalRef?.Hide();
    }
}
