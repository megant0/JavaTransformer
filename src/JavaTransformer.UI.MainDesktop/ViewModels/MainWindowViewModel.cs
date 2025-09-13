namespace JavaTransformer.UI.MainDesktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public string MenuItemHeaderView { get; } = "Вид";

        public string MenuItemHeaderFile { get; } = "Файл";
        public string MenuItemHeaderFile_Save { get; } = "Сохранить";

        public string MenuItemHeaderBuild { get; } = "Сборка";
        public string MenuItemHeaderBuild_Launch { get; } = "Запуск";

    }
}
