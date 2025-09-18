using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.TextMate;
using JavaTransformer.UI.MainDesktop.Models.Common.DataTransferObject.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextMateSharp.Grammars;
using static System.Net.Mime.MediaTypeNames;

namespace JavaTransformer.UI.MainDesktop.Models.Extension
{
    public static class TextEditorExtension
    {
        public static void LoadSettings(this TextEditor _textEditor, EditorSettings settings)
        {
            _textEditor.WordWrap = settings.WordWrap;
            _textEditor.ShowLineNumbers = settings.LineNumbers;
            _textEditor.Options.AllowToggleOverstrikeMode = settings.AllowToggleOverstrikeMode;
            _textEditor.Options.EnableTextDragDrop = settings.EnableTextDragDrop;
            _textEditor.Options.ShowBoxForControlCharacters = settings.ShowBoxForControlCharacters;
            _textEditor.TextArea.RightClickMovesCaret = settings.RightClickMovesCaret;
            _textEditor.Options.HighlightCurrentLine = settings.HighlightCurrentLine;

            var _registryOptions = new TextMateSharp.Grammars.RegistryOptions(Enum.Parse<ThemeName>(settings.Theme));
            var option = _registryOptions.GetLanguageByExtension(settings.LanguageDefault);
          
            var _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);
            _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(option.Id));
        }
    }
}
